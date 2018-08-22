using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace someBot.Commands.RanImg
{
    class Other : BaseCommandModule
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
                    Color = new DiscordColor("#68D3D2"),
                    Title = "Random Cat Picture/Gif!",
                    Description = $"[Full Source Image Link]({response.ResponseUri.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={response.ResponseUri.ToString()}&resize=500"
                };

                response.Close();
                emim.WithAuthor(name: "via thecatapi.com", url: "https://thecatapi.com/");
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
                    Color = new DiscordColor("#68D3D2"),
                    Title = "Random Dog Picture/Gif!",
                    Description = $"[Full Source Image Link]({myresponse[0].url})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse[0].url}&resize=500"
                };
                response.Close();
                emim.WithAuthor(name: "via thedogapi.com", url: "https://thedogapi.com/");
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
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
    }
}
