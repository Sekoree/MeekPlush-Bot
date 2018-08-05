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
    class Derpy : BaseCommandModule
    {
        [Command("nekopara"), Description("Shows you a random nekopara image or gif uwu"), RequireNsfw]
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
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "uwu",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
                };
                response.Close();
                emim.WithAuthor(name: "via api.ohlookitsderpy.space", url: "https://api.ohlookitsderpy.space/");
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
