using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Kamina2.Core
{
    public interface IContainerRegistry
    {
        void RegisterServices(IServiceCollection collection);
    }
}
