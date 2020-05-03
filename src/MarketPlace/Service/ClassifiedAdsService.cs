using System;
using System.Threading.Tasks;
using MarketPlace.Contracts;
using MarketPlace.Domain;
using MarketPlace.Domain.ClassifiedAd;

namespace MarketPlace.Service
{
    public class ClassifiedAdsService : ICommandHandler
    {
        public async Task Handle(object command)
        {
            Task HandleUnknownCommand(object command) =>
                throw new ArgumentException($"Unsupported command type: {command.GetType().Name}");

            var task = command switch
            {
                ClassifiedAds.V1.Create c => Handle(c),
                _ => HandleUnknownCommand(command)
            };
            await task;
        }

        private async Task Handle(ClassifiedAds.V1.Create command) {
            // TODO: check Id, ownerId somehow
            var ad = new ClassifiedAd(new ClassifiedAdId(command.Id), new UserId(command.OwnerId));
            // TODO: persist entity somehow
        }

    }
}

