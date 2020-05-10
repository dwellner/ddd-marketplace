using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.UserProfile
{
    [Route("v1/userprofile")]
    public class UserProfileCommandsApi : Controller
    {
        private readonly UserProfileService appService;

        public UserProfileCommandsApi(UserProfileService appService)
        {
            this.appService = appService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Contracts.V1.Create request)
        {
            await appService.Handle(request);
            return Ok();
        }

        [Route("fullname")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.V1.UpdateFullName request)
        {
            await appService.Handle(request);
            return Ok();
        }

        [Route("displayname")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.V1.UpdateDisplayName request)
        {
            await appService.Handle(request);
            return Ok();
        }

        [Route("photo-url")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.V1.MarkProfilePhotoUploaded request)
        {
            await appService.Handle(request);
            return Ok();
        }
    }
}
