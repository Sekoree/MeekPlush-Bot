using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace someBot.Commands.RanImg
{
    class NekosLife : BaseCommandModule
    {
        [Command("neko"), Description("Show you a Random catgirl image")]
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
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#68D3D2"),
                    Title = "Random Catgirl Pictures!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
                };
                response.Close();
                emim.WithAuthor(name: "via nekos.life", url: "https://nekos.life/");
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
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#68D3D2"),
                    Title = "Random Foxgirl Pictures!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
                };
                response.Close();
                emim.WithAuthor(name: "via nekos.life", url: "https://nekos.life/");
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
                WebRequest request = WebRequest.Create("https://nekos.life/api/v2/img/" + pick);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#6600cc"),
                    Title = "Random Pictures!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
                };
                response.Close();
                emim.WithAuthor(name: "via nekos.life", url: "https://nekos.life/");
                emim.WithFooter("Requested by " + ctx.Message.Author.Username, ctx.Message.Author.AvatarUrl);

                await ctx.RespondAsync(embed: emim.Build());
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }
    }
}
