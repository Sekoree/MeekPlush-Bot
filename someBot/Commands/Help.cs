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
                    if (!(ctx.Message.Channel.Type.ToString() == "Private")) //DM Messages
                    {
                        if (ctx.Channel.GuildId == 373635826703400960)
                        {
                            HelpEmbed.AddField("Xeddd Specific Commands", "**m!roles <rolename>**\nPlease refer to <#467001692429484032> for further instructions and available Roles!");
                        }
                        if (ctx.Channel.GuildId == 469661736534802432)
                        {
                            HelpEmbed.AddField("Rin Specific Commands", "**m!roles <rolename>**\nTo get one or more vocaloid roles! to see all available roles, just type ``m!role``");
                        }
                    }
                    HelpEmbed.AddField("-[Random Commands]-> **m!help random**", "stuff i cant catogorize lol");
                    HelpEmbed.AddField("-[Search Commands]-> **m!help search**", "search some sites ");
                    HelpEmbed.AddField("-[Songinfo Commands]-> **m!help info**", "weebmusic info uwu");
                    HelpEmbed.AddField("-[Image Commands]-> **m!help image**", "random pics from the web");
                    HelpEmbed.AddField("-[Music Commands]-> **m!help music**", "music commands");
                    HelpEmbed.AddField("-[Support Info]-> **m!help support**", "support server n stuff");
                    HelpEmbed.AddField("-[NSFW Commands]-> **m!help nsfw**", "only work in NSFW channels!");
                }
                else if (page.ToLower() == "random")
                {
                    HelpEmbed.AddField("m!emotes", "displays all server emotes", true);
                    HelpEmbed.AddField("m!info", "some bot info", true);
                    HelpEmbed.AddField("m!baguette", "yeet", true);
                    HelpEmbed.AddField("m!test <message>", "the bot will send that message", true);
                    HelpEmbed.AddField("m!knowledge", "gives you a random Wikipedia page", true);
                }
                else if (page.ToLower() == "search")
                {
                    HelpEmbed.AddField("m!wiki <searchterm>", "search for a wikipedia page!", true);
                    HelpEmbed.AddField("m!touhouwiki <searchterm", "Touhouwiki search!", true);
                    HelpEmbed.AddField("m!yts <searchterm>", "will search a YouTube video", true);
                    HelpEmbed.AddField("m!ytsc <searchterm>", "will search a YouTube channel", true);
                    HelpEmbed.AddField("m!ytsp <searchterm>", "will search a YouTube playlist", true);
                    HelpEmbed.AddField("m!yt", "same as the 3 above but with a `convenient´ menu!", true);
                }
                else if (page.ToLower() == "info")
                {
                    HelpEmbed.AddField("m!vocadb <songname>", "search for a Vocaloid song! (+lyrics if available)", true);
                    HelpEmbed.AddField("m!utaitedb <songname>", "search UtaiteDB!(+lyrics if available)", true);
                    HelpEmbed.AddField("m!touhoudb <songname>", "search for a Touhou song! (+lyrics if available)", true);
                    HelpEmbed.AddField("m!nnd <hourly/daily/weekly/monthly>", "shows the corresponding Niconico Vocaloid Ranking", true);
                }
                else if (page.ToLower() == "images" || page.ToLower() == "image")
                {
                    HelpEmbed.AddField("m!cat" ,"random Cat Pic!", true);
                    HelpEmbed.AddField("m!dog" ,"random Dog Pic", true);
                    HelpEmbed.AddField("m!catgirl" ,"random Catgirl Pic!", true);
                    HelpEmbed.AddField("m!foxgirl", "random Foxgirl Pic!", true);
                    HelpEmbed.AddField("m!kanna", "random Kanna image!", true);
                    HelpEmbed.AddField("m!diva", "random Project Diva Image (from the loading screens, not ingame footage lmao)", true);
                    HelpEmbed.AddField("m!rin", "random Rin image!", true);
                    HelpEmbed.AddField("m!una", "random Una image!", true);
                    HelpEmbed.AddField("m!gumi", "random Gumi image", true);
                    HelpEmbed.AddField("m!luka", "random Luka image!", true);
                    HelpEmbed.AddField("m!ia", "random IA image", true);
                    HelpEmbed.AddField("m!yukari", "random Yukari image", true);
                    HelpEmbed.AddField("m!meiko", "random Meiko image", true);
                    HelpEmbed.AddField("m!teto", "random Teto image");
                    HelpEmbed.AddField("m!len", "random Len image", true);
                    HelpEmbed.AddField("m!kaito", "random Kaito image", true);
                    HelpEmbed.AddField("m!fukase", "random Fukase image", true);
                    HelpEmbed.AddField("m!miku", "random Miku image", true);
                    HelpEmbed.AddField("m!miki", "random Miki image", true);
                    HelpEmbed.AddField("m!mayu", "random Mayu image", true);
                    HelpEmbed.AddField("m!aoki", "random Aoki image", true);
                    HelpEmbed.AddField("m!lily", "random Lily image", true);
                }
                else if (page.ToLower() == "music")
                {
                    HelpEmbed.AddField("m!join", "joins your voicechannel!", true);
                    HelpEmbed.AddField("m!leave", "will leave the voicechannel (queue will be saved)\n", true);
                    HelpEmbed.AddField("m!queue", "shows the current queue!\n" +
                        "Aliases: ``m!q``", true);
                    HelpEmbed.AddField("m!play <searchTermOrURL>", "joins your voicechannel and plays something (using this again will add stuff to the queue)!\n" +
                        "Aliases: ``m!p``\n" +
                        "Additionally: when using m!join and then loading a playlist, just use m!p or m!play to start playback!", true);
                    HelpEmbed.AddField("m!skip", "skips the current song!", true);
                    HelpEmbed.AddField("m!stop", "bot will stop and disconnect from the channel!", true);
                    HelpEmbed.AddField("m!repeatall", "will loop the queue!\n" +
                        "Aliases: ``m!ra``", true);
                    HelpEmbed.AddField("m!repeat", "will loop the current song!\n" +
                        "Aliases: ``m!r``", true);
                    HelpEmbed.AddField("m!shuffle", "will play the queue in shuffle mode, in combination with ``m!repeat`` it will play the entire queue loopd and shuffled!\n" +
                        "Aliases: ``m!s``", true);
                    HelpEmbed.AddField("m!queueremove <queueNumber>", "will remove that particular song from the queue!\n" +
                        "Aliases: ``m!qr``", true);
                    HelpEmbed.AddField("m!nowplaying", "shows whats currently playing with some additional info!\n" +
                        "Aliases: ``m!np``", true);
                    HelpEmbed.AddField("m!queueclear", "removes everything from the queue!\n" +
                        "Aliases: ``m!qc", true);
                    HelpEmbed.AddField("m!pause", "will pause the current song!", true);
                    HelpEmbed.AddField("m!resume", "will resume the paused song!", true);
                    HelpEmbed.AddField("m!queueremovesome <start> <(optional)end>", "will remove every song between those 2 numbers in queue, if just 1 is provided everything after that number will be deleted from queue\n" +
                        "Aliases: ``m!qrs``", true);
                    HelpEmbed.AddField("m!volume <number>", "changes the volume (150 is max)!\n" +
                        "Aliases: ``m!vol``", true);
                    HelpEmbed.AddField("m!playlist <youtubePlaylistURL>", "adds a playlist to the queue (max 600 videos)!\n" +
                        "Aliases: ``m!pp", true);
                }
                else if (page.ToLower() == "support")
                {
                    HelpEmbed.AddField("Support Server", "[Invite](https://discord.gg/mrMwkZ8)", true);
                    HelpEmbed.AddField("Contact", "If u dont wanna join the support server, u can also just DM me (Speyd3r#3939)", true);
                }
                else if (page.ToLower() == "nsfw")
                {
                    HelpEmbed.AddField("m!nl <category name>" ,"will display a random image from nekos.life, get the category names at (11) [nekos.life](https://nekos.life/api/v2/endpoints)", true);
                    HelpEmbed.AddField("m!thigh", "random thigh pic uwu", true);
                    HelpEmbed.AddField("m!nekopara", "random lewd nekopara image/gif uwu", true);
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
            await ctx.RespondAsync("https://discord.gg/mrMwkZ8");
        }
    }
}
