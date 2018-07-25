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
    class Help
    {
        
        [Command("help")] //pretty selfexpnanatory
        public async Task NewHelp(CommandContext ctx)
        {
            try
            {
                var HelpEmbed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "MeekPlush Help",
                    Description = "(not so) useful commads that may help you!",
                    ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                };

                HelpEmbed.AddField("-[Random Commads]-", "``m!emotes`` displays all server emotes (yust typing ``emotes`` will do the same)" +
                    "``m!baguette`` *yeet*\n" +
                    "``m!test <message>`` the bot will send that message\n" +
                    "``m!knowledge`` gives you a random Wikipedia page\n" +
                    "``m!wiki <searchterm>`` search for a wikipedia page!\n" +
                    "``m!touhouwiki <searchterm>`` Touhouwiki search!\n" +
                    "``m!invite`` bot invite link");
                HelpEmbed.AddField("-[YouTube Commands]-", "``m!yts <searchterm>`` will search a YouTube video\n" +
                    "``m!ytsc <searchterm>`` will search a YouTube channel\n" +
                    "``m!ytsp <searchterm>`` will search a YouTube playlist\n" +
                    "``m!yt`` same as the 3 above but with a convenient menu!\n");
                HelpEmbed.AddField("-[Picture Commands]-", "``m!cat`` random Cat Pic!\n" +
                    "``m!dog`` random Dog Pic\n" +
                    "``m!catgirl`` random Catgirl Pic!\n" +
                    "``m!foxgirl`` random Foxgirl Pic!\n" +
                    "``m!kanna`` random Kanna image!\n" +
                    "``m!diva`` random Project Diva Image (from the loading screen images'n stuff, not ingame footage lmao))");
                HelpEmbed.AddField("-[Song Info Commands]-", "``m!vocadb <songname>`` search for a Vocaloid song! (+lyrics if available)\n" +
                    "``m!utaitedb <songname>`` search UtaiteDB!(+lyrics if available)\n" +
                    "``m!touhoudb <songname>`` search for a Touhou song! (+lyrics if available)");
                HelpEmbed.AddField("-[NSFW uwu]-", "``m!nl <category name>`` will display a random image from nekos.life get the category names at (11) [nekos.life](https://nekos.life/api/v2/endpoints)\n" +
                    "``m!thigh`` random thigh pic uwu\n" +
                    "``m!nekopara`` random lewd nekopara image/gif uwu");
                HelpEmbed.AddField("Like this?", "Please upvote [here](https://discordbots.org/bot/465675368775417856/vote), helps me very much uwu \n" +
                    "Github: [Link](https://github.com/Speyd3r/MeekPlush-Bot)");
                var yeet = await ctx.RespondAsync(embed: HelpEmbed.Build());
                await ctx.Channel.GetMessageAsync(yeet.Id).Result.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":x:"));
                try
                {
                    await yeet.DeleteAllReactionsAsync();
                }
                catch
                {
                    await yeet.ModifyAsync("**Heya! I'm missing the 'Manage Messages' permission! I need that for some commands uwu (expecially the more advanced ones)**");
                }
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
                Color = new DiscordColor("#289b9a"),
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
    }
}
