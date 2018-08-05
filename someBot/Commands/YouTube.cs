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
                    Color = new DiscordColor("#289b9a"),
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
                        Color = new DiscordColor("#289b9a"),
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
    }
}
