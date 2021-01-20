using System;
using System.Threading.Tasks;

namespace Kamina2.Core.Logging
{
    public class SimpleConsoleLogger : ILogger
    {
        public Task LogAsync(string message)
        {
            return Task.Run(() => Log(message));
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
