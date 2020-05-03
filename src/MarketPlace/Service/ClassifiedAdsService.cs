using System;
using System.Threading.Tasks;
using MarketPlace.CommandHandler;
using MarketPlace.Contracts;
using MarketPlace.Domain;
using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Domain.Monetization;

namespace MarketPlace.Service
{
    public class ClassifiedAdsService : ICommandHandler
    {
        private readonly ICurrencyLookup currencyLookup;

        public ClassifiedAdsService(ICurrencyLookup currencyLookup)
        {
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
            // TODO: check Id, ownerId somehow
            var ad = new ClassifiedAd(new ClassifiedAdId(command.Id), new UserId(command.OwnerId));
            // TODO: persist entity somehow
        }

        private async Task Handle(ClassifiedAds.V1.SetTitle command)
        {
            var ad = GetById(new ClassifiedAdId(command.Id));
            ad.SetTitle(ClassifiedAdTitle.FromTextOrHtml(command.Title));
        }

        private async Task Handle(ClassifiedAds.V1.UpdateText command)
        {
            var ad = GetById(new ClassifiedAdId(command.Id));
            ad.UpdateText(ClassifiedAdText.FromString(command.Text));
        }

        private async Task Handle(ClassifiedAds.V1.UpdatePrice command)
        {
            var ad = GetById(new ClassifiedAdId(command.Id));
            ad.UpdatePrice(Price.FromDecimal(command.Amount, command.CurrencyCode, currencyLookup));
        }

        private async Task Handle(ClassifiedAds.V1.RequestToPublish command)
        {
            var ad = GetById(new ClassifiedAdId(command.Id));
            ad.RequestToPublish();
        }

        private ClassifiedAd GetById(ClassifiedAdId id) => new ClassifiedAd(new ClassifiedAdId(id), new UserId(id));
    }
}

