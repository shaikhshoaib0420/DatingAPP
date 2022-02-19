using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using API.Extensions;
using API.DTOs;
using API.Hoppers;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
// using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    // [ApiController]
    // [Route("api/[controller]")]

//    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        //using userRepository instead Datacontext
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IPhotoService _photoService;

        public UsersController(DataContext context, IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _HttpContextAccessor = httpContextAccessor;
            _photoService = photoService;
            _mapper = mapper;
            _context = context;
            _userRepository = userRepository;
        }

        [HttpGet]
        //Normal method (Syncronous) 
        // public ActionResult<IEnumerable<AppUser>> GetUsers()
        // {
        //     var users = _context.Users.ToList();  Normal way
        //     return users;
        // }

        //Asyncronous way 
        // public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()

        //USING MemberDto instead of AppUser 
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            // var users = await _context.Users.ToListAsync();  
            // var users = await _userRepository.GetUsersAsync();

            // var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
             var token = Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
            var hndler = new JwtSecurityTokenHandler();
            var jsonToken = hndler.ReadJwtToken(token) as JwtSecurityToken;
            var username = jsonToken.Claims.First(claims => claims.Type == "nameid").Value;

            var user = await _userRepository.GetUserByUsernameAsync(username);
            // return new OkObjectResult(user);
            // userParams.CurrentUsername = "";
            userParams.CurrentUsername = user.UserName;
            
            // userParams.CurrentUsername = User.GetUsername();

            if(string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female": "male";

            var users = await _userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader( users.CurrentPage, users.PageSize, 
                users.TotalCount, users.TotalPages );
            // return new OkObjectResult(users);
            return new OkObjectResult(users) ;


        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var USER = HttpContext.
           // var username = _HttpContextAccessor.HttpContext.User.GetUsername();
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
            var hndler = new JwtSecurityTokenHandler();
            var jsonToken = hndler.ReadJwtToken(token) as JwtSecurityToken;
            var username = jsonToken.Claims.First(claims => claims.Type == "nameid").Value;

            if (username == null) return new BadRequestObjectResult("USER is null");
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, user);
           
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return new NoContentResult();
            return new BadRequestObjectResult("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file){


            var username = _HttpContextAccessor.HttpContext.User.GetUsername();
            if(username != null) return new BadRequestObjectResult("USER is not null");
            var user = await _userRepository.GetUserByUsernameAsync(username);


            var result = await _photoService.AddPhotoAsync(file);
            
            if (result.Error != null) return new BadRequestObjectResult(result.Error.Message);

            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId

            };

            if(user.Photos.Count == 0){
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            
            {
                // return _mapper.Map<PhotoDto>(photo);
                //Gives the route that is user name and controller where photo has been added.
                return CreatedAtRoute("GetUser", new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }

            return new BadRequestObjectResult("Problem in adding photo");

        }

        // [HttpGet]
        // [Route("api/users/")]
        // public MemberDto GetDemo(){
        //     MemberDto test = new MemberDto{
        //         Age = 25,
        //         Country = "India"
        //     };
        //     return test;
        // }

        // [HttpGet("{id}")]
        // //api/3
        // // public ActionResult<AppUser> GetUser(int id)
        // public ActionResult<MemberDto> GetUser(int id)
        // {
        //     // var user = _context.Users.Find(id);
        //     var user = _userRepository.GetUserByIdAsync(id);

        //      var userToReturn = _mapper.Map<MemberDto>(user);

        //     return new OkObjectResult(userToReturn);
        // }

        [HttpGet("{username}" , Name = "GetUser")]
        //api/3
        // public ActionResult<AppUser> GetUser(int id)
        public async Task<ActionResult<MemberDto>> GetUserasync(string username)
        {
            // var user = _context.Users.Find(id);
            // var user = await _userRepository.GetUserByUsernameAsync(username);

            var user = await _userRepository.GetMemberAsync(username);

            //  var userToReturn = _mapper.Map<MemberDto>(user);

            // return _mapper.Map<MemberDto>(user);

            return user;
        }

       [HttpPut("set-main-photo/{photoId}")]

      public async Task<ActionResult> SetMainPhoto(int photoId)
      {

          var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

         var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if(photo.IsMain == true) return new BadRequestObjectResult("The photo is already main");

        var CurrentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if(CurrentMain != null) CurrentMain.IsMain = false; 
        photo.IsMain = true;

        if(await _userRepository.SaveAllAsync()) return new NoContentResult();

        return new BadRequestObjectResult("Failed to update photo");
      }


    }
}