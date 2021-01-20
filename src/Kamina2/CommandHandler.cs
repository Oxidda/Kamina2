using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Kamina2.Core.Commands;
using Kamina2.Core.Logging;

namespace Kamina2
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordShardedClient client;
        private readonly CommandService commands;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;

        // Retrieve client and CommandService instance via ctor
        public CommandHandler(
            DiscordShardedClient client, 
            CommandService commands, 
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            this.commands = commands ?? throw new ArgumentNullException(nameof(commands));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task InitializeAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.LoadFrom("Kamina2.Commands.dll"), serviceProvider);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;
            
            if (!(message.HasCharPrefix('>', ref argPos) ||
                message.HasMentionPrefix(client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new ShardedCommandContext(client, message);
            await logger.LogAsync($"Found something to do: {message}");
            await commands.ExecuteAsync(context, argPos, serviceProvider);
        }
    }
}
