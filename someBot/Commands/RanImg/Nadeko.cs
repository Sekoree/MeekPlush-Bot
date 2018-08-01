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
    class Nadeko
    {
        [Command("thigh"), Description("Shows you a random thigh image uwu"), RequireNsfw]
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
                    Description = $"[Full Source Image Link]({myresponse.message.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.message.ToString()}&resize=500"
                };
                response.Close();
                emim.WithAuthor(name: "via api.meek.moe", url: "https://api.meek.moe/");
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
                    Description = $"[Full Source Image Link]({myresponse.message.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.message.ToString()}&resize=500"
                };
                response.Close();
                emim.WithAuthor(name: "via nekobot.xyz", url: "https://nekobot.xyz/");
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }

        public class NadekoRet
        {
            [JsonProperty("message")]
            public string message { get; set; }
        }
    }
}
