using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink.EventArgs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace someBot.Commands.MusicEx
{
    public class LLEvents
    {
        public async Task PlayFin(TrackFinishEventArgs lg)
        {
            //Console.WriteLine("End Event");
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == lg.Player.Guild.Id);
            if (pos == -1 || !con.IsConnected || con == null) { await Task.CompletedTask; return; }
            if (lg.Track.IsStream && lg.Reason != TrackEndReason.Stopped)
            {
                Bot.guit[pos].sstop = false;
                Bot.guit[pos].LLGuild.Play(Bot.guit[pos].playnow.LavaTrack);
                Console.WriteLine("LL Stream Error");
                await Task.CompletedTask;
                return;
            }
            if (!Bot.guit[pos].repeat && !Bot.guit[pos].repeatAll && Bot.guit[pos].LLGuild != null && !Bot.guit[pos].sstop)
            {
                Bot.guit[pos].queue.Remove(Bot.guit[pos].queue.Find(x => x.addtime == Bot.guit[pos].playnow.addtime));
            }
            if (Bot.guit[pos].sstop)
            {
                Bot.guit[pos].sstop = false;
                Bot.guit[pos].playing = false;
            }
            else if (Bot.guit[pos].queue.Count != 0)
            {
                Bot.guit[pos].paused = false;
                await setPlay(pos);
                int nextSong = 0;
                System.Random rnd = new System.Random();
                if (Bot.guit[pos].shuffle) nextSong = rnd.Next(0, Bot.guit[pos].queue.Count);
                if (Bot.guit[pos].repeatAll) { Bot.guit[pos].rAint++; nextSong = Bot.guit[pos].rAint;
                    if (Bot.guit[pos].rAint == Bot.guit[pos].queue.Count) { Bot.guit[pos].rAint = 0; nextSong = 0; }
                }
                await setNP(pos, Bot.guit[pos].queue[nextSong]);
                Console.WriteLine($"[{lg.Player.Guild.Id}] Playing {Bot.guit[pos].playnow.LavaTrack.Title} by {Bot.guit[pos].playnow.LavaTrack.Author}");
                Bot.guit[pos].LLGuild.Play(Bot.guit[pos].playnow.LavaTrack);
            }
            else
            {
                Bot.guit[pos].paused = false;
                Bot.guit[pos].playing = false;
            }
            await Task.CompletedTask;
        }

        public async Task PlayStu(TrackStuckEventArgs ts)
        {
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == ts.Player.Guild.Id);
            if (pos == -1)
            {
                await Task.CompletedTask;
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream && !Bot.guit[pos].playnow.LavaTrack.IsStream)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester,
                    addtime = Bot.guit[pos].playnow.addtime
                });
            }
            await Task.CompletedTask;
        }

        public async Task PlayErr(TrackExceptionEventArgs ts)
        {
            var con = Bot.guit[0].LLinkCon;
            var pos = Bot.guit.FindIndex(x => x.GID == ts.Player.Guild.Id);
            if (pos == -1)
            {
                await Task.CompletedTask;
                return;
            }
            if (Bot.guit[pos].playnow.LavaTrack.IsStream && !Bot.guit[pos].playnow.LavaTrack.IsStream)
            {
                var naet = await con.GetTracksAsync(new Uri(Bot.guit[pos].playnow.LavaTrack.Uri.OriginalString));
                Bot.guit[pos].queue.Insert(0, new Gsets2
                {
                    LavaTrack = Bot.guit[pos].playnow.LavaTrack,
                    requester = Bot.guit[pos].playnow.requester,
                    addtime = Bot.guit[pos].playnow.addtime
                });
            }
            await Task.CompletedTask;
        }

        public Task setPlay(int pos)
        {
            Bot.guit[pos].playing = true;
            return Task.CompletedTask;
        }

        public Task setNP(int pos, Gsets2 queue)
        {
            Bot.guit[pos].playnow.LavaTrack = queue.LavaTrack;
            Bot.guit[pos].playnow.requester = queue.requester;
            Bot.guit[pos].playnow.addtime = queue.addtime;
            return Task.CompletedTask;
        }
    }
}
