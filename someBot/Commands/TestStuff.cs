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
        public async Task YTDL(CommandContext ctx, string link = null)
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

                youtubeDl.Options.FilesystemOptions.Output = $"/var/www/vhosts/srgg.de/httpdocs/ytdl/{finalString}.mp4";
                youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
                youtubeDl.Options.VideoSelectionOptions.NoPlaylist = true;
                youtubeDl.Options.VideoFormatOptions.Format = NYoutubeDL.Helpers.Enums.VideoFormat.best;
                youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
                youtubeDl.Options.PostProcessingOptions.AudioFormat = NYoutubeDL.Helpers.Enums.AudioFormat.mp3;
                youtubeDl.Options.PostProcessingOptions.AudioQuality = "0";
                youtubeDl.Options.PostProcessingOptions.PostProcessorArgs = "";
                youtubeDl.Options.PostProcessingOptions.AddMetadata = true;

                youtubeDl.VideoUrl = link;

                // Or update the binary
                youtubeDl.Options.GeneralOptions.Update = true;

                // Optional, required if binary is not in $PATH
                youtubeDl.YoutubeDlPath = "youtube-dl";

                youtubeDl.StandardOutputEvent += (sender, output) => Console.WriteLine(output);
                youtubeDl.StandardErrorEvent += (sender, errorOutput) => Console.WriteLine(errorOutput);

                youtubeDl.Download();
                long length = new System.IO.FileInfo($"/var/www/vhosts/srgg.de/httpdocs/ytdl/{finalString}.mp3").Length;
                if (length > 8388000)
                {
                    await ctx.RespondAsync("Size too big for Discord, Use this link!\nhttps://srgg.de/ytdl/" + finalString + ".mp3 \n" +
                        "**This file bill be deleted in about 60min!**");
                    await Task.Delay((60000 * 60));
                    File.Delete($"/var/www/vhosts/srgg.de/httpdocs/ytdl/{finalString}.mp3");

                }
                else
                {
                    await ctx.RespondWithFileAsync($"/var/www/vhosts/srgg.de/httpdocs/ytdl/{finalString}.mp3");
                    File.Delete($"/var/www/vhosts/srgg.de/httpdocs/ytdl/{finalString}.mp3");
                }
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

                youtubeDl.Options.FilesystemOptions.Output = $"/var/www/vhosts/srgg.de/httpdocs/nnddl/{finalString}.mp4";
                youtubeDl.Options.AuthenticationOptions.Username = "Hecc off use your";
                youtubeDl.Options.AuthenticationOptions.Password = "own NND account";
                youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
                youtubeDl.Options.FilesystemOptions.NoCacheDir = true;
                youtubeDl.Options.VideoFormatOptions.Format = NYoutubeDL.Helpers.Enums.VideoFormat.best;
                youtubeDl.Options.PostProcessingOptions.ExtractAudio = true;
                youtubeDl.Options.PostProcessingOptions.AudioFormat = NYoutubeDL.Helpers.Enums.AudioFormat.mp3;
                youtubeDl.Options.PostProcessingOptions.AudioQuality = "0";
                youtubeDl.Options.PostProcessingOptions.PostProcessorArgs = "";
                youtubeDl.Options.PostProcessingOptions.AddMetadata = true;

                youtubeDl.VideoUrl = link;

                // Or update the binary
                youtubeDl.Options.GeneralOptions.Update = true;

                // Optional, required if binary is not in $PATH
                youtubeDl.YoutubeDlPath = "youtube-dl";

                youtubeDl.StandardErrorEvent += (sender, errorOutput) => ctx.RespondAsync($"Error: {errorOutput}  (If its 403 blame NNDs Servers, hella slow sometimes and thus cancel the download uwu)");

                youtubeDl.Download();
                long length = new System.IO.FileInfo($"/var/www/vhosts/srgg.de/httpdocs/nnddl/{finalString}.mp3").Length;
                if (length > 8388000)
                {
                    await ctx.RespondAsync("Size too big for Discord, Use this link!\nhttps://srgg.de/nnddl/" + finalString + ".mp3 \n" +
                        "**This file bill be deleted in about 60min!**");
                    await Task.Delay((60000 * 60));
                    File.Delete($"/var/www/vhosts/srgg.de/httpdocs/nnddl/{finalString}.mp3");

                }
                else
                {
                    await ctx.RespondWithFileAsync($"/var/www/vhosts/srgg.de/httpdocs/nnddl/{finalString}.mp3");
                    await init.Result.ModifyAsync();
                    await Task.Delay(60000);
                    File.Delete($"/var/www/vhosts/srgg.de/httpdocs/nnddl/{finalString}.mp3");
                }
            }

            else
            {
                await ctx.RespondAsync("no YouTube or NNDlink detected, please try another");
            }
        }
    }
}
