using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Serilog;
using static MarketPlace.ClassifiedAd.QueryModels;

namespace MarketPlace.ClassifiedAd
{
    [Route("v1/add")]
    public class ClassifiedAdsQueryController
    {
        private readonly IAsyncDocumentSession session;

        public ClassifiedAdsQueryController(IAsyncDocumentSession session) => this.session = session;

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Get(GetPublishedClassifiedAds request) =>
            await Get(s => s.Query(request));

        [HttpGet]
        [Route("myads")]
        public async Task<IActionResult> Get(GetClassifiedAdsOwnedBy request) =>
            await Get(s => s.Query(request));


        [HttpGet]
        public async Task<IActionResult> Get(GetPublicClassifiedAd request) =>
            await Get(s => s.Query(request));


        private async Task<IActionResult> Get<TResult>(Func<IAsyncDocumentSession,Task<TResult>> query)
        {
            try
            {
                var result = await query(session);
                if (result == null) return new NotFoundResult();
                return new OkObjectResult(result);
            } catch (Exception e)
            {
                Log.Error(e, e.Message);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}

