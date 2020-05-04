﻿using System;
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
        private readonly IClassifiedAdRepository repository;
        private readonly ICurrencyLookup currencyLookup;

        public ClassifiedAdsService(IClassifiedAdRepository repository, ICurrencyLookup currencyLookup)
        {
            this.repository = repository;
            this.currencyLookup = currencyLookup;
        }

        public async Task Handle(object command)
        {
            Task HandleUnknownCommand(object command) =>
                throw new ArgumentException($"Unsupported command type: {command.GetType().Name}");

            var task = command switch
            {
                ClassifiedAds.V1.Create c => HandleCreate(() =>
                    new ClassifiedAd(new ClassifiedAdId(c.Id), new UserId(c.OwnerId))),
                ClassifiedAds.V1.SetTitle c => HandleUpdate(c.Id, ad =>
                    ad.SetTitle(ClassifiedAdTitle.FromTextOrHtml(c.Title))),
                ClassifiedAds.V1.UpdateText c => HandleUpdate(c.Id, ad =>
                    ad.UpdateText(ClassifiedAdText.FromString(c.Text))),
                ClassifiedAds.V1.UpdatePrice c => HandleUpdate(c.Id, ad =>
                    ad.UpdatePrice(Price.FromDecimal(c.Amount, c.CurrencyCode, currencyLookup))),
                ClassifiedAds.V1.RequestToPublish c => HandleUpdate(c.Id, ad =>
                    ad.RequestToPublish()),
                _ => HandleUnknownCommand(command)
            };
            await task;
        }

        private async Task HandleCreate(Func<ClassifiedAd> creator)
        {
            var ad = creator();
            var exists = await repository.exists(ad.Id);
            if (exists) throw new InvalidOperationException("ClassifiedAd with same id already exists");
            await repository.Add(ad);
        }

        private async Task HandleUpdate(Guid id, Action<ClassifiedAd> action)
        {
            var ad = await repository.Load(new ClassifiedAdId(id));
            if (ad == null) throw new ArgumentException($"Invalid ad id: {id}");
            action(ad);
            // TODO: repository save?
        }

    }
}

