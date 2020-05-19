using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Domain.UserProfile;
using MarketPlace.Service;
using static MarketPlace.Projections.ReadModels;

namespace MarketPlace.Projections
{
    public class UserDetailsProjection : IProjection
    {
        private readonly List<UserDetails> items;


        public UserDetailsProjection(List<UserDetails> items) => this.items = items;

        public Task Project(object @event)
        {
            switch (@event)
            {
                case Events.UserProfileCreated e:
                    items.Add(new UserDetails
                    {
                        Id = e.UserId,
                        Displayname = e.DisplayName,
                        Fullname = e.FullName
                    });
                    break;
                case Events.UserProfileDisplayNameUpdated e:
                    UpdateItem(e.UserId, item => item.Displayname= e.DisplayName);
                    break;
                case Events.UserProfileFullNameUpdated e:
                    UpdateItem(e.UserId, item => item.Fullname = e.FullName);
                    break;
            }
            return Task.CompletedTask;
        }

        private void UpdateItem(Guid id, Action<UserDetails> action)
        {
            var item = items.FirstOrDefault(item => item.Id == id);
            if (item != null) action(item);
        }

    }
}
