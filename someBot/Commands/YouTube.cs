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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace someBot
{
    class YouTube : BaseCommandModule
    {
        [Command("yts"), Description("Search a YouTube Video!")]
        public async Task YtSearchV(CommandContext ctx, [Description("Searchterm"), RemainingText]string term)
        {
                try //got this from the youtube .net examples, works lol
                {
                    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                    {
                        ApiKey = "AIzaSyDbj184qjOOS8fE6PlHcuyasA8VB_gr_f0", //i dont care about my api key lol, but if u make ur ever public, change it plz
                        ApplicationName = this.GetType().ToString()
                    });

                    var searchListRequest = youtubeService.Search.List("snippet"); //its always snippet
                    searchListRequest.Q = term; // Replace with your search term.
                    searchListRequest.MaxResults = 1;
                    searchListRequest.Type = "video"; //needs to be lowercase!

                    // Call the search.list method to retrieve results matching the specified query term.
                    var searchListResponse = await searchListRequest.ExecuteAsync();

                    await ctx.RespondAsync("https://www.youtube.com/watch?v=" + searchListResponse.Items[0].Id.VideoId); //gets the first searchresults video id and add ot to the link yeet
                }
                catch (AggregateException ex)
                {
                    foreach (var e in ex.InnerExceptions)
                    {
                        await ctx.RespondAsync("Error: " + e.Message);
                    }
                }
        }
        [Command("ytsc"), Description("Search a YouTube Channel!")] //same for this and the next one but with channels and playlists
        public async Task YtSearchC(CommandContext ctx, [Description("Searchterm"), RemainingText]string term)
        {
            if (ctx.Message.Content.EndsWith("!yt channel") || ctx.Message.Content.EndsWith("!yt channel ") || ctx.Message.Content.EndsWith("!yt c") || ctx.Message.Content.EndsWith("!yt c "))
            {
                await ctx.RespondAsync("plz enter search term e.g. ```!yt c channelname```");
            }
            else
            {
                try
                {
                    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                    {
                        ApiKey = "AIzaSyDbj184qjOOS8fE6PlHcuyasA8VB_gr_f0",
                        ApplicationName = this.GetType().ToString()
                    });

                    var searchListRequest = youtubeService.Search.List("snippet");
                    searchListRequest.Q = term; // Replace with your search term.
                    searchListRequest.MaxResults = 1;
                    searchListRequest.Type = "channel";

                    // Call the search.list method to retrieve results matching the specified query term.
                    var searchListResponse = await searchListRequest.ExecuteAsync();

                    await ctx.RespondAsync("https://www.youtube.com/channel/" + searchListResponse.Items[0].Id.ChannelId);
                }
                catch (AggregateException ex)
                {
                    foreach (var e in ex.InnerExceptions)
                    {
                        await ctx.RespondAsync("Error: " + e.Message);
                    }
                }
            }
        }

        [Command("ytsp"), Description("Search a YouTube Playlist!")]
        public async Task YtSearchP(CommandContext ctx, [Description("Searchterm"), RemainingText]string term)
        {
            if (ctx.Message.Content.EndsWith("!yt list") || ctx.Message.Content.EndsWith("!yt list ") || ctx.Message.Content.EndsWith("!yt p") || ctx.Message.Content.EndsWith("!yt p ") || ctx.Message.Content.EndsWith("!yt l") || ctx.Message.Content.EndsWith("!yt l "))
            {
                await ctx.RespondAsync("plz enter search term e.g. ```!yt l playlistname```");
            }
            else
            {
                try
                {
                    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                    {
                        ApiKey = "AIzaSyDbj184qjOOS8fE6PlHcuyasA8VB_gr_f0",
                        ApplicationName = this.GetType().ToString()
                    });

                    var searchListRequest = youtubeService.Search.List("snippet");
                    searchListRequest.Q = term; // Replace with your search term.
                    searchListRequest.MaxResults = 1;
                    searchListRequest.Type = "playlist";

                    // Call the search.list method to retrieve results matching the specified query term.
                    var searchListResponse = await searchListRequest.ExecuteAsync();

                    await ctx.RespondAsync("https://www.youtube.com/playlist?list=" + searchListResponse.Items[0].Id.PlaylistId);
                }
                catch (AggregateException ex)
                {
                    foreach (var e in ex.InnerExceptions)
                    {
                        await ctx.RespondAsync("Error: " + e.Message);
                    }
                }
            }
        }

        [Command("yt"),Description("Same yts n stuff but with a menu lol")] //similar to !nnd but its linear
        public async Task AllOne(CommandContext ctx)
        {
            try
            {
                var interactivity = ctx.Client.GetInteractivity();

                string searchTerm = "";
                string searchType = "";
                ulong TermId = 0;

                var embed2 = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#68D3D2"),
                    Title = "YouTube Search",
                    ThumbnailUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/98/YouTube_Logo.svg/2000px-YouTube_Logo.svg.png",
                    Description = "Search for a YouTube video, channel or playlist!"
                };

                embed2.AddField("Videos", "React with " + DiscordEmoji.FromUnicode("🇻") + " to search a video");
                embed2.AddField("Channels", "React with" + DiscordEmoji.FromUnicode("🇨") + " to search a channel");
                embed2.AddField("Playlists", "React with " + DiscordEmoji.FromUnicode("🇵") + " to search a playlist");
                embed2.WithFooter("requested by " + ctx.Message.Author.Username);

                //await ctx.RespondAsync("dis happends");
                var msg = await ctx.RespondAsync(embed: embed2.Build());


                await ctx.Guild.GetChannel(ctx.Channel.Id).GetMessageAsync(msg.Id).Result.CreateReactionAsync(DiscordEmoji.FromUnicode("🇻"));
                await ctx.Guild.GetChannel(ctx.Channel.Id).GetMessageAsync(msg.Id).Result.CreateReactionAsync(DiscordEmoji.FromUnicode("🇨"));
                await ctx.Guild.GetChannel(ctx.Channel.Id).GetMessageAsync(msg.Id).Result.CreateReactionAsync(DiscordEmoji.FromUnicode("🇵"));
                // send the paginator
                try
                {
                    var pickHappend = await interactivity.WaitForMessageReactionAsync(xe => (xe == DiscordEmoji.FromUnicode("🇻") || xe == DiscordEmoji.FromUnicode("🇨") || xe == DiscordEmoji.FromUnicode("🇵")), msg, ctx.Message.Author, TimeSpan.FromSeconds(60));
                    if (pickHappend.Emoji == DiscordEmoji.FromUnicode("🇻"))
                    {
                        searchType = "video";
                    }
                    else if (pickHappend.Emoji == DiscordEmoji.FromUnicode("🇨"))
                    {
                        searchType = "channel";
                    }
                    else if (pickHappend.Emoji == DiscordEmoji.FromUnicode("🇵"))
                    {
                        searchType = "playlist"; ;
                    }
                }
                catch
                {
                    return;
                }
                //await ctx.Guild.GetChannel(ctx.Message.Channel.Id).GetMessageAsync(ctx.Message.Id).Result.DeleteAsync();
                await ctx.Guild.GetChannel(msg.Channel.Id).GetMessageAsync(msg.Id).Result.DeleteAllReactionsAsync();

                if (searchType != "")
                {
                    var pickEmbed = new DiscordEmbedBuilder
                    {
                        Color = new DiscordColor("#68D3D2"),
                        Title = "YouTube Search",
                        ThumbnailUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/98/YouTube_Logo.svg/2000px-YouTube_Logo.svg.png"
                    };
                    pickEmbed.AddField("You picked: " + searchType, "Please enter your search query now!");
                    await msg.ModifyAsync(embed: pickEmbed.Build());
                    var termHappend = await interactivity.WaitForMessageAsync(xm =>
                    {
                        if (xm.Author.Id == ctx.Message.Author.Id)
                        {
                            if (!(xm.Content.Length == 0))
                            {
                                searchTerm = xm.Content;
                                TermId = xm.Id;
                            }
                            else return false;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }, TimeSpan.FromSeconds(60));

                    if (TermId != 0)
                    {
                        await ctx.Guild.GetChannel(ctx.Message.Channel.Id).GetMessageAsync(TermId).Result.DeleteAsync();
                    }
                    //await ctx.Guild.GetChannel(termHappend.Message.Channel.Id).GetMessageAsync(termHappend.Message.Id).Result.DeleteAsync();
                }

                //YouTube search stuff
                if (searchType.Length > 0 && searchTerm.Length > 0)
                {
                    try
                    {
                        var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                        {
                            ApiKey = "AIzaSyDbj184qjOOS8fE6PlHcuyasA8VB_gr_f0",
                            ApplicationName = "C# Discord Bot"
                        });
                        var searchListRequest = youtubeService.Search.List("snippet");
                        searchListRequest.Q = searchTerm; // Replace with your search term.
                        searchListRequest.MaxResults = 1;
                        searchListRequest.Type = searchType;

                        // Call the search.list method to retrieve results matching the specified query term.
                        var searchListResponse = await searchListRequest.ExecuteAsync();

                        if (searchType == "playlist")
                        {
                            await msg.ModifyAsync("https://www.youtube.com/playlist?list=" + searchListResponse.Items[0].Id.PlaylistId, embed: null);
                        }
                        if (searchType == "channel")
                        {
                            await msg.ModifyAsync("https://www.youtube.com/channel/" + searchListResponse.Items[0].Id.ChannelId, embed: null);
                        }
                        if (searchType == "video")
                        {
                            await msg.ModifyAsync("https://www.youtube.com/watch?v=" + searchListResponse.Items[0].Id.VideoId, embed: null);
                        }

                    }
                    catch (AggregateException ex)
                    {
                        foreach (var e in ex.InnerExceptions)
                        {
                            await ctx.RespondAsync("Error: " + e.Message);
                        }
                    }
                }
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("alexa"), Description("Search a YouTube Video!")] //same as !yts but since there is a space theres "play" string, does not actially contain alexa api stuff
        public async Task YtSearchAlexa(CommandContext ctx, string play, [Description("Searchterm"), RemainingText]string term)
        {
            if (play == "play")
            {
                try
                {
                    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                    {
                        ApiKey = "AIzaSyDbj184qjOOS8fE6PlHcuyasA8VB_gr_f0",
                        ApplicationName = this.GetType().ToString()
                    });

                    var searchListRequest = youtubeService.Search.List("snippet");
                    searchListRequest.Q = term; // Replace with your search term.
                    searchListRequest.MaxResults = 1;
                    searchListRequest.Type = "video";

                    // Call the search.list method to retrieve results matching the specified query term.
                    var searchListResponse = await searchListRequest.ExecuteAsync();

                    await ctx.RespondAsync("https://www.youtube.com/watch?v=" + searchListResponse.Items[0].Id.VideoId);
                }
                catch (AggregateException ex)
                {
                    foreach (var e in ex.InnerExceptions)
                    {
                        await ctx.RespondAsync("Error: " + e.Message);
                    }
                }
            }
            else
            {
                return;
            }
        }

        [Command("nnd")]
        public async Task NND(CommandContext ctx, string listPick = null)
        {
            int offset = 0;
            if (listPick.ToLower() == "hourly") offset = 0;
            else if (listPick.ToLower() == "daily") offset = 20;
            else if (listPick.ToLower() == "weekly") offset = 40;
            else if (listPick.ToLower() == "monthly") offset = 60;
            var inter = ctx.Client.GetInteractivity();
            var init = await ctx.RespondAsync("this may take a while, be patient uwu");
            IWebDriver driver;
            var chromeDriverService = FirefoxDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true; //selenium shows a commandprompt, this hides it
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            FirefoxOptions options = new FirefoxOptions();
            options.AddArguments("--headless"); //to tell firefox to start in windowless mode
            driver = new FirefoxDriver(chromeDriverService, options); //starts up firefox it in the directory already (called gecko)
            driver.Navigate().GoToUrl("http://ex.nicovideo.jp/vocaloid/ranking"); //ig goes to the ranking pages
            string[] allranks = new string[80]; // there are always 80 songs on that page! this is for the names
            string[] allURLS = new string[80]; //it also gets the urls
            int yeet = 0; //to add stuff to the array
            DiscordColor meek = new DiscordColor("#68D3D2");
            foreach (var vids in driver.FindElements(By.ClassName("ttl"))) //lit ay? all items in the ranking have the classname ttl
            {
                allranks[yeet] = vids.Text; //it gest the inner text which is the Name
                allURLS[yeet] = driver.FindElement(By.LinkText(vids.Text)).GetAttribute("href"); //and the link from the "a" tag thats arond the Songname
                yeet++;
            }
            var empty = ctx.Client.GetGuildAsync(401419401011920907).Result.GetEmojiAsync(435447550196318218).Result; //i have an emoji on my server thats just blank, i use it for spaces in shit here
            List<Page> pgs = new List<Page>();
            DiscordEmbedBuilder bassc = new DiscordEmbedBuilder
            {
                Color = meek,
                Title = "NicoNicoDouga " + listPick + " Vocaloid Ranking",
                Description = "Top 20 " + listPick + " songs!\n" + empty,
                ThumbnailUrl = "https://japansaucedotnet.files.wordpress.com/2016/05/photo-22.gif"
            };
            //DiscordEmbedBuilder[] hourly = new DiscordEmbedBuilder[4];
            int adv = 0;
            int adv2 = 5;
            for (int j = 0; j < 4; j++)
            {
                if (j == 1)
                {
                    adv = 5;
                    adv2 = 10;
                }
                if (j == 2)
                {
                    adv = 10;
                    adv2 = 15;
                }
                if (j == 3)
                {
                    adv = 15;
                    adv2 = 20;
                }
                Console.WriteLine("whut");
                DiscordEmbedBuilder hourly = new DiscordEmbedBuilder
                {
                    Color = meek,
                    Title = "NicoNicoDouga " + listPick + " Vocaloid Ranking",
                    Description = "Top 20 " + listPick + " songs!\n" + empty,
                    ThumbnailUrl = "https://japansaucedotnet.files.wordpress.com/2016/05/photo-22.gif"
                };
                for (int i = (offset + adv); i < (offset + adv2); i++) //gets the first 5
                {
                    if (i < (offset + (adv2 - 1)))
                    {
                        hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i]);
                    }
                    else
                    {
                        hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i] + "\n" + empty);
                    }
                }
                hourly.AddField("Page", $"{(j + 1)}/4");
                pgs.Add(new Page { Embed = hourly.Build() });
            }
            driver.Quit();
            //hourly.AddField("Page", pages + "/4\nEnter Page number or type ``exit`` to exit");
            await init.DeleteAsync();
            await inter.SendPaginatedMessage(ctx.Channel, ctx.Message.Author, pgs, timeoutoverride: TimeSpan.FromMinutes(2));
        }
    }
}
