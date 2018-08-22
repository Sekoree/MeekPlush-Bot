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
        [Command("role"), Aliases("roles"), Description("get a role to access a vocaloid channel")] //yeet
        public async Task VocRole(CommandContext ctx, string role = null)
        {
            try
            {
                string ServerJSON = "";
                string Desc = "";
                string Title = "";
                StreamReader r;
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
                        r = new StreamReader("RinGroups.json");
                        string json2 = r.ReadToEnd();
                        var roles2 = JsonConvert.DeserializeObject<List<RootObject>>(json2);
                        string avRoles = "";
                        foreach(var ghd in roles2)
                        {
                            avRoles += $"'``{ghd.Name}``' ";
                        }
                        await ctx.RespondAsync("**__Available Roles:__**\n" +
                            avRoles + "\n" +
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
                r = new StreamReader(ServerJSON);
                string json = r.ReadToEnd();
                var roles = JsonConvert.DeserializeObject<List<RootObject>>(json);
                int select = roles.FindIndex(x => x.Name == role);
                DiscordEmbedBuilder RXEmbed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#68D3D2"),
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

        [Command("crole"), RequireOwner]
        public async Task creAtRole(CommandContext ctx, int pos, string color, string name)
        {
            try
            {
                var test = await ctx.Guild.CreateRoleAsync(name: name,permissions: Permissions.SendMessages, color: new DiscordColor($"#{color}"), hoist: true);
                await test.ModifyPositionAsync(position: pos);
            }
            catch (Exception ex)
            {
                await ctx.RespondAsync($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        [Command("crolee"), RequireOwner]
        public async Task ModRole(CommandContext ctx, ulong id, int pos, string name)
        {
            try
            {
                await ctx.Guild.GetRole(id).ModifyAsync(name: name);
                await ctx.Guild.GetRole(id).ModifyPositionAsync(position: pos);
            }
            catch (Exception ex)
            {
                await ctx.RespondAsync($"{ex.Message}\n{ex.StackTrace}");
            }
        }

        [Command("crolepos"), RequireOwner]
        public async Task ModRolepos(CommandContext ctx, ulong id)
        {
            try
            {
                await ctx.RespondAsync("Position: " + ctx.Guild.GetRole(id).Position);
            }
            catch (Exception ex)
            {
                await ctx.RespondAsync($"{ex.Message}\n{ex.StackTrace}");
            }
        }

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
