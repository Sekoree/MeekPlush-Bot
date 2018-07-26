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
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Dynamic;

namespace someBot
{
    class RanPics
    {
        [Command("cat"), Description("Shows you a random cat picture")]
        public async Task CatPic(CommandContext ctx)
        {
            try
            {

                WebRequest request = WebRequest.Create("http://thecatapi.com/api/images/get?format=src");
                WebResponse response = request.GetResponse();

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Cat Picture/Gif!",
                    Description = "via thecatapi.com",
                    ImageUrl = response.ResponseUri.ToString()
                };

                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());

            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("dog"), Description("Shows you a random dog picture")]
        public async Task DogPic(CommandContext ctx)
        {
            try
            {

                WebRequest request = WebRequest.Create("https://api.thedogapi.com/v1/images/search");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                var myresponse = JsonConvert.DeserializeObject<List<ImgRet>>(responseFromServer);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Dog Picture/Gif!",
                    Description = "via thedogapi.com",
                    ImageUrl = myresponse[0].url
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("catgirl"), Description("Show you a Random catgirl image")]
        public async Task NekoPic(CommandContext ctx)
        {
            try
            {

                WebRequest request = WebRequest.Create("https://nekos.life/api/v2/img/neko");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Catgirl Pictures!",
                    Description = "via nekos.life",
                    ImageUrl = myresponse.url
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("foxgirl"), Description("Shows you a random holo image")]
        public async Task WolfGirlPic(CommandContext ctx)
        {
            try
            {

                WebRequest request = WebRequest.Create("https://nekos.life/api/v2/img/neko");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Foxgirl Pictures!",
                    Description = "via nekos.life",
                    ImageUrl = myresponse.url
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }//https://nekobot.xyz/api/image?type=thigh

        [Command("thigh"), Description("Shows you a random thigh image"), RequireNsfw]
        public async Task ThighPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://nekobot.xyz/api/image?type=thigh");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<NadekoRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "uwu",
                    Description = "via nadeko.xyz",
                    ImageUrl = myresponse.message
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("nekopara"), Description("Shows you a random nekopara image or gif"), RequireNsfw]
        public async Task NPPic(CommandContext ctx)
        {
            try
            {
                System.Random rnd = new System.Random();
                int yo = rnd.Next(0, 2);
                string url = "";
                if (yo == 0) url = "https://api.ohlookitsderpy.space/nekoparagif";
                else url = "https://api.ohlookitsderpy.space/nekoparastatic";
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "uwu",
                    Description = "via api.ohlookitsderpy.space",
                    ImageUrl = myresponse.url
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("diva"), Description("Shows you a Project Diva image")]
        public async Task DivaPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/diva");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Project Diva Image!",
                    Description = "via api.meek.moe",
                    ImageUrl = myresponse.url.ToString()
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("rin"), Description("Shows you a Project Diva image")]
        public async Task KRinPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/rin");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Rin Image!",
                    Description = "via api.meek.moe",
                    ImageUrl = myresponse.url.ToString()
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("una"), Description("Shows you a Project Diva image")]
        public async Task OUnaPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/una");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Una Image!",
                    Description = "via api.meek.moe",
                    ImageUrl = myresponse.url.ToString()
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("gumi"), Description("Shows you a Project Diva image")]
        public async Task GumiPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/gumi");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Gumi Image!",
                    Description = "via api.meek.moe",
                    ImageUrl = myresponse.url.ToString()
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("kanna"), Description("Shows you a Kanna image")]
        public async Task KannaPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://nekobot.xyz/api/image?type=kanna");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<NadekoRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "uwu",
                    Description = "via nadeko.xyz",
                    ImageUrl = myresponse.message
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        [Command("nl"), RequireNsfw, Description("get a random picture from nekos.life (category name required, look at point 11 of https://nekos.life/api/v2/endpoints) nneds NSFW Channel, due to this random pic database beim 2/3 porn lmao")]
        public async Task nekosLife(CommandContext ctx, string pick)
        {
            try
            {

                var interactivity = ctx.Client.GetInteractivityModule();
                WebRequest request = WebRequest.Create("https://nekos.life/api/v2/img/" + pick);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#6600cc"),
                    Title = "Random Pictures!",
                    Description = "via nekos.life",
                    ImageUrl = myresponse.url
                };
                response.Close();
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                var it = await ctx.RespondAsync(embed: emim.Build());
                bool woo = false;
                await ctx.Channel.GetMessageAsync(it.Id).Result.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":one:"));
                await Task.Delay(2500);
                try
                {
                    var lewd = await interactivity.WaitForMessageReactionAsync(xe => xe == DiscordEmoji.FromName(ctx.Client, ":one:"), it, ctx.Message.Author, TimeSpan.FromSeconds(60));
                    if (lewd.Emoji == DiscordEmoji.FromName(ctx.Client, ":one:"))
                    {
                        woo = true;
                    }
                    if (woo)
                    {
                        await ctx.Channel.GetMessageAsync(it.Id).Result.DeleteAsync();
                    }
                }
                catch
                {
                    return;
                }
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        public class ImgRet
        {
            [JsonProperty("url")]
            public string url { get; set; }
        }

        public class NadekoRet
        {
            [JsonProperty("message")]
            public string message { get; set; }
        }
    }
}
