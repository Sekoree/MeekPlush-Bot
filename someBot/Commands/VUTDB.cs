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
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Dynamic;

namespace someBot
{
    class VUTDB : BaseCommandModule
    {
        [Command("vocadb"), Description("Search Vocaloidsongs (and their lyrics, if available) via vocadb.net")]
        public async Task VocaDBGet(CommandContext ctx, [RemainingText] string song)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var init = await ctx.RespondAsync("Searching");
            int select = 0;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://vocadb.net/api/songs?query=" + song + "&maxResults=10&sort=FavoritedTimes&preferAccurateMatches=true&nameMatchMode=Auto&fields=Lyrics,PVs&lang=English");
            request.Method = "GET";
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (!((int)response.StatusCode == 200))
            {
                await ctx.RespondAsync("Song not found (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
                return;
            }
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //await ctx.RespondAsync("data got");
            var myresponse = JsonConvert.DeserializeObject<VocaDBGetData>(responseFromServer);
            response.Close();
            //await ctx.RespondAsync("data: " + myresponse.items[0].artistString);
            if (myresponse.items.Count == 0)
            {
                await init.ModifyAsync("nothing found uwu (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
                return;
            }
            if (myresponse.items.Count > 1)
            {
                var embed2 = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "VocaDB Search",
                    Description = "Multiple entries were found, select one!"
                };
                int an = 0;
                string[] nums = { ":one:", ":two:", ":three:", ":four:", ":five:", ":six:", ":seven:", ":eight:", ":nine:", ":keycap_ten:" };
                string songs = "";
                var blank = DiscordEmoji.FromGuildEmote(ctx.Client, 435447550196318218);
                foreach (var entries in myresponse.items)
                {
                    songs += $"React {DiscordEmoji.FromName(ctx.Client, nums[an])} for {entries.name} by {entries.artistString} \n";
                    await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, nums[an]));
                    an++;
                }
                embed2.AddField("Found Songs", songs);
                await init.ModifyAsync("", embed: embed2.Build());

