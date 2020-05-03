using System;
using System.Threading.Tasks;
using MarketPlace.CommandHandler;
using MarketPlace.Contracts;
using MarketPlace.Domain;
using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Domain.Monetization;
using MarketPlace.EntityStore;

namespace MarketPlace.Service
{
    public class ClassifiedAdsService : ICommandHandler
    {
        private readonly IEntityStore<ClassifiedAd> entityStore;
        private readonly ICurrencyLookup currencyLookup;

        public ClassifiedAdsService(IEntityStore<ClassifiedAd> entityStore, ICurrencyLookup currencyLookup)
        {
            this.entityStore = entityStore;
            this.currencyLookup = currencyLookup;
        }

        public async Task Handle(object command)
        {
            Task HandleUnknownCommand(object command) =>
                throw new ArgumentException($"Unsupported command type: {command.GetType().Name}");

            var task = command switch
            {
                ClassifiedAds.V1.Create c => Handle(c),
                ClassifiedAds.V1.SetTitle c => Handle(c),
                ClassifiedAds.V1.UpdateText c => Handle(c),
                ClassifiedAds.V1.UpdatePrice c => Handle(c),
                ClassifiedAds.V1.RequestToPublish c => Handle(c),
                _ => HandleUnknownCommand(command)
            };
            await task;
        }

        private async Task Handle(ClassifiedAds.V1.Create command)
        {
            var ad = new ClassifiedAd(new ClassifiedAdId(command.Id), new UserId(command.OwnerId));
            await entityStore.Create(ad);
        }

        private async Task Handle(ClassifiedAds.V1.SetTitle command)
        {
            var ad = await entityStore.GetById(new ClassifiedAdId(command.Id));
            ad.SetTitle(ClassifiedAdTitle.FromTextOrHtml(command.Title));
            await entityStore.Save(ad);
        }

        private async Task Handle(ClassifiedAds.V1.UpdateText command)
        {
            var ad = await entityStore.GetById(new ClassifiedAdId(command.Id));
            ad.UpdateText(ClassifiedAdText.FromString(command.Text));
            await entityStore.Save(ad);
        }

        private async Task Handle(ClassifiedAds.V1.UpdatePrice command)
        {
            var ad = await entityStore.GetById(new ClassifiedAdId(command.Id));
            ad.UpdatePrice(Price.FromDecimal(command.Amount, command.CurrencyCode, currencyLookup));
            await entityStore.Save(ad);
        }

        private async Task Handle(ClassifiedAds.V1.RequestToPublish command)
        {
            var ad = await entityStore.GetById(new ClassifiedAdId(command.Id));
            ad.RequestToPublish();
            await entityStore.Save(ad);
        }

    }
}

