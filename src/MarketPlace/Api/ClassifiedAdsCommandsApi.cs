using System.Threading.Tasks;
using MarketPlace.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Api
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        public ClassifiedAdsCommandsApi()
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClassifiedAds.V1.Create request)
        {
            return Ok();
        }
    }
}
