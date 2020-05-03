using System.Threading.Tasks;

namespace MarketPlace
{
    public interface ICommandHandler
    {
        Task Handle(object command);
    }
}
