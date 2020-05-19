using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Domain.ClassifiedAd;
using MarketPlace.Service;
using static MarketPlace.Projections.ReadModels;

namespace MarketPlace.Projections
{
    public class ClassifiedAdDetailsProjection : IProjection
    {
        private readonly List<ClassifiedAdDetails> items;
        private readonly Func<Guid, string> getUserDisplayName;

        public ClassifiedAdDetailsProjection(List<ClassifiedAdDetails> items,
            Func<Guid, string> getUserDisplayName)
        {
            this.items = items;
            this.getUserDisplayName = getUserDisplayName;
        }

        public Task Project(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    items.Add(new ClassifiedAdDetails
                    {
                        ClassifiedAdId = e.Id,
                        SellerDisplayName = getUserDisplayName(e.OwnerId)
                    });
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    UpdateItem(e.Id, item => item.Title = e.AdTitle);
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    UpdateItem(e.Id, item => item.Description = e.AdText);
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    UpdateItem(e.Id, item => {
                        item.Price = e.Price;
                        item.CurrencyCode = e.CurrenctCode;
                    });
                    break;

            }
            return Task.CompletedTask;
        }

        private void UpdateItem(Guid id, Action<ClassifiedAdDetails> action)
        {
            var item = items.FirstOrDefault(item => item.ClassifiedAdId == id);
            if (item != null) action(item);
        }

    }
}
