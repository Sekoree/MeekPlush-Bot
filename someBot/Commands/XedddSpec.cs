using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Net;

namespace someBot
{
    class XedddSpec : BaseCommandModule
    {
        [Command("role"), Description("get a role to access a vocaloid channel")] //yeet
        public async Task VocRole(CommandContext ctx, string role = null)
        {
            try
            {
                string ServerJSON = "";
                string Desc = "";
                string Title = "";
                if (ctx.Channel.GuildId == 373635826703400960) //Xeddd
                {
                    if (role == null)
                    {
                        await ctx.RespondAsync("Please look in <#467001692429484032> to see all available roles and command usage!");
                        return;
                    }
                    ServerJSON = "XedddGroups.json";
                    Desc = $"Please look in <#467001692429484032> to see all available roles!";
                    Title = $"Xeddd Vocaloid Role Select!";
                }
                else if (ctx.Channel.GuildId == 469661736534802432) //Rin
                {
                    if (role == null)
                    {
                        await ctx.RespondAsync("**__Available Roles:__**\n" +
                            "``rin, miku, len, una, aoki, luka, meiko, gumi, mayu, kaito, ia, maki``\n" +
                            "Usage: ``m!role RoleName``\n" +
                            "Example: ``m!role rin``");
                        return;
                    }
                    ServerJSON = "RinGroups.json";
                    Desc = $"To see all available roles just type ``m!role``";
                    Title = $"Rin Role Select!";
                }
                else return;
                role = role.ToLower();
                StreamReader r = new StreamReader(ServerJSON);
                string json = r.ReadToEnd();
                var roles = JsonConvert.DeserializeObject<List<RootObject>>(json);
                int select = roles.FindIndex(x => x.Name == role);
                DiscordEmbedBuilder RXEmbed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Description = Desc,
                    Title = Title
                };
                if (select == -1) return;
                //await ctx.RespondAsync(ctx.Channel.Guild.GetMemberAsync(ctx.Message.Author.Id).Result.Roles.Where(x => x.Id == Convert.ToUInt64(roles[select].RoleId)).ToString());
                if (!ctx.Channel.Guild.GetMemberAsync(ctx.Message.Author.Id).Result.Roles.Any(x => x.Id == Convert.ToUInt64(roles[select].RoleId)))
                {
                    await ctx.Channel.Guild.GetMemberAsync(ctx.Message.Author.Id).Result.GrantRoleAsync(ctx.Channel.Guild.GetRole(Convert.ToUInt64(roles[select].RoleId)));
                    RXEmbed.AddField("Added Role:", ctx.Channel.Guild.GetRole(Convert.ToUInt64(roles[select].RoleId)).Mention);
                }
                else
                {
                    try
                    {
                        await ctx.Channel.Guild.GetMemberAsync(ctx.Message.Author.Id).Result.RevokeRoleAsync(ctx.Channel.Guild.GetRole(Convert.ToUInt64(roles[select].RoleId)));
                        RXEmbed.AddField("Removed Role:", ctx.Channel.Guild.GetRole(Convert.ToUInt64(roles[select].RoleId)).Mention);
                    }
                    catch
                    {
                        await ctx.RespondAsync("You did something wrong there :/ \nIf you believe you did nothing wrong contact Speyd3r#3939");
                        return;
                    }
                }
                await Task.Delay(TimeSpan.FromMilliseconds(500));
                string demRoles = "-> ";
                foreach (var ee in ctx.Channel.Guild.GetMemberAsync(ctx.Message.Author.Id).Result.Roles)
                {
                    int newselect = roles.FindIndex(x => Convert.ToUInt64(x.RoleId) == ee.Id);
                    if (newselect != -1)
                    {
                        demRoles += $"<@&{roles[newselect].RoleId}> ";
                    }
                }
                RXEmbed.AddField("Your Roles:", demRoles);
                RXEmbed.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);
                await ctx.RespondAsync(embed: RXEmbed.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }//<@&467492990462459904>


        public class RootObject
        {
            [JsonProperty("Name")]
            public string Name { get; set; }
            [JsonProperty("ChannelId")]
            public object ChannelID { get; set; }
            [JsonProperty("RoleId")]
            public object RoleId { get; set; }
        }
    }
}
