using System;
using System.Threading.Tasks;
using MarketPlace.Domain;
using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Domain.Monetization;
using MarketPlace.Service;

namespace MarketPlace.ClassifiedAd
{
    public class ClassifiedAdsService
    {
        private readonly IClassifiedAdRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IFailoverPolicy failoverPolicy;
        private readonly ICurrencyLookup currencyLookup;

        public ClassifiedAdsService(IClassifiedAdRepository repository, IUnitOfWork unitOfWork,
            IFailoverPolicyProvider failoverPolicyProvider, ICurrencyLookup currencyLookup)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.failoverPolicy = failoverPolicyProvider.CommandRetryPolicy;
            this.currencyLookup = currencyLookup;
        }

        public async Task Handle(object command) =>
            await failoverPolicy.ExecuteAsync(async () => {
                switch (command)
                {
                    case Contracts.V1.Create c:
                        await HandleCreate(() =>
                            new Domain.ClassifiedAd.ClassifiedAd(new ClassifiedAdId(c.Id), new UserId(c.OwnerId)));
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

        private async Task HandleCreate(Func<Domain.ClassifiedAd.ClassifiedAd> creator)
        {
            var ad = creator();
            var exists = await repository.Exists(ad.Id);
            if (exists) throw new InvalidOperationException("ClassifiedAd with same id already exists");
            await repository.Add(ad);
            await unitOfWork.Commit();
        }

        private async Task HandleUpdate(Guid id, Action<Domain.ClassifiedAd.ClassifiedAd> action)
        {
            var ad = await repository.Load(new ClassifiedAdId(id));
            if (ad == null) throw new ArgumentException($"Invalid ad id: {id}");
            action(ad);
            await unitOfWork.Commit();
        }

    }
}

