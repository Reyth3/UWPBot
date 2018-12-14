using Discord.Commands;
using Discord.Net.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UWPBot.Commands
{
    public class Developer : ModuleBase<ExtendedCommandContext>
    {
        [Command("iam"), Alias("dev", "im"), Summary("Get the developer role")]
        public async Task DevCommand([Remainder]string appName)
        {
            if(appName.Length < 3)
            {
                await ReplyAsync("*The app name is too short.*");
                return;
            }
            else if (appName.Length > 48)
            {
                await ReplyAsync("*The app name is too long.");
                return;
            }
            Context.GuildSpecificPreferences.AddOrUpdate(Context.User.Id.ToString(), appName);
            await ReplyAsync("*Your request will be reviewed shortly.*");
        }
    }
}
