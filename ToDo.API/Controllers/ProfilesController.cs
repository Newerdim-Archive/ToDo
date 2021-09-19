using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Const;
using ToDo.API.Helpers;
using ToDo.API.Services;

namespace ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProfilesController : CustomControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProfileService _profileService;

        public ProfilesController(IUserService userService, IProfileService profileService)
        {
            _userService = userService;
            _profileService = profileService;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentAsync()
        {
            var currentUserId = _userService.GetCurrentId();

            var profile = await _profileService.GetByUserIdAsync(currentUserId);

            if (profile is null)
            {
                return Unauthorized(ResponseMessage.UserNotExist);
            }

            return Ok(ResponseMessage.GotCurrentProfileSuccessfully, profile);
        }
    }
}