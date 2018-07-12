﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace someBot
{
    class VoWiki
    {
        [Command("vwiki"), Description("Search Vocaloidwiki"), Aliases("vocaloidwiki")]
        public async Task vocawiki(CommandContext ctx, [RemainingText] string term)
        {//http://vocaloid.wikia.com/api/v1/Search/List?query=miku&limit=10&minArticleQuality=10&batch=1&namespaces=0
            var interactivity = ctx.Client.GetInteractivityModule();
            var init = await ctx.RespondAsync("Searching");
            int select = 0;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://vocaloid.wikia.com/api/v1/Search/List?query=" + term + "&limit=10&minArticleQuality=10&batch=1&namespaces=0");
            request.Method = "GET";
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (!((int)response.StatusCode == 200))
            {
                await ctx.RespondAsync("Song not found");
                return;
            }
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            //await ctx.RespondAsync("data got");
            var myresponse = JsonConvert.DeserializeObject<VWikiSearch>(responseFromServer);
            response.Close();
            //await ctx.RespondAsync("data: " + myresponse.items[0].artistString);
            if (myresponse.items.Count == 0)
            {
                await init.ModifyAsync("nothing found uwu");
                return;
            }
            if (myresponse.items.Count > 1)
            {
                var embed2 = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#0d2b35"),
                    Title = "VocaloidWiki Search",
                    Description = "Multiple entries were found, select one!"
                };
                int an = 0;
                string[] nums = { ":one:", ":two:", ":three:", ":four:", ":five:", ":six:", ":seven:", ":eight:", ":nine:", ":keycap_ten:" };
                string songs = "";
                var blank = DiscordEmoji.FromGuildEmote(ctx.Client, 435447550196318218);
                foreach (var entries in myresponse.items)
                {
                    songs += $"React {DiscordEmoji.FromName(ctx.Client, nums[an])} for {entries.title} \n";
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

                var reSelect = await interactivity.WaitForReactionAsync(xe => (xe == one || xe == two || xe == three || xe == four || xe == five || xe == six || xe == seven || xe == eight || xe == nine || xe == ten), ctx.Message.Author, TimeSpan.FromSeconds(60));

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
                var oof = ctx.Channel.GetMessageAsync(ctx.Message.Id).Result.GetReactionsAsync(DiscordEmoji.FromUnicode("🇻")).Result;
                await init.DeleteAllReactionsAsync();
            }

            HttpWebRequest Arequest = (HttpWebRequest)WebRequest.Create("http://vocaloid.wikia.com/api/v1/Articles/AsSimpleJson?id=" + myresponse.items[select].id);
            Arequest.Method = "GET";
            Arequest.Accept = "application/json";
            HttpWebResponse Aresponse = (HttpWebResponse)Arequest.GetResponse();
            if (!((int)Aresponse.StatusCode == 200))
            {
                await ctx.RespondAsync("oof");
                return;
            }
            Stream AdataStream = Aresponse.GetResponseStream();
            StreamReader Areader = new StreamReader(AdataStream);
            string AresponseFromServer = Areader.ReadToEnd();
            //await ctx.RespondAsync("data got");
            var Amyresponse = QuickType.WikiHandle.FromJson(AresponseFromServer);
            response.Close();

            int pselect = 0;

            try
            {
                string thumb = Amyresponse.Sections[7].Images[0].First().ToString();
                thumb = thumb.Remove(thumb.Length - 1);
                thumb = thumb.Substring(5);
                await ctx.RespondAsync("thumb " + thumb);
            }
            catch
            {
                await ctx.RespondAsync("told ya bitch lmao");
            }
            var emim = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#0d2b35"),
                Title = Amyresponse.Sections[pselect].Title,
                Description = "Entry for " + Amyresponse.Sections[0].Title
            };

            foreach (var Layer1 in Amyresponse.Sections[pselect].Content)
            {
                try
                {
                    emim.AddField("Stuff", Layer1.Text);
                }
                catch
                {
                    emim.AddField("this section was too long uwu", "tell me which loid plz");
                }
            }

            emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);
            var end = emim.Build();
            await init.ModifyAsync(null, embed: end);
            //await ctx.RespondAsync(Amyresponse.Sections[8].Content[0].Elements[0].Elements[0].Elements[0].Text);
        }

        public class SItem
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("title")]
            public string title { get; set; }
            [JsonProperty("url")]
            public string url { get; set; }
        }

        public class VWikiSearch
        {
            [JsonProperty("total")]
            public int total { get; set; }
            [JsonProperty("items")]
            public List<SItem> items { get; set; }
        }
    }
}
