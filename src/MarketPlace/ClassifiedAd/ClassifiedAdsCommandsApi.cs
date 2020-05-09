using System.Threading.Tasks;
using MarketPlace.CommandHandler;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.ClassifiedAd
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
        public async Task<IActionResult> Post(Contracts.V1.Create request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

        [Route("/title")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.V1.SetTitle request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

        [Route("/text")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.V1.UpdateText request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

        [Route("/price")]
        [HttpPut]
        public async Task<IActionResult> Put(Contracts.V1.UpdatePrice request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

        [HttpPut]
        [Route("/request-publish")]
        public async Task<IActionResult> Put(Contracts.V1.RequestToPublish request)
        {
            await commandHandler.Handle(request);
            return Ok();
        }

    }
}
