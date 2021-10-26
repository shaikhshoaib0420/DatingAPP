using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // [ApiController]
    // [Route("api/[controller]")]

    // public class UsersController : ControllerBase
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        //using userRepository instead Datacontext
        private readonly IMapper _mapper;
        public UsersController(DataContext context, IUserRepository userRepository, IMapper mapper)
        {
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
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // var users = await _context.Users.ToListAsync();  
            // var users = await _userRepository.GetUsersAsync();
            
            // var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

            var users = await _userRepository.GetMembersAsync();

            return new OkObjectResult(users);


        }



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

              [HttpGet("{username}")]
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
    }
}