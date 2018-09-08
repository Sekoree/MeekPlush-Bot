using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;

namespace someBot
{
    class Help : BaseCommandModule
    {
        
        [Command("help")] //pretty selfexpnanatory
        public async Task NewHelp(CommandContext ctx, string page = null)
        {
            try
            {
                var HelpEmbed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#68D3D2"),
                    Title = "MeekPlush Help",
                    Description = "(not so) useful commads that may help you!\n **Support me on Patreon!** [link](https://www.patreon.com/speyd3r)",
                    ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                };
                if (page == null)
                {
                    if (ctx.Channel?.Guild?.Id == 373635826703400960)
                    {
                        HelpEmbed.AddField("Xeddd Specific Commands", "**m!roles <rolename>**\nPlease refer to <#467001692429484032> for further instructions and available Roles!");
                    }
                    if (ctx.Channel?.Guild?.Id == 469661736534802432)
                    {
                        HelpEmbed.AddField("Rin Specific Commands", "**m!roles <rolename>**\nTo get one or more vocaloid roles! to see all available roles, just type ``m!role``");
                    }
                    HelpEmbed.AddField("-[Random Commands]-> **m!help random**", "stuff i cant categorize lol");
                    HelpEmbed.AddField("-[Search Commands]-> **m!help search**", "search some sites ");
                    HelpEmbed.AddField("-[Songinfo Commands]-> **m!help info**", "weebmusic info uwu");
                    HelpEmbed.AddField("-[Image Commands]-> **m!help image**", "random pics from the web");
                    HelpEmbed.AddField("-[Music Commands]-> **m!help music**", "music commands");
                    HelpEmbed.AddField("-[Other Stuff]-> **m!help other**", "support server and feedback command");
                    HelpEmbed.AddField("-[NSFW Commands]-> **m!help nsfw**", "only work in NSFW channels!");
                }
                else if (page.ToLower() == "random")
                {
                    HelpEmbed.AddField("Random Commands:", "" +
                        "**m!emotes** --Display all server emotes\n" +
                        "**m!info** --Some bot info\n" +
                        "**m!baguette** --yeet\n" +
                        "**m!test <message>** --The bot will say that\n" +
                        "**m!knowledge** --Random Wikipedia artikle!");
                }
                else if (page.ToLower() == "search")
                {
                    HelpEmbed.AddField("Search stuff!", "" +
                        "**m!wiki <text>** --Search Wikipedia\n" +
                        "**m!touhouwiki <text>** --Search Touhouwiki\n--Aliases: **m!tohowiki <text>**\n" +
                        "**m!yts <text>** --Search a YouTube video\n" +
                        "**m!ytsc <text>** --Search a YouTube channel\n" +
                        "**m!ytsp <text>** --Search a Youtube playlist\n" +
                        "**m!yt** --Can do the same as the 3 above but with a guided menu");
                }
                else if (page.ToLower() == "info")
                {
                    HelpEmbed.AddField("Song if stuff uwu", "" +
                        "**m!vocadb <songname>** --Search VocaDB for a song!\n" +
                        "**m!utaitedb <songname>** --Search UtaiteDB for a song!\n" +
                        "**m!touhoudb <songname>** --Search TouhouDB for a song!\n--Aliases: **m!tohodb <songname>**\n" +
                        "**m!nnd <hourly|daily|weekly|montly>** --Look up the current Vocaloid song ranking from NND!");
                }
                else if (page.ToLower() == "images" || page.ToLower() == "image")
                {
                    HelpEmbed.AddField("'Normal' images","" +
                        "**m!cat** --Random cat picture!\n" +
                        "**m!dog** --Random dog Picture!\n" +
                        "**m!neko** --Random neko image uwu\n" +
                        "**m!kanna** --Random Kanna image uwu");
                    HelpEmbed.AddField("Vocaloid (and UTAU) images!", "" +
                        "**m!diva** --Radom Project Diva loadingscreen image!\n" +
                        "**m!rin** --Random Rin image!\n" +
                        "**m!una** --Random Una image!\n" +
                        "**m!gumi** --Random Gumi image!\n" +
                        "**m!luka** --Random Luka image!\n" +
                        "**m!ia** --Random IA image!\n" +
                        "**m!yukari** --Random Yukari image!\n" +
                        "**m!meiko** --Random Meiko image!\n" +
                        "**m!teto** --Random Teto image!\n" +
                        "**m!len** --Random Len image!\n" +
                        "**m!kaito** --Random Kaito image!\n" +
                        "**m!fukase** --Random Fukase image!\n" +
                        "**m!miku** --Random Miku image!\n" +
                        "**m!miki** --Random Miki image!");
                    HelpEmbed.AddField("more (cause ↑ hit the 1024 character limit)", "" +
                        "**m!mayu** --Random Mayu image!\n" +
                        "**m!aoki** --Random Aoki image!\n" +
                        "**m!lily** --Random Lily image!");
                }
                else if (page.ToLower() == "music")
                {
                    HelpEmbed.AddField("Music commands!", "" +
                        "**m%join** - Joins the Voice Channel you are in\n" +
                        "**m%leave** - Leaves the Voice Channel (queue will be saved)\n" +
                        "**m%play (URL or search term²)** - Play a song! If there are still songs in queue just using **m%play** will resume that -- Alias: m%p (URL or search term)\n" +
                        "**m%stop** - stops playback, to resume use **m%play**\n" +
                        "**m%skip** - skips the current song\n" +
                        "**m%repeat** - repeat the current song -- Alias: m%r\n" +
                        "**m%repeatall** - repats the entire queue -- Alias: m%ra\n" +
                        "**m%shuffle** - play the queue in shuffle mode, yes this works with the repeat command -- Alias: m%s");
                    HelpEmbed.AddField("more (cause ↑ hit the 1024 character limit)", "" +
                        "**m%queue** - shows you the current queue -- Alias: m%q\n" +
                        "**m%queueclear** - clear the queue -- Alias: m%qc\n" +
                        "**m%queueremove (number)** - remove that entry from the queue -- Alias: m%qr (number)\n" +
                        "**m%playlist** - load a playlist into queue -- Alias: m%pp\n" +
                        "**m%volume (volume)** - Change the music volume (Max 150) -- Alias: m%vol (number)\n" +
                        "**m%pause** - Pause the music!\n" +
                        "**m%resume** - Resume the music");
                }
                else if (page.ToLower() == "support" || page.ToLower() == "other")
                {
                    HelpEmbed.AddField("Support stuff uwu", "" +
                        "**m!feedback <message>** Send feedback!\n\n" +
                        "**Support server:** [Invite](https://discord.gg/YPPA2Pu)\n\n" +
                        "**Contact:** If you dont wanna join the support server, you can also just DM me (Speyd3r#3939)");
                }
                else if (page.ToLower() == "nsfw")
                {
                    HelpEmbed.AddField("Lewd stuff uwu", "" +
                        "**m!nl <categoryName>** --Will display a random image from nekos.life, get the category names at (11) [nekos.life](https://nekos.life/api/v2/endpoints)\n" +
                        "**m!thigh** --Random thigh pic uwu\n" +
                        "m!nekopara** --Random lewd nekopara image/gif uwu");
                }
                else
                {
                    return;
                }
                HelpEmbed.AddField("Like this?", "" +
                    "Please upvote [here](https://discordbots.org/bot/465675368775417856/vote), helps me very much uwu \n" +
                    "Github: [Link](https://github.com/Speyd3r/MeekPlush-Bot) \n" +
                    "Support me and keep the bot alive UwU: [Paypal](https://www.paypal.me/speyd3r) or [Patreon](https://www.patreon.com/speyd3r)");
                string check = "";
                int ir = ctx.Channel.Guild.CurrentMember.Roles.ToList().FindIndex(x => x.CheckPermission(DSharpPlus.Permissions.Administrator) == DSharpPlus.PermissionLevel.Allowed);
                int ir2 = ctx.Channel.Guild.CurrentMember.Roles.ToList().FindIndex(x => x.CheckPermission(DSharpPlus.Permissions.ManageMessages) == DSharpPlus.PermissionLevel.Allowed);
                if (ir == -1 && ir2 == -1) {
                    check = "**Heya! I'm missing the 'Manage Messages' permission! I need that for some commands uwu (expecially the more advanced ones)**";
                }
                var yeet = await ctx.RespondAsync(content: check, embed: HelpEmbed.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }
        [Command("help-dev")]
        public async Task DevHelp(CommandContext ctx)
        {
            try { 
            var HelpEmbed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#68D3D2"),
                Title = "MeekPlush Help",
                Description = "indev commands, dont expect them to work!",
                ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
            };
            HelpEmbed.AddField("-[Dev Commads]-", "``m!vocawiki`` super indev Vocaloid Wiki search\n" +
                "``m!vdl <link>`` download YouTube or NND videos as mp3! (does this violate some TOS? *Probably*\n");
            await ctx.RespondAsync(embed: HelpEmbed.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("info")]
        public async Task StatInfo(CommandContext ctx)
        {
            try
            {
                var HelpEmbed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#68D3D2"),
                    Title = "MeekPlush Stats",
                    Description = "stats n stuff",
                    ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                };
                HelpEmbed.AddField("Guilds", $"{ctx.Client.Guilds.Count.ToString()}");
                int men = 0;
                int memno = 0;
                foreach (var oi in ctx.Client.Guilds)
                {
                    men += oi.Value.MemberCount;
                    memno += oi.Value.Members.Where(x => x.IsBot == false).Count();
                }
                HelpEmbed.AddField("Users (without Bots)", $"{men} ({memno})");
                HelpEmbed.AddField("Ping", $"{ctx.Client.Ping.ToString()}");
                HelpEmbed.AddField("Bot Owner", $"Speyd3r#3939");
                HelpEmbed.AddField("Donations uwu", $"[Paypal](https://www.paypal.me/speyd3r)\n[Patreon](https://www.patreon.com/speyd3r)");
                await ctx.RespondAsync(embed: HelpEmbed.Build());
                //ctx.Guild.Members
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("support")] //i think u same some people use this, pretty simple
        public async Task supportServer(CommandContext ctx)
        {
            await ctx.RespondAsync("https://discord.gg/YPPA2Pu");
        }

        [Command("llinfo"), RequireOwner] //i think u same some people use this, pretty simple
        public async Task llinfo(CommandContext ctx)
        {
            var oo = Bot.guit[0].LLinkCon;
            await ctx.RespondAsync($"Players: {oo.Statistics.ActivePlayers}\n" +
                $"AllPlayers: {oo.Statistics.TotalPlayers}\n" +
                $"Uptime: {oo.Statistics.Uptime}");
        }

        [Command("feedback"), Description("send feedback uwu")] //yeet
        public async Task Feetforth(CommandContext ctx, [RemainingText] string message)
        {
            var em = new DiscordEmbedBuilder
            {
                Title = "Feedback!",
                Description = $"**User:** {ctx.Member.Username} ({ctx.Member.Mention})\n" +
                $"**Message:** {message}",
                Color = new DiscordColor("#68D3D2")
            };
            var send = ctx.Client.GetChannelAsync(484698696974336012).Result.SendMessageAsync(embed: em.Build());
            send.Wait();
            await ctx.RespondAsync("Feedback sent!");
        }
    }
}
