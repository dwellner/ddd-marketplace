using System.Threading.Tasks;

namespace MarketPlace.Service
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
