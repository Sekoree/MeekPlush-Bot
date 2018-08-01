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
    class MeekMoe
    {
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
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Project Diva Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("rin"), Description("Shows you a Rin image")]
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
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Rin Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("una"), Description("Shows you a Una image")]
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
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Una Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("gumi"), Description("Shows you a Gumi image")]
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
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.RespondAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Gumi Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("luka"), Description("Shows you a Luka image")]
        public async Task MLukaPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/luka");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.Client.CreateDmAsync(ctx.Client.GetUserAsync(174970757468651520).Result).Result.SendMessageAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Luka Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("ia"), Description("Shows you a IA image")]
        public async Task IAPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/ia");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //await ctx.RespondAsync(responseFromServer);
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                //await ctx.Client.CreateDmAsync(ctx.Client.GetUserAsync(174970757468651520).Result).Result.SendMessageAsync(myresponse.url);

                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random IA Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("yukari"), Description("Shows you a Yukari image")]
        public async Task YYukariPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/yukari");
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
                    Title = "Random Yukari Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("teto"), Description("Shows you a Teto image")]
        public async Task KTetoPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/teto");
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
                    Title = "Random Teto Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("len"), Description("Shows you a Len image")]
        public async Task KLenPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/len");
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
                    Title = "Random Len Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("kaito"), Description("Shows you a Kaito image")]
        public async Task KaitoPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/kaito");
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
                    Title = "Random Kaito Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("meiko"), Description("Shows you a Meiko image")]
        public async Task MeikoPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/meiko");
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
                    Title = "Random Meiko Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("fukase"), Description("Shows you a Meiko image")]
        public async Task FukasePic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/fukase");
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
                    Title = "Random Fukase Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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

        [Command("miku"), Description("Shows you a Meiko image")]
        public async Task HMikuPic(CommandContext ctx)
        {
            try
            {
                WebRequest request = WebRequest.Create("https://api.meek.moe/miku");
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                var myresponse = JsonConvert.DeserializeObject<Other.ImgRet>(responseFromServer);
                var emim = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#289b9a"),
                    Title = "Random Miku Image!",
                    Description = $"[Full Source Image Link]({myresponse.url.ToString()})",
                    ImageUrl = $"https://api.meek.moe/im/?image={myresponse.url.ToString()}&resize=500"
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
    }
}
