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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Diagnostics;
using NYoutubeDL;

namespace someBot
{
    class TestStuff
    {
        [Command("vdl")]
        public async Task YTDL(CommandContext ctx, string link)
        {
            if (link.Contains("youtu"))
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[20];
                var random = new System.Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);

                var youtubeDl = new YoutubeDL();

                youtubeDl.Options.FilesystemOptions.Output = $"/var/www/vhosts/srgg.de/why-is-this-a-me.me/ytdl/{finalString}.mp4";
                youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
                youtubeDl.Options.VideoSelectionOptions.NoPlaylist = true;
                youtubeDl.Options.VideoFormatOptions.Format = NYoutubeDL.Helpers.Enums.VideoFormat.best;
                youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
                youtubeDl.Options.PostProcessingOptions.AudioFormat = NYoutubeDL.Helpers.Enums.AudioFormat.mp3;
                youtubeDl.Options.PostProcessingOptions.AudioQuality = "320k";
                youtubeDl.Options.PostProcessingOptions.AddMetadata = true;
                youtubeDl.Options.PostProcessingOptions.EmbedThumbnail = true;

                youtubeDl.VideoUrl = link;

                // Or update the binary
                youtubeDl.Options.GeneralOptions.Update = true;

                // Optional, required if binary is not in $PATH
                youtubeDl.YoutubeDlPath = "youtube-dl";

                //youtubeDl.StandardOutputEvent += (sender, output) => Console.WriteLine(output);
                youtubeDl.StandardErrorEvent += (sender, errorOutput) => Console.WriteLine(errorOutput);

                await youtubeDl.DownloadAsync();

                    await ctx.RespondAsync("https://why-is-this-a-me.me/ytdl/" + finalString + ".mp3 \n" +
                        "**This file bill be deleted in about 30min!**");
                    await Task.Delay((60000 * 30));
                    File.Delete($"/var/www/vhosts/srgg.de/why-is-this-a-me.me/ytdl/{finalString}.mp3");
            }
            else if (link.Contains("nico"))
            {
                var init = ctx.RespondAsync("NND is slow this may take a while!");

                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[20];
                var random = new System.Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);

                var youtubeDl = new YoutubeDL();

                youtubeDl.Options.FilesystemOptions.Output = $"/var/www/vhosts/srgg.de/why-is-this-a-me.me/nnddl/{finalString}.mp4";
                youtubeDl.Options.AuthenticationOptions.Username = "hecc oof";
                youtubeDl.Options.AuthenticationOptions.Password = "get ur own lol";
                youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
                youtubeDl.Options.FilesystemOptions.NoCacheDir = true;
                youtubeDl.Options.VideoFormatOptions.Format = NYoutubeDL.Helpers.Enums.VideoFormat.best;
                youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
                youtubeDl.Options.PostProcessingOptions.AudioFormat = NYoutubeDL.Helpers.Enums.AudioFormat.mp3;
                youtubeDl.Options.PostProcessingOptions.AudioQuality = "320k";
                youtubeDl.Options.PostProcessingOptions.AddMetadata = true;
                youtubeDl.Options.PostProcessingOptions.EmbedThumbnail = true;

                youtubeDl.VideoUrl = link;

                // Or update the binary
                youtubeDl.Options.GeneralOptions.Update = true;

                // Optional, required if binary is not in $PATH
                youtubeDl.YoutubeDlPath = "youtube-dl";

                youtubeDl.StandardErrorEvent += (sender, errorOutput) => ctx.RespondAsync($"{errorOutput}  (If its 403 blame NNDs Servers, hella slow sometimes and thus cancel the download uwu)");

                await youtubeDl.DownloadAsync();

                if (File.Exists($"/var/www/vhosts/srgg.de/why-is-this-a-me.me/nnddl/{finalString}.mp3"))
                {
                    await ctx.RespondAsync("https://why-is-this-a-me.me/nnddl/" + finalString + ".mp3 \n" +
                            "**This file will be deleted in about 30min!**");
                    await Task.Delay((60000 * 30));
                    File.Delete($"/var/www/vhosts/srgg.de/why-is-this-a-me.me/nnddl/{finalString}.mp3");
                }
            }

            else
            {
                await ctx.RespondAsync("no YouTube or NNDlink detected, please try another");
            }
        }
    }
}
