using Discord;
using Discord.Commands;
using Discord.Net.Framework;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPBot.Commands
{
    public class Mod : ModuleBase<ExtendedCommandContext>
    {
        public async Task<string> GetRequestedRole(string user)
        {
            var match = await Context.FindUser(user);
            if (!match.Success)
            {
                await ReplyAsync($"*{(match.Status == UserSearchResult.UserSearchResultStatus.MultipleMatches ? "Multiple matches found!" : "No matches found!")}*");
                return null;
            }
            var subject = match.Match;
            var roleName = Context.GuildSpecificPreferences.GetValue<string>(Context.User.Id.ToString(), null);
            if(roleName == null)
                await ReplyAsync("*This user didn't request a role.*");
            return roleName;
        }

        [Command("verify"), Summary("Approve user's role request")]
        public async Task VerifyCommand(string user)
        {
            var roleName = await GetRequestedRole(user);
            if(roleName != null)
            {
                var devRole = Context.Guild.Roles.FirstOrDefault(o => o.Name == "Developer");
                var specificRole = Context.Guild.Roles.FirstOrDefault(o => o.Name == roleName);
                if(specificRole == null) specificRole = await Context.Guild.CreateRoleAsync(roleName, color: new Color((uint)(Context.User.Id % 0xFFFFFF)));
                var u = await Context.Guild.GetUserAsync(Context.User.Id);
                await u.AddRolesAsync(new IRole[] { devRole, specificRole });
                await ReplyAsync("*Role verified successfully.*");
            }
        }

        [Command("assign"), Summary("Assign a role for an user manually")]
        public async Task AssignCommand(string user, [Remainder]string role)
        {
            var roleName = await GetRequestedRole(user);
            var match = await Context.FindUser(user);
            var u = match?.Match as SocketGuildUser;
            if (roleName != null)
            {
                var devRole = Context.Guild.Roles.FirstOrDefault(o => o.Name == "Developer");
                var specificRole = Context.Guild.Roles.FirstOrDefault(o => o.Name == roleName);
                if (specificRole == null) specificRole = await Context.Guild.CreateRoleAsync(roleName, color: new Color((uint)(Context.User.Id % 0xFFFFFF)));
                await u?.AddRolesAsync(new IRole[] { devRole, specificRole });
                await ReplyAsync("*Role assigned successfully.*");
            }
        }
    }
}
