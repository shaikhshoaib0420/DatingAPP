using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletPhotoAsync(string publicId);
        
        //  Task<IEnumerable<AppUser>> GetUsersAsync();
    }
}