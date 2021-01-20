using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Kamina2.Core.Common;

namespace Kamina2.Commands.Info
{
    public class InfoModule : ModuleBase<ShardedCommandContext>
    {
        private readonly ITimeService timeService;

        public InfoModule(ITimeService timeService)
        {
            this.timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }

        [Command("info")]
        [Summary
            ("Returns info about the current user, or the user parameter, if one passed.")]
        public async Task UserInfoAsync()
        {
            TimeSpan time = DateTime.Now - timeService.StartTime;
            var discordSocketClient = Context.Client;
            await ReplyAsync(
                $"{Format.Bold("Info")}\n" +
                $"- Author: Various people\n" +
                $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}\n" +
                $"- Uptime: Days: {time.Days} Hours: {time.Hours} Minutes: {time.Minutes}\n\n" +
                $"{Format.Bold("Stats")}\n" +
                $"- Heap Size: {GetHeapSize()} MB\n" +
                $"- Guilds: {discordSocketClient.Guilds.Count}\n" +
                $"- Channels: {discordSocketClient.Guilds.Sum(g => g.Channels.Count)}\n" +
                $"- Users: {discordSocketClient.Guilds.Sum(g => g.Users.Count)}"
            );
        }

        [Command("say")]
        [Alias("respond")]
        public async Task UserInfoAsync(string message)
        {
            await ReplyAsync($"You typed {message}");
        }

        private static string GetHeapSize()
            => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString(CultureInfo.InvariantCulture);
    }
}
