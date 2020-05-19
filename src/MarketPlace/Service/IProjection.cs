using System.Threading.Tasks;

namespace MarketPlace.Service
{
    public interface IProjection
    {
        Task Project(object @event);
    }
}
