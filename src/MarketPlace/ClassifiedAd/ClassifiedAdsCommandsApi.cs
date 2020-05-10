using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.ClassifiedAd
{
    [Route("v1/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private readonly ClassifiedAdsService appService;

        public ClassifiedAdsCommandsApi(ClassifiedAdsService appService)
        {
            this.appService = appService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Contracts.V1.Create request)
        {
            await appService.Handle(request);
            return Ok();
        }

        [Route("title")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.V1.SetTitle request)
        {
            await appService.Handle(request);
            return Ok();
        }

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.V1.UpdateText request)
        {
            await appService.Handle(request);
            return Ok();
        }

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.V1.UpdatePrice request)
        {
            await appService.Handle(request);
            return Ok();
        }

        [HttpPut]
        [Route("request-publish")]
        public async Task<IActionResult> Put(Contracts.V1.RequestToPublish request)
        {
            await appService.Handle(request);
            return Ok();
        }

    }
}
