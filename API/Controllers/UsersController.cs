using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
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
        public UsersController(DataContext context){
            _context = context;
            }

            [HttpGet]
            //Normal method (Syncronous) 
            // public ActionResult<IEnumerable<AppUser>> GetUsers()
            // {
            //     var users = _context.Users.ToList();  Normal way
            //     return users;
            // }

            //Asyncronous way 
            public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
            {
            var users = await _context.Users.ToListAsync();  
            return users;

        
            }



             [HttpGet("{id}")]
            //api/3
            public ActionResult<AppUser> GetUser(int id)
            {
                var user = _context.Users.Find(id);
                return user;
            }
    }
}