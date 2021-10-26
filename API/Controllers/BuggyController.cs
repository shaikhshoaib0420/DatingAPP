using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){
            
            var thing = _context.Users.Find(-1);
            
            if(thing == null) return new NotFoundResult();

            return new OkObjectResult(thing);
        }

         [HttpGet("server-error")]
        public ActionResult<string> GetServerError(){
            
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

         [HttpGet("bad-request")]
        public ActionResult<string> GetSecret(){
            return new BadRequestObjectResult("This was not a good request");
        }

        //  [HttpGet("auth")]
        // public ActionResult<string> GetSecret(){
        //     return " secrete text";
        // }

        //  [HttpGet("auth")]
        // public ActionResult<string> GetSecret(){
        //     return " secrete text";
        // }

        //  [HttpGet("auth")]
        // public ActionResult<string> GetSecret(){
        //     return " secrete text";
        // }

        //  [HttpGet("auth")]
        // public ActionResult<string> GetSecret(){
        //     return " secrete text";
        // }


    }
}