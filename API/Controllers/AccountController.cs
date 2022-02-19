using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [EnableCors("MyPolicy")]
    public class AccountController : BaseApiController
    {

        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]

        // public async Task<ActionResult<AppUser>> Register(string username, string password) --- normal method
        // public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto) //Using DTOs(Data Tranasfer Object) method
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)  //token method 
        {   
            if (await UserExists(registerDto.Username)) 
            {
                return new BadRequestObjectResult("Username is taken");
            }
            // if (await UserExists(registerDto.Username)) return new BadRequestObjectResult("Username is taken");
        
            var user = _mapper.Map<AppUser>(registerDto);
            using var hmac = new HMACSHA512();

            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;


            // var user = new AppUser
            // {
            //     UserName = registerDto.Username,
            //     DateOfBirth = registerDto.DateOfBirth,
            //     KnownAs = registerDto.KnownAs,
            //     Gender = registerDto.Gender,
            //     // Introduction = registerDto.Introduction,
            //     // LookingFor = registerDto.LookingFor,
            //     // Interests = registerDto.Interests,
            //     City = registerDto.City,
            //     Country = registerDto.Country,
            //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //     PasswordSalt = hmac.Key
            // };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // return user;
            return new UserDto{
                Username = user.UserName,
                Tokekn = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username);
        }

        [HttpPost("login")]
        // public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.
            Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if(user == null) return new UnauthorizedObjectResult("Invalid Username");  //null = Unauthorized("Invalid Username")
            
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            
            for(int i=0; i<computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return  new UnauthorizedObjectResult("Invalid Password");; //Unauthorize("Invalid Password")
            }
            
        // return user;

        return new UserDto{

                Username = user.UserName,
                Tokekn = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
                // Password = user.PasswordHash

            };

        
        // return new UserDto{
        //         Username = user.UserName,
        //         Tokekn = _tokenService.CreateToken(user)
        //     };

            
    
        }
    //     [HttpPost("Test")]
    //    public ActionResult Test([FromBody] param)  //token method 
    //     {   
    
    //      return null;
    //     }


       
    }


}