using System.Threading.Tasks;

namespace MarketPlace.CommandHandler
{
    public interface ICommandHandler
    {
        Task Handle(object command);
    }
}
