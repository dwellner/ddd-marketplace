using System.Threading.Tasks;
using MarketPlace.CommandHandler;
using MarketPlace.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private readonly ICommandHandler commandHandler;

        public ClassifiedAdsCommandsApi(ICommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClassifiedAds.V1.Create request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

        [Route("/title")]
        [HttpPut]
        public async Task<IActionResult> Put(ClassifiedAds.V1.SetTitle request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

        [Route("/text")]
        [HttpPut]
        public async Task<IActionResult> Put(ClassifiedAds.V1.UpdateText request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

        [Route("/price")]
        [HttpPut]
        public async Task<IActionResult> Put(ClassifiedAds.V1.UpdatePrice request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(ClassifiedAds.V1.RequestToPublish request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

    }
}
