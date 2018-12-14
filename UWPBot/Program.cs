using Discord.Net.Framework;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace UWPBot
{
    class Program
    {
        static DiscordBotFramework Bot;

        static void Main(string[] args) => Start(args).Wait();

        static async Task Start(string[] args)
        {
            Bot = new DiscordBotFramework("uwp.");
            await Bot.ImportCommandsAsync(Assembly.GetAssembly(typeof(Program)));
            await Bot.RunAsync();

            await Task.Delay(-1);
        }
    }
}
