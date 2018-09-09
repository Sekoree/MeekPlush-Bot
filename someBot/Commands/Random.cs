using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Interactivity;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Sagitta.Clients;
using Sagitta;

namespace someBot
{
    class Random : BaseCommandModule
    {
        //!!!you can define groups with [Group("groupName")] BUT its added to the command so if id call this group random, it'd be !random emotes

        [Command("emotes"),Description("lists all customs server emotes")] //same as if u just type emotes, 
        public async Task Emotes(CommandContext ctx)
        {
            var pixivClient = new PixivClient("CLIENT_ID", "CLIENT_SECRET");
            var imgs = await pixivClient.Search.IllustAsync("kagamine", Sagitta.Enum.SearchTarget.PartialMatchForTags, Sagitta.Enum.SortOrder.PopularDesc);
            var img = imgs.Illusts.First().ImageUrls.Original;
            string emotelist = "";
            foreach (var emotes in ctx.Guild.Emojis)
            {
                emotelist += emotes;
            }
            await ctx.RespondAsync(emotelist);
        }

        [Command("baguette"),Description("yeet")] //yeet
        public async Task Baguette(CommandContext ctx)
        {
            await ctx.RespondAsync(":french_bread::french_bread::french_bread::french_bread::french_bread:");
        }

        [Command("llmanco"), Description("yeet"), RequireOwner] //yeet
        public async Task LLman(CommandContext ctx)
        {
            var oo = ctx.Client.GetLavalink();
            Bot.guit[0].LLinkCon = await oo.ConnectAsync(Bot.lcfg);
        }

        [Command("getrid"), RequireOwner] //yeet
        public async Task getRid(CommandContext ctx, [RemainingText] string rol)
        {
            var darol = ctx.Guild.Roles.Where(x => x.Name.ToLower().StartsWith(rol.ToLower()));
            await ctx.RespondAsync(darol.First().Name + " " + darol.First().Id);
        }

        [Command("addprefix"), Description("change prefix uwu"), RequirePermissions(DSharpPlus.Permissions.ManageGuild)] //yeet
        public async Task PFixChange(CommandContext ctx, string hell)
        {
            if (hell == null || hell == "")
            {
                return;
            }
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            Bot.guit[pos].prefix.Add(hell);
            await ctx.RespondAsync($"prefix {hell} added!");
        }

        [Command("removeprefix"), Description("change prefix uwu"), RequirePermissions(DSharpPlus.Permissions.ManageGuild)] //yeet
        public async Task PFixremoce(CommandContext ctx, int remID)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            if (Bot.guit[pos].prefix.Count == 1)
            {
                await ctx.RespondAsync($"This Guild has only 1 Prefix (``{Bot.guit[pos].prefix[0]}``) cannot remove last one");
                return;
            }
            await ctx.RespondAsync($"prefix {Bot.guit[pos].prefix[remID]} was deleted");
            Bot.guit[pos].prefix.RemoveAt(remID);
        }

        [Command("prefix"), Description("get this guilds prefixes"), Aliases("prefixes")] //yeet
        public async Task PFix(CommandContext ctx)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            if (pos == -1)
            {
                await ctx.RespondAsync($"This guild has the default ``m!`` prefix!");
                return;
            }
            //string owo = "";
            DiscordEmbedBuilder bassc = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#68D3D2"),
                Title = $"Prefixes of {ctx.Guild.Name}",
                Description = "A list of all the prefixes this Guild has! and their ID (in case you want to remove one)",
                ThumbnailUrl = ctx.Member.AvatarUrl
            };
            string owo = "";
            int i = 0;
            foreach(var oo in Bot.guit[pos].prefix)
            {
                owo += $"{i} ``{oo}``\n";
                i++;
            }
            bassc.AddField("Usable Prefixes", owo);
            bassc.AddField("How To Add Profixes:", $"Use ``{Bot.guit[pos].prefix[0]}addprefix <prefixname>`` to add a prefix!\nExample: ``{Bot.guit[pos].prefix[0]}addprefix test`` wil add the prefix ``test``\nso commands can start with ``test`` like ``testrin`` would show you a Rin-chan picture like ``{Bot.guit[pos].prefix[0]}rin`` would");
            bassc.AddField("How To Remove A Prefix:", $"Use ``{Bot.guit[pos].prefix[0]}removeprefix <ID>`` to remove one (note removing all doesnt work)\nExample: {Bot.guit[pos].prefix[0]}removeprefix 0 will remove the first prefix");
            bassc.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);
            await ctx.RespondAsync(embed: bassc.Build());
        }

        [Command("addopfix"), Description("change prefix uwu"), RequireOwner] //yeet
        public async Task PFixChangeOwner(CommandContext ctx, string hell)
        {
            if (hell == null || hell == "")
            {
                return;
            }
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            Bot.guit[pos].prefix.Add(hell);
            await ctx.RespondAsync($"prefix {hell} added");
        }

        [Command("removeopfix"), Description("change prefix uwu"), RequireOwner] //yeet
        public async Task PFixremovOwner(CommandContext ctx, int remID)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            if (Bot.guit[pos].prefix.Count == 1)
            {
                await ctx.RespondAsync($"This Guild has only 1 Prefix (``{Bot.guit[pos].prefix[0]}``) cannot remove last one");
                return;
            }
            await ctx.RespondAsync($"prefix {Bot.guit[pos].prefix[remID]} was deleted");
            Bot.guit[pos].prefix.RemoveAt(remID);
        }

        [Command("test"),Description("this should be called different but nah")] //i think u same some people use this, pretty simple
        public async Task Test(CommandContext ctx, [RemainingText]string oof)
        {
            await ctx.RespondAsync(oof);
        }

        [Command("rolesids"), Description("list roles")]
        public async Task roles(CommandContext ctx)
        {
            foreach(var oof in ctx.Guild.Roles)
            {
                await ctx.Member.SendMessageAsync($"``{oof.Name} {oof.Id}``");
                await Task.Delay(1000);
            }
        }

        [Command("guilds"), Description("list guilds"), RequireOwner]
        public async Task Guilds(CommandContext ctx)
        {
            foreach (var oof in ctx.Client.Guilds)
            {
                await ctx.Member.SendMessageAsync($"``{oof.Value.Name} {oof.Value.Owner} {oof.Value.MemberCount} {oof.Value.IconUrl}``");
            }
        }

        [Command("dj-links"), Aliases("music-links")] //i use sinusbot so, yea, long test is long, this was before i discovered Embeds
        public async Task DjLinks(CommandContext ctx)
        {
                await ctx.RespondAsync("**Webinterface**: https://srgg.de:8012/ \n" +
                    "**Youtube Search**: https://srgg.de:8012/scripts/youtube/ (Webinterface login required, if u have no personal account use 'dj' as username and password)\n" +
                    "**SyncWatch**: https://srgg.de:8012/scripts/syncwatch/ (for this Method 3 is required + webinterface account needs to be bound to your discord account (!music for details)) !yt <ytlink> to watch videos of the sent link together");
        }

        [Command("invite"),Aliases("link"),Description("Invitelink for the bot")] //guess....
        public async Task Invite(CommandContext ctx)
        {
            await ctx.RespondAsync("https://meek.moe/invite/");
        }
    }
}