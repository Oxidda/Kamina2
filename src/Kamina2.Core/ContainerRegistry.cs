using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kamina2.Core.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Kamina2.Core
{
    public class ContainerRegistry : IContainerRegistry
    {
        public void RegisterServices(IServiceCollection serviceCollection)
        {
#if DEBUG
            serviceCollection.AddScoped<ILogger, SimpleConsoleLogger>();
#endif
        }
    }
}
