using System;
using System.Threading.Tasks;

namespace MarketPlace.Service
{
    public interface IFailoverPolicy
    {
        Task ExecuteAsync(Func<Task> action);
    }
}