                var one = DiscordEmoji.FromName(ctx.Client, ":one:");
                var two = DiscordEmoji.FromName(ctx.Client, ":two:");
                var three = DiscordEmoji.FromName(ctx.Client, ":three:");
                var four = DiscordEmoji.FromName(ctx.Client, ":four:");
                var five = DiscordEmoji.FromName(ctx.Client, ":five:");
                var six = DiscordEmoji.FromName(ctx.Client, ":six:");
                var seven = DiscordEmoji.FromName(ctx.Client, ":seven:");
                var eight = DiscordEmoji.FromName(ctx.Client, ":eight:");
                var nine = DiscordEmoji.FromName(ctx.Client, ":nine:");
                var ten = DiscordEmoji.FromName(ctx.Client, ":keycap_ten:");
                try
                {
                    var reSelect = await interactivity.WaitForMessageReactionAsync(xe => (xe == one || xe == two || xe == three || xe == four || xe == five || xe == six || xe == seven || xe == eight || xe == nine || xe == ten), init, ctx.Message.Author, TimeSpan.FromSeconds(60));

                    if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":one:")) select = 0;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":two:")) select = 1;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":three:")) select = 2;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":four:")) select = 3;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":five:")) select = 4;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":six:")) select = 5;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":seven:")) select = 6;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":eight:")) select = 7;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":nine:")) select = 8;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":keycap_ten:")) select = 9;
                }
                catch
                {
                    return;
                }
                await init.DeleteAllReactionsAsync();
            }
            //https://vocadb.net/api/songs/4083/derived?fields=PVs&lang=English
            HttpWebRequest drequest = (HttpWebRequest)WebRequest.Create("https://vocadb.net/api/songs/" + myresponse.items[select].id + "/derived?lang=English");
            drequest.Method = "GET";
            drequest.Accept = "application/json";
            HttpWebResponse dresponse = (HttpWebResponse)drequest.GetResponse();
            if (!((int)dresponse.StatusCode == 200))
            {
                await ctx.RespondAsync("Song not found (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
                return;
            }
            Stream ddataStream = dresponse.GetResponseStream();
            StreamReader dreader = new StreamReader(ddataStream);
            string dresponseFromServer = dreader.ReadToEnd();
            dresponse.Close();
            //await ctx.RespondAsync("data got");

            var ddresponse = JsonConvert.DeserializeObject<List<VTUDBrGet>>(dresponseFromServer);
            int oof2 = myresponse.items[select].pvs.FindIndex(x => x.url.Contains("youtu"));
            int oof3 = myresponse.items[select].pvs.FindIndex(x => x.url.Contains("nico"));
            YTDLC got = new YTDLC();
            BaseiInfo(ctx, init, select, myresponse, ddresponse);
            await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pushpin:"));
            if (myresponse.items[select].lyrics.Count != 0) await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":page_facing_up:"));
            if (myresponse.items[select].pvs.Count != 0)
            {
                await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":movie_camera:"));
                if (myresponse.items[select].pvs.Any(x => (x.url.Contains("youtu") || x.url.Contains("nico")))) await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"));
            }
            if (ddresponse.Count != 0) await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:"));
            await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":x:"));

            bool loop = true;
            while (loop == true)
            {
                var emoji = DiscordEmoji.FromName(ctx.Client, ":pushpin:");
                var emoji2 = DiscordEmoji.FromName(ctx.Client, ":page_facing_up:");
                var emoji3 = DiscordEmoji.FromName(ctx.Client, ":movie_camera:");
                var emoji4 = DiscordEmoji.FromName(ctx.Client, ":x:");
                var emoji5 = DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:");//:arrow_double_down: 
                var emoji6 = DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:");
                try
                {
                    var reSelect = await interactivity.WaitForMessageReactionAsync(xe => (xe == emoji || xe == emoji2 || xe == emoji3 || xe == emoji4 || xe == emoji5 || xe == emoji6), init, ctx.Message.Author, TimeSpan.FromSeconds(123));

                    if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":pushpin:")) BaseiInfo(ctx, init, select, myresponse, ddresponse);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":page_facing_up:")) LyricEm(ctx, init, select, myresponse, ddresponse);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":movie_camera:")) PVShowInfo(ctx, init, select, myresponse, ddresponse, oof2);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:")) Derratives(ctx, init, select, myresponse, ddresponse);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"))
                    {
                        if (!(oof2 == -1 && oof3 == -1))
                        {
                            if (oof3 != -1)
                            {
                                await ctx.RespondAsync("Downloading from NicoNico, this may take a while! (NND is slow but has best quality)");
                                got.YTDL(ctx, myresponse.items[select].pvs[oof3].url);
                            }
                            else if (oof2 != -1)
                            {
                                await ctx.RespondAsync("Downloading from YouTube! Please wait!");
                                got.YTDL(ctx, myresponse.items[select].pvs[oof2].url);
                            }
                            await init.DeleteReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"), ctx.Message.Author);
                            await init.DeleteOwnReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"));
                        }
                        else
                        {
                            await ctx.RespondAsync("No valid Download Source found, sorry uwu");
                        }
                    }
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":x:"))
                    {
                        loop = false;
                        await init.DeleteAllReactionsAsync();
                    }
                    else
                    {
                        loop = false;
                        await init.DeleteAllReactionsAsync();
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        [Command("utaitedb"), Description("Search Japanese songs (and their lyrics, if available) via utaitedb.net")]
        public async Task UtaiteDBGet(CommandContext ctx, [RemainingText] string song)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var init = await ctx.RespondAsync("Searching");
            int select = 0;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://utaitedb.net/api/songs?query=" + song + "&maxResults=10&sort=FavoritedTimes&preferAccurateMatches=true&nameMatchMode=Auto&fields=Lyrics,PVs&lang=English");
            request.Method = "GET";
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (!((int)response.StatusCode == 200))
            {
                await ctx.RespondAsync("Song not found (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
                return;
            }
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //await ctx.RespondAsync("data got");
            var myresponse = JsonConvert.DeserializeObject<VocaDBGetData>(responseFromServer);
            response.Close();
            //await ctx.RespondAsync("data: " + myresponse.items[0].artistString);
            if (myresponse.items.Count == 0)
            {
                await init.ModifyAsync("nothing found uwu (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
                return;
            }
            if (myresponse.items.Count > 1)
            {
                var embed2 = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "UtaiteDB Search",
                    Description = "Multiple entries were found, select one!"
                };
                int an = 0;
                string[] nums = { ":one:", ":two:", ":three:", ":four:", ":five:", ":six:", ":seven:", ":eight:", ":nine:", ":keycap_ten:" };
                string songs = "";
                var blank = DiscordEmoji.FromGuildEmote(ctx.Client, 435447550196318218);
                foreach (var entries in myresponse.items)
                {
                    songs += $"React {DiscordEmoji.FromName(ctx.Client, nums[an])} for {entries.name} by {entries.artistString} \n";
                    await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, nums[an]));
                    an++;
                }
                embed2.AddField("Found Songs", songs);
                await init.ModifyAsync("", embed: embed2.Build());

                var one = DiscordEmoji.FromName(ctx.Client, ":one:");
                var two = DiscordEmoji.FromName(ctx.Client, ":two:");
                var three = DiscordEmoji.FromName(ctx.Client, ":three:");
                var four = DiscordEmoji.FromName(ctx.Client, ":four:");
                var five = DiscordEmoji.FromName(ctx.Client, ":five:");
                var six = DiscordEmoji.FromName(ctx.Client, ":six:");
                var seven = DiscordEmoji.FromName(ctx.Client, ":seven:");
                var eight = DiscordEmoji.FromName(ctx.Client, ":eight:");
                var nine = DiscordEmoji.FromName(ctx.Client, ":nine:");
                var ten = DiscordEmoji.FromName(ctx.Client, ":keycap_ten:");
                try
                {
                    var reSelect = await interactivity.WaitForMessageReactionAsync(xe => (xe == one || xe == two || xe == three || xe == four || xe == five || xe == six || xe == seven || xe == eight || xe == nine || xe == ten), init, ctx.Message.Author, TimeSpan.FromSeconds(60));
                    if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":one:")) select = 0;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":two:")) select = 1;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":three:")) select = 2;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":four:")) select = 3;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":five:")) select = 4;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":six:")) select = 5;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":seven:")) select = 6;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":eight:")) select = 7;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":nine:")) select = 8;
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":keycap_ten:")) select = 9;
                }
                catch
                {
                    return;
                }
                await init.DeleteAllReactionsAsync();
            }

            HttpWebRequest drequest = (HttpWebRequest)WebRequest.Create("https://utaitedb.net/api/songs/" + myresponse.items[select].id + "/derived?lang=English");
            drequest.Method = "GET";
            drequest.Accept = "application/json";
            HttpWebResponse dresponse = (HttpWebResponse)drequest.GetResponse();
            if (!((int)dresponse.StatusCode == 200))
            {
                await ctx.RespondAsync("Song not found (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
                return;
            }
            Stream ddataStream = dresponse.GetResponseStream();
            StreamReader dreader = new StreamReader(ddataStream);
            string dresponseFromServer = dreader.ReadToEnd();
            dresponse.Close();
            //await ctx.RespondAsync("data got");
            var ddresponse = JsonConvert.DeserializeObject<List<VTUDBrGet>>(dresponseFromServer);
            int oof2 = myresponse.items[select].pvs.FindIndex(x => x.url.Contains("youtu"));
            int oof3 = myresponse.items[select].pvs.FindIndex(x => x.url.Contains("nico"));
            YTDLC got = new YTDLC();
            BaseiInfo(ctx, init, select, myresponse, ddresponse);
            await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pushpin:"));
            if (myresponse.items[select].lyrics.Count != 0) await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":page_facing_up:"));
            if (myresponse.items[select].pvs.Count != 0)
            {
                await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":movie_camera:"));
                if (myresponse.items[select].pvs.Any(x => (x.url.Contains("youtu") || x.url.Contains("nico")))) await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"));
            }
            if (ddresponse.Count != 0) await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:"));
            await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":x:"));

            bool loop = true;
            while (loop == true)
            {
                var emoji = DiscordEmoji.FromName(ctx.Client, ":pushpin:");
                var emoji2 = DiscordEmoji.FromName(ctx.Client, ":page_facing_up:");
                var emoji3 = DiscordEmoji.FromName(ctx.Client, ":movie_camera:");
                var emoji4 = DiscordEmoji.FromName(ctx.Client, ":x:");
                var emoji5 = DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:");//:arrow_double_down: 
                var emoji6 = DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:");
                try
                {
                    var reSelect = await interactivity.WaitForMessageReactionAsync(xe => (xe == emoji || xe == emoji2 || xe == emoji3 || xe == emoji4 || xe == emoji5 || xe == emoji6), init, ctx.Message.Author, TimeSpan.FromSeconds(123));

                    if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":pushpin:")) BaseiInfo(ctx, init, select, myresponse, ddresponse);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":page_facing_up:")) LyricEm(ctx, init, select, myresponse, ddresponse);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":movie_camera:")) PVShowInfo(ctx, init, select, myresponse, ddresponse, oof2);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:")) Derratives(ctx, init, select, myresponse, ddresponse);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"))
                    {
                        if (!(oof2 == -1 && oof3 == -1))
                        {
                            if (oof3 != -1)
                            {
                                await ctx.RespondAsync("Downloading from NicoNico, this may take a while! (NND is slow but has best quality)");
                                got.YTDL(ctx, myresponse.items[select].pvs[oof3].url);
                            }
                            else if (oof2 != -1)
                            {
                                await ctx.RespondAsync("Downloading from YouTube! Please wait!");
                                got.YTDL(ctx, myresponse.items[select].pvs[oof2].url);
                            }
                            await init.DeleteReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"), ctx.Message.Author);
                            await init.DeleteOwnReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"));
                        }
                        else
                        {
                            await ctx.RespondAsync("No valid Download Source found, sorry uwu");
                        }
                    }
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":x:"))
                    {
                        loop = false;
                        await init.DeleteAllReactionsAsync();
                    }
                    else
                    {
                        loop = false;
                        await init.DeleteAllReactionsAsync();
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        [Command("touhoudb"), Aliases("tohodb"), Description("Search Touhou songs (and their lyrics, if available) via touhoudb.com")]
        public async Task TohoDBGet(CommandContext ctx, [RemainingText] string song)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var init = await ctx.RespondAsync("Searching");
            int select = 0;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://touhoudb.com/api/songs?query=" + song + "&maxResults=10&sort=FavoritedTimes&preferAccurateMatches=true&nameMatchMode=Auto&fields=Lyrics,PVs&lang=English");
            request.Method = "GET";
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (!((int)response.StatusCode == 200))
            {
                await ctx.RespondAsync("Song not found (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
                return;
            }
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //await ctx.RespondAsync("data got");
            var myresponse = JsonConvert.DeserializeObject<VocaDBGetData>(responseFromServer);
            response.Close();
            //await ctx.RespondAsync("data: " + myresponse.items[0].artistString);
            if (myresponse.items.Count == 0)
            {
                await init.ModifyAsync("nothing found uwu (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
                return;
            }
            if (myresponse.items.Count > 1)
            {
                var embed2 = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "TouhouDB Search",
                    Description = "Multiple entries were found, select one!"
                };
                int an = 0;
                string[] nums = { ":one:", ":two:", ":three:", ":four:", ":five:", ":six:", ":seven:", ":eight:", ":nine:", ":keycap_ten:" };
                string songs = "";
                var blank = DiscordEmoji.FromGuildEmote(ctx.Client, 435447550196318218);
                foreach (var entries in myresponse.items)
                {
                    songs += $"React {DiscordEmoji.FromName(ctx.Client, nums[an])} for {entries.name} by {entries.artistString} \n";
                    await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, nums[an]));
                    an++;
                }
                embed2.AddField("Found Songs", songs);
                await init.ModifyAsync("", embed: embed2.Build());

                var one = DiscordEmoji.FromName(ctx.Client, ":one:");
                var two = DiscordEmoji.FromName(ctx.Client, ":two:");
                var three = DiscordEmoji.FromName(ctx.Client, ":three:");
                var four = DiscordEmoji.FromName(ctx.Client, ":four:");
                var five = DiscordEmoji.FromName(ctx.Client, ":five:");
                var six = DiscordEmoji.FromName(ctx.Client, ":six:");
                var seven = DiscordEmoji.FromName(ctx.Client, ":seven:");
                var eight = DiscordEmoji.FromName(ctx.Client, ":eight:");
                var nine = DiscordEmoji.FromName(ctx.Client, ":nine:");
                var ten = DiscordEmoji.FromName(ctx.Client, ":keycap_ten:");
                try { 
                var reSelect = await interactivity.WaitForMessageReactionAsync(xe => (xe == one || xe == two || xe == three || xe == four || xe == five || xe == six || xe == seven || xe == eight || xe == nine || xe == ten), init, ctx.Message.Author, TimeSpan.FromSeconds(60));

                if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":one:")) select = 0;
                else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":two:")) select = 1;
                else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":three:")) select = 2;
                else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":four:")) select = 3;
                else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":five:")) select = 4;
                else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":six:")) select = 5;
                else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":seven:")) select = 6;
                else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":eight:")) select = 7;
                else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":nine:")) select = 8;
                else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":keycap_ten:")) select = 9;
                }
                catch
                {
                    return;
                }
                await init.DeleteAllReactionsAsync();
            }
            HttpWebRequest drequest = (HttpWebRequest)WebRequest.Create("https://touhoudb.com/api/songs/" + myresponse.items[select].id + "/derived?lang=English");
            drequest.Method = "GET";
            drequest.Accept = "application/json";
            HttpWebResponse dresponse = (HttpWebResponse)drequest.GetResponse();
            if (!((int)dresponse.StatusCode == 200))
            {
                await ctx.RespondAsync("Song not found (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
                return;
            }
            Stream ddataStream = dresponse.GetResponseStream();
            StreamReader dreader = new StreamReader(ddataStream);
            string dresponseFromServer = dreader.ReadToEnd();
            dresponse.Close();
            //await ctx.RespondAsync("data got");
            //var myresponse = JsonConvert.DeserializeObject<VocaDBGetData>(responseFromServer);
            var ddresponse = JsonConvert.DeserializeObject<List<VTUDBrGet>>(dresponseFromServer);
            int oof2 = myresponse.items[select].pvs.FindIndex(x => x.url.Contains("youtu"));
            int oof3 = myresponse.items[select].pvs.FindIndex(x => x.url.Contains("nico"));
            YTDLC got = new YTDLC();
            BaseiInfo(ctx, init, select, myresponse, ddresponse);
            await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pushpin:"));
            if (myresponse.items[select].lyrics.Count != 0) await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":page_facing_up:"));
            if (myresponse.items[select].pvs.Count != 0)
            {
                await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":movie_camera:"));
                if (myresponse.items[select].pvs.Any(x => (x.url.Contains("youtu") || x.url.Contains("nico")))) await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"));
            }
            if (ddresponse.Count != 0) await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:"));
            await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":x:"));

            bool loop = true;
            while (loop == true)
            {
                var emoji = DiscordEmoji.FromName(ctx.Client, ":pushpin:");
                var emoji2 = DiscordEmoji.FromName(ctx.Client, ":page_facing_up:");
                var emoji3 = DiscordEmoji.FromName(ctx.Client, ":movie_camera:");
                var emoji4 = DiscordEmoji.FromName(ctx.Client, ":x:");
                var emoji5 = DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:");//:arrow_double_down: 
                var emoji6 = DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:");
                try
                {
                    var reSelect = await interactivity.WaitForMessageReactionAsync(xe => (xe == emoji || xe == emoji2 || xe == emoji3 || xe == emoji4 || xe == emoji5 || xe == emoji6), init, ctx.Message.Author, TimeSpan.FromSeconds(123));

                    if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":pushpin:")) BaseiInfo(ctx, init, select, myresponse, ddresponse);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":page_facing_up:")) LyricEm(ctx, init, select, myresponse, ddresponse);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":movie_camera:")) PVShowInfo(ctx, init, select, myresponse, ddresponse, oof2);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:")) Derratives(ctx, init, select, myresponse, ddresponse);
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"))
                    {
                        if (!(oof2 == -1 && oof3 == -1))
                        {
                            if (oof3 != -1)
                            {
                                await ctx.RespondAsync("Downloading from NicoNico, this may take a while! (NND is slow but has best quality)");
                                got.YTDL(ctx, myresponse.items[select].pvs[oof3].url);
                            }
                            else if (oof2 != -1)
                            {
                                await ctx.RespondAsync("Downloading from YouTube! Please wait!");
                                got.YTDL(ctx, myresponse.items[select].pvs[oof2].url);
                            }
                            await init.DeleteReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"), ctx.Message.Author);
                            await init.DeleteOwnReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"));
                        }
                        else
                        {
                            await ctx.RespondAsync("No valid Download Source found, sorry uwu");
                        }
                    }
                    else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":x:"))
                    {
                        loop = false;
                        await init.DeleteAllReactionsAsync();
                    }
                    else
                    {
                        loop = false;
                        await init.DeleteAllReactionsAsync();
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        public async void BaseiInfo(CommandContext ctx, DiscordMessage init, int select, dynamic myresponse, dynamic dresponse)
        {
            try
            {
                try { await init.DeleteReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pushpin:"), ctx.Message.Author); }
                catch { }
                string tumurl = "";
                if (myresponse.items[select].pvs.Count != 0) tumurl = myresponse.items[select].pvs[0].thumbUrl;
                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Results!",
                    Description = "Entry for " + myresponse.term,
                    ThumbnailUrl = tumurl
                };

                emim.AddField("Artist", myresponse.items[select].artistString, true);
                emim.AddField("Title", myresponse.items[select].defaultName, true);
                emim.AddField("Language", myresponse.items[select].defaultNameLanguage, true);
                emim.AddField("Published", myresponse.items[select].publishDate.ToShortDateString(), true);
                emim.AddField("Song Type", myresponse.items[select].songType, true);
                //emim.AddField("Lyrics", lys);
                string pvs = "";
                foreach (var pv in myresponse.items[select].pvs)
                {
                    pvs += pv.url + "\n";
                }
                emim.AddField("PV(s)", pvs, true);
                emim.AddField("VocaDB Link", $"https://vocadb.net/S/{myresponse.items[select].id}", true);
                emim.AddField("Cover/Remix count",dresponse.Count.ToString(), true);
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                //emim.AddField("Buttons", $"{DiscordEmoji.FromName(ctx.Client, ":pushpin: ")} this\n" +
                //    $"{DiscordEmoji.FromName(ctx.Client, ":page_facing_up:")} (if available) Lyrics!\n" +
                //    $"{DiscordEmoji.FromName(ctx.Client, ":movie_camera:")} Watch the PV!\n" +
                //    $"{DiscordEmoji.FromName(ctx.Client, ":x:")} cancel out, not needed, but its there");
                var end = emim.Build();
                await init.ModifyAsync(null, embed: end);
            }
            catch
            {
                await ctx.RespondAsync("Song not found (Note: In weird cases it finds the song but still returns 0 results, since this crashes the bot, its not showing anything here)");
            }
        }

        public async void PVShowInfo(CommandContext ctx, DiscordMessage init, int select, dynamic myresponse, dynamic dresponse, int yt)
        {
            try
            {
                try {await init.DeleteReactionAsync(DiscordEmoji.FromName(ctx.Client, ":movie_camera:"), ctx.Message.Author); }
                catch { }
                if (yt == -1) yt = 0;
                await init.ModifyAsync(myresponse.items[select].pvs[yt].url, embed: null);
            }
            catch
                {
                    await ctx.RespondAsync("If you see this, idk how this can even break, it just changes the message to a link, no fancy embed making involved anyway, tell Speyd3r#3939");
                }
        }

        public async void LyricEm(CommandContext ctx, DiscordMessage init, int select, dynamic myresponse, dynamic dresponse)
        {
            try
            {
                try { await init.DeleteReactionAsync(DiscordEmoji.FromName(ctx.Client, ":page_facing_up:"), ctx.Message.Author); }
                catch { }
                var interactivity = ctx.Client.GetInteractivity();
                int lyse = 0;
                string tumurl = "";
                if (myresponse.items[select].pvs.Count != 0) tumurl = myresponse.items[select].pvs[0].thumbUrl;
                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Results!",
                    ThumbnailUrl = tumurl
                };

                emim.AddField("Artist", myresponse.items[select].artistString, true);
                emim.AddField("Title", myresponse.items[select].defaultName, true);
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);
                if (myresponse.items[select].lyrics.Count > 1)
                {
                    await init.DeleteAllReactionsAsync();
                    var embed2 = new DiscordEmbedBuilder
                    {
                        Color = new DiscordColor("#289b9a"),
                        Title = "Lyrics Entries!",
                        Description = "Multiple entries were found, select one!"
                    };
                    int an = 0;
                    string[] nums = { ":one:", ":two:", ":three:", ":four:", ":five:", ":six:", ":seven:", ":eight:", ":nine:", ":keycap_ten:" };
                    var blank = DiscordEmoji.FromGuildEmote(ctx.Client, 435447550196318218);
                    foreach (var entries in myresponse.items[select].lyrics)
                    {
                        if (an == 10) break;
                        embed2.AddField("React " + DiscordEmoji.FromName(ctx.Client, nums[an]), $"[{entries.translationType}] + {entries.cultureCode.ToUpper()} by {entries.source}");
                        await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, nums[an]));
                        an++;
                    }
                    await init.ModifyAsync(null, embed: embed2.Build());

                    var one = DiscordEmoji.FromName(ctx.Client, ":one:");
                    var two = DiscordEmoji.FromName(ctx.Client, ":two:");
                    var three = DiscordEmoji.FromName(ctx.Client, ":three:");
                    var four = DiscordEmoji.FromName(ctx.Client, ":four:");
                    var five = DiscordEmoji.FromName(ctx.Client, ":five:");
                    var six = DiscordEmoji.FromName(ctx.Client, ":six:");
                    var seven = DiscordEmoji.FromName(ctx.Client, ":seven:");
                    var eight = DiscordEmoji.FromName(ctx.Client, ":eight:");
                    var nine = DiscordEmoji.FromName(ctx.Client, ":nine:");
                    var ten = DiscordEmoji.FromName(ctx.Client, ":keycap_ten:");
                    try
                    {
                        var reSelect = await interactivity.WaitForMessageReactionAsync(xe => (xe == one || xe == two || xe == three || xe == four || xe == five || xe == six || xe == seven || xe == eight || xe == nine || xe == ten), init, ctx.Message.Author, TimeSpan.FromSeconds(30));
                        if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":one:")) lyse = 0;
                        else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":two:")) lyse = 1;
                        else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":three:")) lyse = 2;
                        else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":four:")) lyse = 3;
                        else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":five:")) lyse = 4;
                        else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":six:")) lyse = 5;
                        else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":seven:")) lyse = 6;
                        else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":eight:")) lyse = 7;
                        else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":nine:")) lyse = 8;
                        else if (reSelect.Emoji == DiscordEmoji.FromName(ctx.Client, ":keycap_ten:")) lyse = 9;
                    }
                    catch
                    {
                        return;
                    }
                    await init.DeleteAllReactionsAsync();
                    await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":pushpin:"));
                    if (myresponse.items[select].lyrics.Count != 0)
                    {
                        await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":page_facing_up:"));
                    }
                    if (myresponse.items[select].pvs.Count != 0)
                    {
                        await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":movie_camera:"));
                        await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":arrow_double_down:"));
                    }
                    if (dresponse.Count != 0)
                    {
                        await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:"));
                    }
                    await init.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":x:"));
                }
                if (myresponse.items[select].lyrics[lyse].translationType.Length != 0) emim.AddField("Language", myresponse.items[select].lyrics[lyse].translationType, true);
                if (myresponse.items[select].lyrics[lyse].source.Length != 0) emim.AddField("Source", myresponse.items[select].lyrics[lyse].source, true);
                if (myresponse.items[select].lyrics[lyse].translationType.Length != 0) emim.AddField("Type", myresponse.items[select].lyrics[lyse].translationType, true);
                if (myresponse.items[select].lyrics[lyse].url.Length != 0) emim.AddField("Link", myresponse.items[select].lyrics[lyse].url, true);
                else
                {
                    emim.AddField("Link", $"https://vocadb.net/S/{myresponse.items[select].id}", true);
                }
                if (myresponse.items[select].lyrics[lyse].value.Length > 2000)
                {
                    emim.WithDescription("Lyrics too long, look at the link uwu");
                }
                else
                {
                    emim.WithDescription(myresponse.items[select].lyrics[lyse].value);
                }

                var end = emim.Build();

                await init.ModifyAsync(null, embed: end);
            }
            catch
            {
                await ctx.RespondAsync("Error lmao report this to Speyd3r#3939 tell him shit in the lyrics thing broke");
            }
        }

        public async void Derratives(CommandContext ctx, DiscordMessage init, int select, dynamic myresponse, dynamic dresponse)
        {
            try
            {
                try { await init.DeleteReactionAsync(DiscordEmoji.FromName(ctx.Client, ":revolving_hearts:"), ctx.Message.Author); }
                catch { }
                string tumurl = "";
                if (myresponse.items[select].pvs.Count != 0) tumurl = myresponse.items[select].pvs[0].thumbUrl;
                //await ctx.RespondAsync("b4 build");
                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Covers & Remixes!",
                    ThumbnailUrl = tumurl
                };
                string stuff = "";
                //await ctx.RespondAsync("b4 loop");
                foreach (var derr in dresponse)
                {
                    //await ctx.RespondAsync($"**{derr.name} by {derr.artistString}\n " +
                    //    $"https://vocadb.net/S/{derr.id} \n\n");
                    stuff += $"**{derr.name} by {derr.artistString}**\n https://vocadb.net/S/" + derr.id +" \n\n";
                }
                //await ctx.RespondAsync("af loop");
                if (stuff.Length > 1950)
                {
                    stuff = "too many please go to https://vocadb.net/S/" + myresponse.items[select].id;
                }
                emim.WithDescription("Found: \n\n" + stuff);
                //await ctx.RespondAsync("damn field");
                
                var end = emim.Build();
                await init.ModifyAsync(null, embed: end);
            }
            catch
            {
                await ctx.RespondAsync("Error lmao report this to Speyd3r#3939 tell him shit in the derratives broke");
            }
        }

        public class VTUDBrGet
        {
            [JsonProperty("artistString")]
            public string artistString { get; set; }
            [JsonProperty("defaultName")]
            public string defaultName { get; set; }
            [JsonProperty("defaultNameLanguage")]
            public string defaultNameLanguage { get; set; }
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("songType")]
            public string songType { get; set; }
        }


        public class VocaDBGetData
        {
            [JsonProperty("items")]
            public List<Item> items { get; set; }
            [JsonProperty("term")]
            public string term { get; set; }
            [JsonProperty("totalCount")]
            public int totalCount { get; set; }
        }

        public class Lyric
        {
            [JsonProperty("cultureCode")]
            public string cultureCode { get; set; }
            [JsonProperty("source")]
            public string source { get; set; }
            [JsonProperty("translationType")]
            public string translationType { get; set; }
            [JsonProperty("url")]
            public string url { get; set; }
            [JsonProperty("value")]
            public string value { get; set; }
        }

        public class Pv
        {
            [JsonProperty("author")]
            public string author { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("publishdate")]
            public DateTime publishDate { get; set; }
            [JsonProperty("pvType")]
            public string pvType { get; set; }
            [JsonProperty("thumbUrl")]
            public string thumbUrl { get; set; }
            [JsonProperty("url")]
            public string url { get; set; }
        }

        public class Item
        {
            [JsonProperty("artistString")]
            public string artistString { get; set; }
            [JsonProperty("defaultName")]
            public string defaultName { get; set; }
            [JsonProperty("defaultNameLanguage")]
            public string defaultNameLanguage { get; set; }
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("lyrics")]
            public List<Lyric> lyrics { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("publishDate")]
            public DateTime publishDate { get; set; }
            [JsonProperty("pvs")]
            public List<Pv> pvs { get; set; }
            [JsonProperty("songType")]
            public string songType { get; set; }
        }
    }
}
