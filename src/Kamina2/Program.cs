using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kamina2.Core.Common;
using Kamina2.Core.Configuration;
using Kamina2.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Kamina2
{
    public class Program
    {
        private static DiscordShardedClient client;
        private static Kamina2TokenConfiguration configuration;
        private static IServiceProvider serviceProvider;
        private static ILogger logger;

        public static async Task Main()
        {
            await Setup();
            await StartClient();
            await Task.Delay(-1);
        }

        private static async Task StartClient()
        {
            await client.LoginAsync(TokenType.Bot, configuration.Token);
            await client.StartAsync();
        }

        private static async Task Client_MessageReceived(SocketMessage arg)
        {
            await logger.LogAsync($"Message recieved {arg.Content}");
        }

        private static async Task Setup()
        {
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            configuration = await LoadConfiguration();
            serviceProvider = await RegisterServices();

            client = serviceProvider.GetRequiredService<DiscordShardedClient>();
            CommandHandler commandHandler = serviceProvider.GetRequiredService<CommandHandler>();
            logger = serviceProvider.GetService<ILogger>();
            await commandHandler.InitializeAsync();
            client.MessageReceived += Client_MessageReceived;
        }

        private static async Task<Kamina2TokenConfiguration> LoadConfiguration()
        {
            return JsonConvert.DeserializeObject<Kamina2TokenConfiguration>(File.ReadAllText("configuration.json"));
        }

        private static async Task<IServiceProvider> RegisterServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<DiscordShardedClient>();
            serviceCollection.AddSingleton<CommandHandler>();
            serviceCollection.AddSingleton<CommandService>();
            serviceCollection.AddSingleton<ITimeService>(new TimeService(DateTime.Now));

            new Commands.ContainerRegistry().RegisterServices(serviceCollection);
            new Core.ContainerRegistry().RegisterServices(serviceCollection);
            return serviceCollection.BuildServiceProvider();
        }

        public static void OnExit(object sender, EventArgs e)
        {
            Task.Run(async () => await client.LogoutAsync());
        }
    }
}
