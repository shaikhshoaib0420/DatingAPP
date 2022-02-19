using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        
        [Required] public string Username { get; set; }
        
        
        [Required] public DateTime DateOfBirth { get; set; }

        [Required] public string KnownAs { get; set; }

        // public DateTime Created { get; set; } = DateTime.Now;

        // public DateTime LastActive{ get; set; } = DateTime.Now;

        [Required] public string Gender { get; set; }

        // [Required] public string Introduction { get; set; }
        // [Required] public string LookingFor { get; set; }

        // [Required] public string Interests { get; set; }
        [Required] public string Country { get; set; }

        [Required] public string City { get; set; }

        [Required]  
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }


    }
}