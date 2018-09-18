using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace someBot.Commands.MusicEx
{
    public class Qadd
    {
        public async Task QueueSong(int pos, CommandContext ctx, string song = null) //Queue only
        {
            DSharpPlus.Lavalink.LavalinkTrack track;
            var inter = ctx.Client.GetInteractivity();
            string pora = "Playing";
            string end = "";
            if (!song.StartsWith("https://") && !song.StartsWith("http://")) {
                var tra = await Bot.guit[0].LLinkCon.GetTracksAsync(song);
                if (tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.NoMatches || tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.LoadFailed) {
                    await ctx.RespondAsync("An error occoured while loading the song, this could be due to the song being region locked uwu");
                    await Task.CompletedTask;
                    return;
                } if (tra.Tracks.Count() == 1) {
                    track = tra.Tracks.First();
                    Console.WriteLine($"[{ctx.Guild.Id}] Added to queue: {track.Title} by {track.Author}");
                } else {
                    var selem = new DiscordEmbedBuilder {
                        Color = new DiscordColor("#68D3D2"),
                        Title = "Results are in!",
                        Description = "Multiple tracks were found, select one!\n React with the number you want to add",
                        ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                    };
                    DiscordEmoji[] nums = { DiscordEmoji.FromName(ctx.Client, ":one:"), DiscordEmoji.FromName(ctx.Client, ":two:"), DiscordEmoji.FromName(ctx.Client, ":three:"), DiscordEmoji.FromName(ctx.Client, ":four:"), DiscordEmoji.FromName(ctx.Client, ":five:") };
                    int er = 0;
                    var TraList = tra.Tracks.ToList();
                    foreach (var ztacks in TraList) {
                        if (er == tra.Tracks.ToList().Count || er == 4) break;
                        string time2 = "";
                        if (ztacks.Length.Hours < 1) time2 = ztacks.Length.ToString(@"mm\:ss");
                        else time2 = ztacks.Length.ToString(@"hh\:mm\:ss");
                        selem.AddField($"{nums[er]} **{ztacks.Title}** by **{ztacks.Author}**", $"Length: ({time2}) [Link]({ztacks.Uri.OriginalString})");
                        er++;
                    } if (er == 0) {
                        await ctx.RespondAsync("An error occoured while loading the song, this could be due to the song being region locked uwu");
                        await Task.CompletedTask;
                        return;
                    } try
                    {
                        var mes = await ctx.RespondAsync(embed: selem.Build());
                        int o = 0;
                        foreach (var fld in mes.Embeds.First().Fields)
                        {
                            try { await mes.CreateReactionAsync(nums[o]); } catch { }
                            o++;
                        }
                        try { await mes.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":x:")); } catch { }
                        var ems = await inter.WaitForMessageReactionAsync(xe => (xe == nums[0] || xe == nums[1] || xe == nums[2] || xe == nums[3] || xe == nums[4] || xe == DiscordEmoji.FromName(ctx.Client, ":x:")), mes, ctx.User, TimeSpan.FromSeconds(30));
                        if (ems.Emoji == nums[0]) track = tra.Tracks.ElementAt(0);
                        else if (ems.Emoji == nums[1]) track = tra.Tracks.ElementAt(1);
                        else if (ems.Emoji == nums[2]) track = tra.Tracks.ElementAt(2);
                        else if (ems.Emoji == nums[3]) track = tra.Tracks.ElementAt(3);
                        else if (ems.Emoji == nums[4]) track = tra.Tracks.ElementAt(4);
                        else if (ems.Emoji == DiscordEmoji.FromName(ctx.Client, ":x:"))
                        {
                            await mes.DeleteAsync();
                            await Task.CompletedTask;
                            return;
                        }
                        else track = tra.Tracks.First();
                        await mes.DeleteAsync();
                    }
                    catch { track = tra.Tracks.First(); }
                    Console.WriteLine($"[{ctx.Guild.Id}] Added to queue: {track.Title} by {track.Author}");
                }
            } else {
                var tra = await Bot.guit[0].LLinkCon.GetTracksAsync(new Uri(song));
                if (tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.NoMatches || tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.LoadFailed) {
                    await ctx.RespondAsync("An error occoured while loading the song, this could be due to the song being region locked uwu");
                    await Task.CompletedTask;
                    return;
                } if (tra.PlaylistInfo.SelectedTrack == -1) {
                    await ctx.RespondAsync("To load a playlist use ``m!pp (link)`` and then ``m!p`` to start playback (if nothing is playing at the moment)");
                    await Task.CompletedTask;
                    return;
                }
                track = tra.Tracks.First();
                Console.WriteLine($"[{ctx.Guild.Id}] Added to queue: {track.Title} by {track.Author}");
                Console.WriteLine(tra.PlaylistInfo.SelectedTrack + " yeet");
                if (tra.LoadResultType == DSharpPlus.Lavalink.LavalinkLoadResultType.PlaylistLoaded) track = tra.Tracks.ElementAt(tra.PlaylistInfo.SelectedTrack);
            } if (Bot.guit[pos].queue.Count > 0) {
                pora = "Added";
                end = "to the queue!";
            }
            Bot.guit[pos].queue.Add(new Gsets2 {
                LavaTrack = track,
                requester = ctx.Member,
                addtime = DateTime.Now
            });
            int yoyo = Bot.guit[pos].queue.FindIndex(x => x.LavaTrack.Uri == track.Uri && x.requester == ctx.Member);
            string uwu = Bot.guit[pos].queue[yoyo].requester.Username;
            if (Bot.guit[pos].queue[yoyo].requester.Nickname != null) uwu += $" ({Bot.guit[pos].queue[yoyo].requester.Nickname})";
            await ctx.RespondAsync($"{pora}: **{Bot.guit[pos].queue[yoyo].LavaTrack.Title}** by **{Bot.guit[pos].queue[yoyo].LavaTrack.Author}** {end}\nRequested by: {uwu}");
            await Task.CompletedTask;
        }
    }
}
