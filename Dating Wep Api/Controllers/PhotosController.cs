using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dating_Wep_Api.Data.IRepository;
using Dating_Wep_Api.DTO;
using Dating_Wep_Api.Helpers;
using Dating_Wep_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Dating_Wep_Api.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudnarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudnarySettings> cloudinaryConfig)
        {
            this._repo = repo;
            this._mapper = mapper;
            this._cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                    _cloudinaryConfig.Value.CloudName,
                    _cloudinaryConfig.Value.ApiKey,
                    _cloudinaryConfig.Value.ApiSecret
                );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotosFOrUser(int userId, PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Url.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }

            userFromRepo.Photos.Add(photo);

            if (await _repo.SaveAll())
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
