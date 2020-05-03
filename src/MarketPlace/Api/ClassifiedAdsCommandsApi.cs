using System.Threading.Tasks;
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
    }
}
