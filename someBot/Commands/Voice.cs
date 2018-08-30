using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Lavalink.EventArgs;
using DSharpPlus.VoiceNext;
using DSharpPlus.Entities;
using System.Linq;
using System.Collections.Generic;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;

namespace someBot.Commands
{
    class Voice : BaseCommandModule
    {
        [Command("join")]
        public async Task LLinkJoin(CommandContext ctx, [RemainingText] string uri = null)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    await ctx.RespondAsync("You need to be in a voice channel.");
                    return;
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                var con = Bot.guit[0].LLinkCon;
                if (Bot.guit[pos].LLGuild == null)
                {
                    Bot.guit[pos].LLGuild = await con.ConnectAsync(chn);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("play"), Aliases("p"), Cooldown(1, 5, CooldownBucketType.Guild)]
        public async Task LLink(CommandContext ctx, [RemainingText] string uri = null)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    await ctx.RespondAsync("You need to be in a voice channel.");
                    return;
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1 || (uri == null && Bot.guit[pos].queue.Count == 0))
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                var con = Bot.guit[0].LLinkCon;
                bool wasdisc = false;
                if (Bot.guit[pos].LLGuild == null)
                {
                    Bot.guit[pos].LLGuild = await con.ConnectAsync(chn);
                    wasdisc = true;
                }
                else if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    Bot.guit[pos].LLGuild = await con.ConnectAsync(chn);
                    wasdisc = true;
                }
                if (wasdisc && uri == null)
                {
                    if (Bot.guit[pos].queue.Count != 0)
                    {
                        await ctx.RespondAsync("Resuming queue!");
                    }
                    Bot.guit[pos].audioPlay.QueueLoop(pos, ctx);
                }
                else if (wasdisc && uri != null)
                {
                    if (Bot.guit[pos].queue.Count != 0)
                    {
                        await ctx.RespondAsync("Resuming queue!");
                    }
                    Bot.guit[pos].audioPlay.PlaySong(pos, ctx, uri);
                }
                else if (!wasdisc && Bot.guit[pos].queue.Count < 1 && !Bot.guit[pos].playing)
                {
                    Bot.guit[pos].audioPlay.PlaySong(pos, ctx, uri);
                }
                else if (!wasdisc && Bot.guit[pos].queue.Count > 1 && Bot.guit[pos].playing)
                {
                    var q_It = Bot.guit[pos].audioPlay.QueueSong(pos, ctx, uri);
                    q_It.Wait();
                }
                else if (!wasdisc && Bot.guit[pos].queue.Count < 1 && Bot.guit[pos].playing)
                {
                    Bot.guit[pos].queue.Add(new Gsets2
                    {
                        LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                        requester = Bot.guit[pos].playnow.requester,
                        addtime = Bot.guit[pos].playnow.addtime
                    });
                    var q_It = Bot.guit[pos].audioPlay.QueueSong(pos, ctx, uri);
                    q_It.Wait();
                }
                else
                {
                    var q_It = Bot.guit[pos].audioPlay.QueueSong(pos, ctx, uri);
                    q_It.Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("leave")]
        public async Task LLinkleave(CommandContext ctx)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    throw new InvalidOperationException("You need to be in a voice channel.");
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                {
                    Bot.guit[pos].playnow.sstop = true;
                }
                Bot.guit[pos].paused = false;
                Bot.guit[pos].LLGuild.Disconnect();
                Bot.guit[pos].LLGuild = null;
                Bot.guit[pos].playing = false;
                await ctx.RespondAsync("disconnected");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("stop")]
        public async Task LLinkstop(CommandContext ctx)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    throw new InvalidOperationException("You need to be in a voice channel.");
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                {
                    Bot.guit[pos].playnow.sstop = true;
                }
                var stop = Bot.guit[pos].audioFunc.Stop(pos);
                stop.Wait();
                await Task.Delay(2500);
                Bot.guit[pos].stoppin = false;
                await ctx.RespondAsync("stopped");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("skip")]
        public async Task LLinkskip(CommandContext ctx)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    throw new InvalidOperationException("You need to be in a voice channel.");
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                if (Bot.guit[pos].queue.Count > 1)
                {
                    if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                    {
                        Bot.guit[pos].playnow.sstop = true;
                    }
                    var stop = Bot.guit[pos].audioFunc.Skip(pos);
                    stop.Wait();
                    await ctx.RespondAsync("skipped");
                }
                else
                {
                    if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                    {
                        Bot.guit[pos].playnow.sstop = true;
                    }
                    var stop = Bot.guit[pos].audioFunc.Skip(pos);
                    stop.Wait();
                    await ctx.RespondAsync("skipped");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("volume"), Aliases("vol")]
        public async Task LLinkvol(CommandContext ctx, int vol = 100)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    throw new InvalidOperationException("You need to be in a voice channel.");
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                if (vol > 150) vol = 150;
                Bot.guit[pos].LLGuild.SetVolume(vol);
                await ctx.RespondAsync($"Volume changed to **{vol}** (150 is max)");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("pause")]
        public async Task LLinkpa(CommandContext ctx)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    throw new InvalidOperationException("You need to be in a voice channel.");
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                var pause = Bot.guit[pos].audioFunc.Pause(pos);
                pause.Wait();
                await ctx.RespondAsync($"**Paused**");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("resume")]
        public async Task LLinkresume(CommandContext ctx)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    throw new InvalidOperationException("You need to be in a voice channel.");
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                var botusr = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id);
                if (Bot.guit[pos].playing)
                {
                    var resume = Bot.guit[pos].audioFunc.Resume(pos);
                    resume.Wait();
                }
                else
                {
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
                    Bot.guit[pos].audioPlay.QueueLoop(pos, ctx);
                }
                await ctx.RespondAsync($"**Resumed**");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("queueremove"), Aliases("qr")]
        public async Task LLinkqreme(CommandContext ctx, int r)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    throw new InvalidOperationException("You need to be in a voice channel.");
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (!Bot.guit[pos].LLGuild.IsConnected || Bot.guit[pos].queue.Count == 1)
                {
                    return;
                }
                int pos2 = ctx.Member.Roles.ToList().FindIndex(x => x.CheckPermission(DSharpPlus.Permissions.ManageMessages) == DSharpPlus.PermissionLevel.Allowed);
                int pos3 = ctx.Member.Roles.ToList().FindIndex(x => x.CheckPermission(DSharpPlus.Permissions.Administrator) == DSharpPlus.PermissionLevel.Allowed);
                if (r > Bot.guit[pos].queue.Count - 1)
                {
                    return;
                }
                else if (ctx.Member == Bot.guit[pos].queue[r].requester || pos2 != -1 || pos3 != -1)
                {
                    await ctx.RespondAsync($"Removed: **{Bot.guit[pos].queue[r].LavaTrack.Title}** by **{Bot.guit[pos].queue[r].LavaTrack.Author}**");
                    var oi = Bot.guit[pos].audioQueue.queueRemove(pos, r);
                    oi.Wait();
                }
                else
                {
                    await ctx.RespondAsync("You need the manage messages permission to delete others tracks");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("queueremovesome"), Aliases("qrs")]
        public async Task LLinkqrs(CommandContext ctx, int beg, int end = 0)
        {
            try
            {
                //m%play https://www.youtube.com/watch?v=pEUe2g9O_ks
                var chn = ctx.Member?.VoiceState?.Channel;
                if (chn == null)
                {
                    throw new InvalidOperationException("You need to be in a voice channel.");
                }
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (!Bot.guit[pos].LLGuild.IsConnected || Bot.guit[pos].queue.Count == 1 || end > Bot.guit[pos].queue.Count - beg - 1)
                {
                    return;
                }
                if (end == 0)
                {
                    end = Bot.guit[pos].queue.Count - beg - 1;
                }
                await ctx.RespondAsync($"Removed: **{end + 1 - beg} Songs**");
                var qrs = Bot.guit[pos].audioQueue.queueRemoveSome(pos, beg, end);
                qrs.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("queue"), Aliases("q")]
        public async Task queueAllLL(CommandContext ctx)
        {
            try
            {
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                var chn = ctx.Member?.VoiceState?.Channel;
                if (pos == -1)
                {
                    return;
                }
                var getQ = Bot.guit[pos].audioQueue.QueueList(pos, ctx);
                getQ.Wait();
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            await Task.CompletedTask;
        }

        [Command("repeat"), Aliases("r")]
        public async Task RepeatOn(CommandContext ctx)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            var chn = ctx.Member?.VoiceState?.Channel;
            if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
            {
                return;
            }
            Bot.guit[pos].cmdChannel = ctx.Channel.Id;
            var repeat = Bot.guit[pos].audioFunc.Repeat(pos);
            repeat.Wait();
            await ctx.RespondAsync("Repeat is set to " + Bot.guit[pos].repeat.ToString().ToLower());
        }

        [Command("repeatall"), Aliases("ra")]
        public async Task RepeatOnAll(CommandContext ctx)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            var chn = ctx.Member?.VoiceState?.Channel;
            if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
            {
                return;
            }
            Bot.guit[pos].cmdChannel = ctx.Channel.Id;
            var repeatall = Bot.guit[pos].audioFunc.RepeatAll(pos);
            repeatall.Wait();
            await ctx.RespondAsync("Repeat all is set to " + Bot.guit[pos].repeatAll.ToString().ToLower());
        }

        [Command("shuffle"), Aliases("s")]
        public async Task ShuffOn(CommandContext ctx)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            var chn = ctx.Member?.VoiceState?.Channel;
            if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
            {
                return;
            }
            Bot.guit[pos].cmdChannel = ctx.Channel.Id;
            var shuffle = Bot.guit[pos].audioFunc.Shuffle(pos);
            shuffle.Wait();
            await ctx.RespondAsync("Shuffle is set to " + Bot.guit[pos].shuffle.ToString().ToLower());
        }

        [Command("queueclear"), Aliases("qc")]
        public async Task QueCl(CommandContext ctx)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            var chn = ctx.Member?.VoiceState?.Channel;
            if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
            {
                return;
            }
            Bot.guit[pos].cmdChannel = ctx.Channel.Id;
            if (Bot.guit[pos].playing)
            {
                Bot.guit[pos].queue.RemoveRange(1, Bot.guit[pos].queue.Count - 1);
            }
            else
            {
                Bot.guit[pos].queue.Clear();
            }
            await ctx.RespondAsync("Cleared Queue");
        }

        [Command("playlist"), Aliases("pp")]
        public async Task QueP(CommandContext ctx, [RemainingText] string uri)
        {
            try
            {
                var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
                var chn = ctx.Member?.VoiceState?.Channel;
                if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
                {
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (!uri.StartsWith("https://") && !uri.StartsWith("http://"))
                {
                    await ctx.RespondAsync("no valid playlist link");
                    return;
                }
                var con = Bot.guit[0].LLinkCon;
                var datrack = await con.GetTracksAsync(new Uri(uri));
                int couldadd = datrack.Count();
                foreach (var dracks in datrack)
                {
                    if (dracks.Author == null)
                    {
                        couldadd--;
                        continue;
                    }
                    Bot.guit[pos].queue.Add(new Gsets2
                    {
                        LavaTrack = dracks,
                        requester = ctx.Member,
                        addtime = DateTime.Now
                    });
                }
                await ctx.RespondAsync($"Added {datrack.Count()} songs to queue ({Bot.guit[pos].queue.Count} in queue now)");
                if (couldadd != datrack.Count())
                {
                    await ctx.RespondAsync("Not all Songs were loaded, this could be due to an error or the video being blocked");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        [Command("nowplaying"), Aliases("np")]
        public async Task NowPl(CommandContext ctx)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            var chn = ctx.Member?.VoiceState?.Channel;
            if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn || Bot.guit[pos].playnow.requester == null)
            {
                return;
            }
            Bot.guit[pos].cmdChannel = ctx.Channel.Id;
            var eb = new DiscordEmbedBuilder
            {
                Color = new DiscordColor("#68D3D2"),
                Title = "Now Playing",
                Description = "**__Current Song:__**",
                ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
            };
            var que = Bot.guit[pos].playnow;
            if (que.LavaTrack.Uri.ToString().Contains("youtu"))
            {
                try
                {
                    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                    {
                        ApiKey = "AIzaSyDbj184qjOOS8fE6PlHcuyasA8VB_gr_f0",
                        ApplicationName = this.GetType().ToString()
                    });

                    var searchListRequest = youtubeService.Search.List("snippet");
                    searchListRequest.Q = que.LavaTrack.Title + " " + que.LavaTrack.Author; // Replace with your search term.
                    searchListRequest.MaxResults = 1;
                    searchListRequest.Type = "video";
                    string time1 = "";
                    string time2 = "";
                    if (que.LavaTrack.Length.Hours < 1)
                    {
                        time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                        time2 = que.LavaTrack.Length.ToString(@"mm\:ss");
                    }
                    else
                    {
                        time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                        time2 = que.LavaTrack.Length.ToString(@"hh\:mm\:ss");
                    }
                    // Call the search.list method to retrieve results matching the specified query term.
                    var searchListResponse = await searchListRequest.ExecuteAsync();
                    eb.AddField($"{que.LavaTrack.Title} ({time1}/{time2})", $"[Video Link]({que.LavaTrack.Uri})\n" +
                        $"[{que.LavaTrack.Author}](https://www.youtube.com/channel/" + searchListResponse.Items[0].Snippet.ChannelId +")");
                    eb.AddField("Description", searchListResponse.Items[0].Snippet.Description);
                    eb.WithImageUrl(searchListResponse.Items[0].Snippet.Thumbnails.High.Url);
                }
                catch
                {
                    //eb.AddField($"{que.LavaTrack.Title} ({que.LavaTrack.Length})", $"By {que.LavaTrack.Author}\n[Link]({que.LavaTrack.Uri})\nRequested by {que.requester.Mention}");
                }
            }
            else
            {
                string time1 = "";
                string time2 = "";
                if (que.LavaTrack.Length.Hours < 1)
                {
                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                    time2 = que.LavaTrack.Length.ToString(@"mm\:ss");
                }
                else
                {
                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                    time2 = que.LavaTrack.Length.ToString(@"hh\:mm\:ss");
                }
                eb.AddField($"{que.LavaTrack.Title} ({time1}/{time2})", $"By {que.LavaTrack.Author}\n[Link]({que.LavaTrack.Uri})\nRequested by {que.requester.Mention}");
            }
            await ctx.RespondAsync(embed: eb.Build());
        }
    }
}
