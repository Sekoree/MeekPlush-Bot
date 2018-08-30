using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace someBot.Commands.Audio
{
    public class Playback
    {
        public async void PlaySong(int pos, CommandContext ctx, string song) //Queue then play
        {
            try
            {
                Console.WriteLine("Here now!");
                var chn = ctx.Member?.VoiceState?.Channel;
                var con = Bot.guit[0].LLinkCon;
                if (chn != Bot.guit[pos].LLGuild.Channel)
                {
                    return;
                }
                if (song == null && Bot.guit[pos].queue.Count > 0)
                {
                    await ctx.RespondAsync("Playing preloaded playlist/queue!");
                }
                if (song != null)
                {
                    var queue_it = QueueSong(pos, ctx, song);
                    queue_it.Wait();
                    Console.WriteLine("Adding done!");
                }
                await Task.Delay(500);
                QueueLoop(pos, ctx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task QueueSong(int pos, CommandContext ctx, string song) //Queue only
        {
            try
            {
                IEnumerable<DSharpPlus.Lavalink.LavalinkTrack> track;
                string pora = "Playing";
                string end = "";
                if (!song.StartsWith("https://") && !song.StartsWith("http://"))
                {
                    track = await Bot.guit[0].LLinkCon.GetTracksAsync(song);
                    if (track.First().Author == null)
                    {
                        await ctx.RespondAsync("Nothing found uwu or error while trying to load track (maybe geoblock)");
                        return;
                    }
                }
                else
                {
                    track = await Bot.guit[0].LLinkCon.GetTracksAsync(new Uri(song));
                    if (track.First().Author == null)
                    {
                        await ctx.RespondAsync("Nothing found uwu or error while trying to load track (maybe geoblock)");
                        return;
                    }
                }
                if (Bot.guit[pos].queue.Count > 0)
                {
                    pora = "Added";
                    end = "to the queue!";
                }
                Bot.guit[pos].queue.Add(new Gsets2
                {
                    LavaTrack = track.First(),
                    requester = ctx.Member,
                    addtime = DateTime.Now
                });
                int yoyo = Bot.guit[pos].queue.FindIndex(x => x.LavaTrack.Uri == track.First().Uri && x.requester == ctx.Member);
                string uwu = Bot.guit[pos].queue[yoyo].requester.Username;
                if (Bot.guit[pos].queue[yoyo].requester.Nickname != null) uwu += $" ({Bot.guit[pos].queue[yoyo].requester.Nickname})";
                await ctx.RespondAsync($"{pora}: **{Bot.guit[pos].queue[yoyo].LavaTrack.Title}** by **{Bot.guit[pos].queue[yoyo].LavaTrack.Author}** {end}\nRequested by: {uwu}");
                await Task.CompletedTask;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async void QueueLoop(int pos, CommandContext ctx) //Start playback without queueing
        {
            Console.WriteLine("startloop");
            var con = Bot.guit[0].LLinkCon;
            try
            {
                Bot.guit[pos].LLGuild.PlaybackFinished -= Bot.guit[pos].audioEvents.PlayFin;
                Bot.guit[pos].LLGuild.TrackStuck -= Bot.guit[pos].audioEvents.PlayStu;
                Bot.guit[pos].LLGuild.TrackException -= Bot.guit[pos].audioEvents.PlayErr;
            }
            catch { }
            Bot.guit[pos].LLGuild.PlaybackFinished += Bot.guit[pos].audioEvents.PlayFin;
            Bot.guit[pos].LLGuild.TrackStuck += Bot.guit[pos].audioEvents.PlayStu;
            Bot.guit[pos].LLGuild.TrackException += Bot.guit[pos].audioEvents.PlayErr;
            while (Bot.guit[pos].queue.Count != 0)
            {
                if (Bot.guit[pos].LLGuild == null || Bot.guit[pos].stoppin)
                {
                    Console.WriteLine("stop");
                    break;
                }
                Console.WriteLine("in loop");
                System.Random rnd = new System.Random();
                int rr = 0;
                if (Bot.guit[pos].shuffle)
                {
                    rr = rnd.Next(0, Bot.guit[pos].queue.Count);
                }
                if (Bot.guit[pos].repeatAll)
                {
                    Bot.guit[pos].rAint++;
                    rr = Bot.guit[pos].rAint;
                    if (Bot.guit[pos].rAint == Bot.guit[pos].queue.Count)
                    {
                        Bot.guit[pos].rAint = 0;
                        rr = 0;
                    }
                }
                await Task.Delay(250);
                var nps = Bot.guit[pos].audioEvents.setNP(pos, Bot.guit[pos].queue[rr]);
                nps.Wait();
                if (Bot.guit[pos].queue[rr].LavaTrack.Author != null)
                {
                    var ll = LavaLinkHandOff(pos, Bot.guit[pos].playnow.LavaTrack, ctx, rr);
                    ll.Wait();
                    Console.WriteLine(Bot.guit[pos].playing);
                    if (!Bot.guit[pos].repeat && !Bot.guit[pos].repeatAll && Bot.guit[pos].LLGuild != null && !Bot.guit[pos].stoppin)
                    {
                        try
                        {
                            Bot.guit[pos].queue.RemoveAt(rr);
                        }
                        catch { }
                    }
                }
            }
            try
            {
                Bot.guit[pos].LLGuild.PlaybackFinished -= Bot.guit[pos].audioEvents.PlayFin;
                Bot.guit[pos].LLGuild.TrackStuck -= Bot.guit[pos].audioEvents.PlayStu;
                Bot.guit[pos].LLGuild.TrackException -= Bot.guit[pos].audioEvents.PlayErr;
            }
            catch { }
            Console.WriteLine("end");
            await Task.CompletedTask;
        }

        public async Task LavaLinkHandOff(int pos, DSharpPlus.Lavalink.LavalinkTrack track, CommandContext ctx, int rr)
        {
            try
            {
                Console.WriteLine("playStart");
                if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                {
                    var naet = await Bot.guit[0].LLinkCon.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                    Bot.guit[pos].playnow.LavaTrack = naet.First();
                }
                IEnumerable<DSharpPlus.Lavalink.LavalinkTrack> datrack2 = await Bot.guit[0].LLinkCon.GetTracksAsync(new Uri(Bot.guit[pos].queue[rr].LavaTrack.Uri.OriginalString));
                if (datrack2.Count() == 0)
                {
                    try
                    {
                        Bot.guit[pos].queue.RemoveAt(rr);
                    }
                    catch { }
                    await ctx.RespondAsync("Track error, maybe regionlocked, skipped >>");
                    return;
                }
                await Task.Delay(250);
                Bot.guit[pos].LLGuild.Play(track);
                var sp = Bot.guit[pos].audioEvents.setPlay(pos);
                sp.Wait();
                Console.WriteLine(Bot.guit[pos].playing);
                while (Bot.guit[pos].playing)
                {
                    if (ctx.Client.GetGuildAsync(Bot.guit[pos].GID).Result.GetMemberAsync(ctx.Client.CurrentUser.Id).Result.VoiceState?.Channel.Users.Where(x => x.IsBot == false).Count() == 0)
                    {
                        if (DateTime.Now.Subtract(Bot.guit[pos].offtime).TotalMinutes > 4)
                        {
                            Bot.guit[pos].LLGuild.PlaybackFinished -= Bot.guit[pos].audioEvents.PlayFin;
                            Bot.guit[pos].LLGuild.TrackStuck -= Bot.guit[pos].audioEvents.PlayStu;
                            Bot.guit[pos].LLGuild.TrackException -= Bot.guit[pos].audioEvents.PlayErr;
                            Bot.guit[pos].paused = false;
                            Bot.guit[pos].LLGuild.Disconnect();
                            Bot.guit[pos].LLGuild = null;
                            Bot.guit[pos].playing = false;
                            break;
                        }
                    }
                    else
                    {
                        Bot.guit[pos].offtime = DateTime.Now;
                    }
                    await Task.Delay(25);
                }
            }
            catch { }
            await Task.CompletedTask;
        }
    }
}
