using System.Threading.Tasks;

namespace Kamina2.Core.Commands
{
    public interface ICommandHandler
    {
        Task InitializeAsync();
    }
}
