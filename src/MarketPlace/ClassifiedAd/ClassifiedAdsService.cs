using System;
using System.Threading.Tasks;
using MarketPlace.Domain;
using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Domain.Monetization;
using MarketPlace.Service;


namespace MarketPlace.ClassifiedAd
{
    public class ClassifiedAdsService: IApplicationService
    {
        private readonly IAggregateStore store;
        private readonly IFailoverPolicy failoverPolicy;
        private readonly ICurrencyLookup currencyLookup;

        public ClassifiedAdsService(IAggregateStore store, IFailoverPolicyProvider failoverPolicyProvider, ICurrencyLookup currencyLookup)
        {
            this.store = store;
            failoverPolicy = failoverPolicyProvider.CommandRetryPolicy;
            this.currencyLookup = currencyLookup;
        }

        public async Task Handle(object command) =>
            await failoverPolicy.ExecuteAsync(async () => {
                switch (command)
                {
                    case Contracts.V1.Create cmd:
                        await HandleCreate(cmd);
                        break;
                    case Contracts.V1.SetTitle c:
                        await HandleUpdate(c.Id, ad =>
                        ad.SetTitle(ClassifiedAdTitle.FromTextOrHtml(c.Title)));
                        break;
                    case Contracts.V1.UpdateText c:
                        await HandleUpdate(c.Id, ad => ad.UpdateText(ClassifiedAdText.FromString(c.Text)));
                        break;
                    case Contracts.V1.UpdatePrice c:
                        await HandleUpdate(c.Id, ad =>
                            ad.UpdatePrice(Price.FromDecimal(c.Amount, c.CurrencyCode, currencyLookup)));
                        break;
                    case Contracts.V1.RequestToPublish c:
                        await HandleUpdate(c.Id, ad => ad.RequestToPublish());
                        break;
                    default: throw new ArgumentException($"Unsupported command type: {command.GetType().Name}");
                };
            });

        private async Task HandleCreate(Contracts.V1.Create cmd)
        {
            var id = new ClassifiedAdId(cmd.Id);
            var exists = await store.Exists<Domain.ClassifiedAd.ClassifiedAd, ClassifiedAdId>(id);
            if (exists) throw new InvalidOperationException("ClassifiedAd with same id already exists");

            var ad = new Domain.ClassifiedAd.ClassifiedAd(id, new UserId(cmd.OwnerId));
            await store.Save<Domain.ClassifiedAd.ClassifiedAd, ClassifiedAdId>(ad);
        }

        private async Task HandleUpdate(Guid guid, Action<Domain.ClassifiedAd.ClassifiedAd> operation) =>
            await this.HandleUpdate(store, new ClassifiedAdId(guid), operation);
    }
}

