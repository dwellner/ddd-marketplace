using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static MarketPlace.ClassifiedAd.QueryModels;
using static MarketPlace.Projections.ReadModels;

namespace MarketPlace.ClassifiedAd
{
    [Route("v1/add")]
    public class ClassifiedAdsQueryController
    {
        private readonly IEnumerable<ClassifiedAdDetails> items;

        public ClassifiedAdsQueryController(IEnumerable<ClassifiedAdDetails> items) => this.items = items;

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


        private async Task<IActionResult> Get<TResult>(Func<IEnumerable<ClassifiedAdDetails>, Task<TResult>> query)
        {
            try
            {
                var result = await query(items);
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

