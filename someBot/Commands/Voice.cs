using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Lavalink.EventArgs;
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
                var con = Bot.guit[0].LLinkCon;
                if (Bot.guit[pos].LLGuild == null)
                {
                    //if(Bot.guit[pos].LLGuild.Channel != chn) {
                        Bot.guit[pos].LLGuild = await con.ConnectAsync(chn);
                    //}
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
                var con = Bot.guit[0].LLinkCon;
                if (Bot.guit[pos].LLGuild == null)
                {
                    Bot.guit[pos].LLGuild = await con.ConnectAsync(chn);
                }
                if (uri == null)
                {
                    await ctx.RespondAsync("Playing preloaded playlist/queue!");
                    Bot.guit[pos].LLGuild.PlaybackFinished += PlayFin;
                    Bot.guit[pos].LLGuild.TrackStuck += PlayStu;
                    Bot.guit[pos].LLGuild.TrackException += PlayErr;
                }
                else if (Bot.guit[pos].playing)
                {
                    if (chn != Bot.guit[pos].LLGuild.Channel)
                    {
                        return;
                    }
                    if (!uri.StartsWith("https://") && !uri.StartsWith("http://"))
                    {
                        var datrack = await con.GetTracksAsync(uri);
                        Bot.guit[pos].queue.Add(new Gsets2
                        {
                            LavaTrack = datrack.First(),
                            requester = ctx.Member
                        });
                        int yoyo = Bot.guit[pos].queue.FindIndex(x => x.LavaTrack.Uri == datrack.First().Uri);
                        string uwu = Bot.guit[pos].queue[yoyo].requester.Username;
                        if (Bot.guit[pos].queue[yoyo].requester.Nickname != null) uwu += $" ({Bot.guit[pos].queue[yoyo].requester.Nickname})";
                        await ctx.RespondAsync($"Added: **{Bot.guit[pos].queue[yoyo].LavaTrack.Title}** by **{Bot.guit[pos].queue[yoyo].LavaTrack.Author}** to the queue!\nRequested by: {uwu}");
                    }
                    else
                    {
                        var datrack = await con.GetTracksAsync(new Uri(uri));
                        Bot.guit[pos].queue.Add(new Gsets2
                        {
                            LavaTrack = datrack.First(),
                            requester = ctx.Member
                        });
                        int yoyo = Bot.guit[pos].queue.FindIndex(x => x.LavaTrack.Uri == datrack.First().Uri);
                        string uwu = Bot.guit[pos].queue[yoyo].requester.Username;
                        if (Bot.guit[pos].queue[yoyo].requester.Nickname != null) uwu += $" ({Bot.guit[pos].queue[yoyo].requester.Nickname})";
                        await ctx.RespondAsync($"Added: **{Bot.guit[pos].queue[yoyo].LavaTrack.Title}** by **{Bot.guit[pos].queue[yoyo].LavaTrack.Author}** to the queue!\nRequested by: {uwu}");
                    }
                    
                    return;
                }
                else
                {
                    if (!uri.StartsWith("https://") && !uri.StartsWith("http://"))
                    {
                        var datrack = await con.GetTracksAsync(uri);
                        Bot.guit[pos].queue.Add(new Gsets2
                        {
                            LavaTrack = datrack.First(),
                            requester = ctx.Member
                        });
                        int yoyo = Bot.guit[pos].queue.FindIndex(x => x.LavaTrack.Uri == datrack.First().Uri);
                        string uwu = Bot.guit[pos].queue[yoyo].requester.Username;
                        if (Bot.guit[pos].queue[yoyo].requester.Nickname != null) uwu += $" ({Bot.guit[pos].queue[yoyo].requester.Nickname})";
                        await ctx.RespondAsync($"Playing: **{Bot.guit[pos].queue[yoyo].LavaTrack.Title}** by **{Bot.guit[pos].queue[yoyo].LavaTrack.Author}**\nRequested by: {uwu}");
                    }
                    else
                    {
                        var datrack = await con.GetTracksAsync(new Uri(uri));
                        Bot.guit[pos].queue.Add(new Gsets2
                        {
                            LavaTrack = datrack.First(),
                            requester = ctx.Member
                        });
                        int yoyo = Bot.guit[pos].queue.FindIndex(x => x.LavaTrack.Uri == datrack.First().Uri);
                        string uwu = Bot.guit[pos].queue[yoyo].requester.Username;
                        if (Bot.guit[pos].queue[yoyo].requester.Nickname != null) uwu += $" ({Bot.guit[pos].queue[yoyo].requester.Nickname})";
                        await ctx.RespondAsync($"Playing: **{Bot.guit[pos].queue[yoyo].LavaTrack.Title}** by **{Bot.guit[pos].queue[yoyo].LavaTrack.Author}**\nRequested by: {uwu}");
                    }
                    Bot.guit[pos].LLGuild.PlaybackFinished += PlayFin;
                    Bot.guit[pos].LLGuild.TrackStuck += PlayStu;
                    Bot.guit[pos].LLGuild.TrackException += PlayErr;
                }
                while (Bot.guit[pos].queue.Count != 0)
                {
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
                    Bot.guit[pos].playnow.LavaTrack = Bot.guit[pos].queue[rr].LavaTrack;
                    Bot.guit[pos].playnow.requester = Bot.guit[pos].queue[rr].requester;
                    if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                    {
                        Bot.guit[pos].playnow.sstop = false;
                    }
                    else
                    {
                        Bot.guit[pos].playnow.sstop = true;
                    }
                    await Task.Delay(500);
                    if (Bot.guit[pos].queue[rr].LavaTrack.Author != null)
                    {
                        if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                        {
                            var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                            Bot.guit[pos].playnow.LavaTrack = naet.First();
                        }
                        await Task.Delay(500);
                        Bot.guit[pos].LLGuild.Play(Bot.guit[pos].playnow.LavaTrack);
                        await Task.Delay(500);
                        Bot.guit[pos].playing = true;
                        await Task.Delay(500);
                        //await ctx.RespondAsync($"Playing: **{Bot.guit[pos].queue[0].LavaTrack.Title}** by **{Bot.guit[pos].queue[0].LavaTrack.Author}**");
                        while (Bot.guit[pos].playing)
                        {
                            if (ctx.Client.GetGuildAsync(Bot.guit[pos].GID).Result.GetMemberAsync(ctx.Client.CurrentUser.Id).Result.VoiceState?.Channel.Users.Where(x => x.IsBot == false).Count() == 0)
                            {
                                if (DateTime.Now.Subtract(Bot.guit[pos].offtime).TotalMinutes > 5)
                                {
                                    Bot.guit[pos].LLGuild.PlaybackFinished -= PlayFin;
                                    Bot.guit[pos].LLGuild.TrackStuck -= PlayStu;
                                    Bot.guit[pos].LLGuild.TrackException -= PlayErr;
                                    Bot.guit[pos].LLGuild.Disconnect();
                                    Bot.guit[pos].LLGuild = null;
                                    break;
                                }
                            }
                            else
                            {
                                Bot.guit[pos].offtime = DateTime.Now;
                            }
                            await Task.Delay(1000);
                        }
                    }
                    if (!Bot.guit[pos].repeat && !Bot.guit[pos].repeatAll)
                    {
                        try
                        {
                            Bot.guit[pos].queue.RemoveAt(rr);
                        }
                        catch { }
                    }
                }
                try
                {
                    Bot.guit[pos].LLGuild.PlaybackFinished -= PlayFin;
                    Bot.guit[pos].LLGuild.TrackStuck -= PlayStu;
                    Bot.guit[pos].LLGuild.TrackException -= PlayErr;
                }
                catch { }
                if (ctx.Client.GetGuildAsync(Bot.guit[pos].GID).Result.GetMemberAsync(ctx.Client.CurrentUser.Id).Result.VoiceState?.Channel.Users.Where(x => x.IsBot == false).Count() == 0 || Bot.guit[pos].queue.Count < 1)
                {
                    await Task.Delay(TimeSpan.FromMinutes(5));
                    if (DateTime.Now.Subtract(Bot.guit[pos].offtime).TotalMinutes > 5)
                    {
                        Bot.guit[pos].LLGuild.Disconnect();
                        Bot.guit[pos].LLGuild = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task PlayFin(TrackFinishEventArgs lg)
        {
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == lg.Player.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream && !Bot.guit[pos].playnow.sstop)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester
                });
                Console.WriteLine("LL Stream Error");
            }
            Bot.guit[pos].playing = false;
            await Task.CompletedTask;
        }

        public async Task PlayStu(TrackStuckEventArgs ts)
        {
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == ts.Player.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester
                });
            }
            await ts.Player.Channel.SendMessageAsync("Track was stuck, so it was skipped >>");
            Console.WriteLine("LL Stuck");
            Bot.guit[pos].playing = false;
            await Task.CompletedTask;
        }

        public async Task PlayErr(TrackExceptionEventArgs ts)
        {
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == ts.Player.Guild.Id);
            if (pos == -1)
            {
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester
                });
            }
            await ts.Player.Channel.SendMessageAsync($"There was an error with the track, so it was skipped ({ts.Error.First()})>>");
            Console.WriteLine("LL Error");
            Bot.guit[pos].playing = false;
            await Task.CompletedTask;
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
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                {
                    Bot.guit[pos].playnow.sstop = true;
                }
                Bot.guit[pos].LLGuild.Stop();
                Bot.guit[pos].LLGuild.Disconnect();
                Bot.guit[pos].LLGuild = null;
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
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                {
                    Bot.guit[pos].playnow.sstop = true;
                }
                Bot.guit[pos].LLGuild.Stop();
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
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                if (Bot.guit[pos].queue.Count < 1)
                {
                    Bot.guit[pos].playing = false;
                    await ctx.RespondAsync("skipped");
                }
                else
                {
                    if (Bot.guit[pos].playnow.LavaTrack.IsStream)
                    {
                        Bot.guit[pos].playnow.sstop = true;
                    }
                    Bot.guit[pos].LLGuild.Stop();
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
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                Bot.guit[pos].LLGuild.Pause();
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
                if (!Bot.guit[pos].LLGuild.IsConnected)
                {
                    return;
                }
                Bot.guit[pos].LLGuild.Resume();
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
                if (!Bot.guit[pos].LLGuild.IsConnected || Bot.guit[pos].queue.Count == 1)
                {
                    return;
                }
                int pos2 = ctx.Member.Roles.ToList().FindIndex(x => x.CheckPermission(DSharpPlus.Permissions.ManageMessages) == DSharpPlus.PermissionLevel.Allowed);
                int pos3 = ctx.Member.Roles.ToList().FindIndex(x => x.CheckPermission(DSharpPlus.Permissions.Administrator) == DSharpPlus.PermissionLevel.Allowed);
                if (ctx.Member == Bot.guit[pos].queue[r].requester || pos2 != -1 || pos3 != -1)
                {
                    await ctx.RespondAsync($"Removed: **{Bot.guit[pos].queue[r].LavaTrack.Title}** by **{Bot.guit[pos].queue[r].LavaTrack.Author}**");
                    Bot.guit[pos].queue.RemoveAt(r);
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
                if (!Bot.guit[pos].LLGuild.IsConnected || Bot.guit[pos].queue.Count == 1)
                {
                    return;
                }
                if (end == 0)
                {
                    end = Bot.guit[pos].queue.Count - beg - 1;
                }
                await ctx.RespondAsync($"Removed: **{end + 1 - beg} Songs**");
                Bot.guit[pos].queue.RemoveRange(beg, end);
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
            var intbi = ctx.Client.GetInteractivity();
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            var chn = ctx.Member?.VoiceState?.Channel;
            if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
            {
                return;
            }
            //int i = 0;
            int pagamount = Bot.guit[pos].queue.Count / 5;
            int pagrest = Bot.guit[pos].queue.Count % 5;
            List<Page> dapa = new List<Page>();
            int ushere2 = 0;
            if (pagamount != 0)
            {
                int pagup = 0;
                int ushere = 0;
                bool another = false;
                while (ushere != pagamount)
                {
                    if (ushere == 1)
                    {
                        pagup = 5;
                    }
                    else if (ushere != 0)
                    {
                        if (ushere % 2 == 1)
                        {
                            pagup = Convert.ToInt32(Convert.ToString(ushere2) + Convert.ToString(5));
                        }
                        else
                        {
                            pagup = Convert.ToInt32(Convert.ToString(ushere2) + Convert.ToString(0));
                        }
                    }
                    var eb = new DiscordEmbedBuilder
                    {
                        Color = new DiscordColor("#68D3D2"),
                        Title = "Current Queue",
                        Description = "more",
                        ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                    };
                    for (int norpag = 0; norpag < 5; norpag++)
                    {
                        if (pagup == 0)
                        {
                            var que = Bot.guit[pos].queue;
                            eb.WithDescription("**__Now Playing:__**");
                            eb.AddField($"{que[pagup].LavaTrack.Title} ({que[pagup].LavaTrack.Length})", $"By **{que[pagup].LavaTrack.Author}** [Link]({que[pagup].LavaTrack.Uri}) **||** Requested by {que[pagup].requester.Mention}\n-----");
                        }
                        else
                        {
                            var que = Bot.guit[pos].queue;
                            eb.AddField($"{pagup}.{que[pagup].LavaTrack.Title} ({que[pagup].LavaTrack.Length})", $"By **{que[pagup].LavaTrack.Author}** [Link]({que[pagup].LavaTrack.Uri}) **||** Requested by {que[pagup].requester.Mention}\nᵕ");
                        }
                        pagup++;
                    }
                    dapa.Add(new Page
                    {
                        Embed = eb.Build()
                    });
                    ushere++;
                    if (another)
                    {
                        ushere2++;
                        another = false;
                    }
                    else
                    {
                        another = true;
                    }
                }
            }
            if (pagrest != 0)
            {
                int resup = 0;
                if (pagamount == 1)
                {
                    resup = 5;
                }
                else if (pagamount != 0)
                {
                    if (pagamount % 2 == 0)
                    {
                        resup = Convert.ToInt32(Convert.ToString(ushere2) + Convert.ToString(0));
                    }
                    else
                    {
                        resup = Convert.ToInt32(Convert.ToString(ushere2) + Convert.ToString(5));
                    }
                }
                var eb = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor("#68D3D2"),
                    Title = "Current Queue",
                    Description = "more",
                    ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                };
                for (int norpag = 0; norpag < pagrest; norpag++)
                {
                    if (resup == 0)
                    {
                        var que = Bot.guit[pos].queue;
                        eb.WithDescription("**__Now Playing:__**");
                        eb.AddField($"{que[resup].LavaTrack.Title} ({que[resup].LavaTrack.Length})", $"By **{que[resup].LavaTrack.Author}** [Link]({que[resup].LavaTrack.Uri}) **||** Requested by {que[resup].requester.Mention}\n**-----**");
                    }
                    else
                    {
                        var que = Bot.guit[pos].queue;
                        eb.AddField($"{resup}.{que[resup].LavaTrack.Title} ({que[resup].LavaTrack.Length})", $"By **{que[resup].LavaTrack.Author}** [Link]({que[resup].LavaTrack.Uri}) **||** Requested by {que[resup].requester.Mention}\nᵕ");
                    }
                    resup++;
                }
                dapa.Add(new Page {
                    Embed = eb.Build()
                });
            }
            if (dapa.Count > 1)
            {
                await intbi.SendPaginatedMessage(ctx.Channel, ctx.Member, dapa, timeoutoverride: TimeSpan.FromMinutes(2));
            }
            else if (dapa.Count == 0 || dapa == null)
            {
                await ctx.RespondAsync("Queue is empty uwu");
            }
            else
            {
                await ctx.RespondAsync(embed: dapa[0].Embed);
            }
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
            Bot.guit[pos].repeat = !Bot.guit[pos].repeat;
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
            Bot.guit[pos].repeatAll = !Bot.guit[pos].repeatAll;
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
            Bot.guit[pos].shuffle = !Bot.guit[pos].shuffle;
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
            Bot.guit[pos].queue.RemoveRange(0, Bot.guit[pos].queue.Count - 1);
            await ctx.RespondAsync("Cleared Queue");
        }

        [Command("playlist"), Aliases("pp")]
        public async Task QueP(CommandContext ctx, [RemainingText] string uri)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            var chn = ctx.Member?.VoiceState?.Channel;
            if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
            {
                return;
            }
            if (!uri.StartsWith("https://") && !uri.StartsWith("http://"))
            {
                await ctx.RespondAsync("no valid playlist link");
                return;
            }
            var con = Bot.guit[0].LLinkCon;
            var datrack = await con.GetTracksAsync(new Uri(uri));
            foreach (var dracks in datrack)
            {
                Bot.guit[pos].queue.Add(new Gsets2
                {
                    LavaTrack = dracks,
                    requester = ctx.Member
                });
            }
            await ctx.RespondAsync($"Added {datrack.Count()} songs to queue ({Bot.guit[pos].queue.Count} in queue now)");
        }

        [Command("nowplaying"), Aliases("np")]
        public async Task NowPl(CommandContext ctx)
        {
            var pos = Bot.guit.FindIndex(x => x.GID == ctx.Guild.Id);
            var chn = ctx.Member?.VoiceState?.Channel;
            if (pos == -1 || Bot.guit[pos].LLGuild.Channel != chn)
            {
                return;
            }
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
                    searchListRequest.Q = que.LavaTrack.Title; // Replace with your search term.
                    searchListRequest.MaxResults = 1;
                    searchListRequest.Type = "video";

                    // Call the search.list method to retrieve results matching the specified query term.
                    var searchListResponse = await searchListRequest.ExecuteAsync();
                    eb.AddField($"{que.LavaTrack.Title} ({que.LavaTrack.Length})", $"[Video Link]({que.LavaTrack.Uri})\n" +
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
                eb.AddField($"{que.LavaTrack.Title} ({que.LavaTrack.Length})", $"By {que.LavaTrack.Author}\n[Link]({que.LavaTrack.Uri})\nRequested by {que.requester.Mention}");
            }
            await ctx.RespondAsync(embed: eb.Build());
        }
    }
}
