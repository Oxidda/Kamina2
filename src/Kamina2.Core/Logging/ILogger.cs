using System.Threading.Tasks;

namespace Kamina2.Core.Logging
{
    public interface ILogger
    { Task LogAsync(string message);
    }
}
