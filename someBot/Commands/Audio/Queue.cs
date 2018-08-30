using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace someBot.Commands.Audio
{
    public class Queue
    {
        public async Task QueueList(int pos,CommandContext ctx)
        {
            try
            {
                var intbi = ctx.Client.GetInteractivity();
                var chn = ctx.Member?.VoiceState?.Channel;
                if (Bot.guit[pos].LLGuild.Channel != chn)
                {
                    await Task.CompletedTask;
                    return;
                }
                Bot.guit[pos].cmdChannel = ctx.Channel.Id;
                if (Bot.guit[pos].queue.Count == 0 && !Bot.guit[pos].playing)
                {
                    await ctx.RespondAsync("Queue is empty uwu");
                    return;
                }
                else if (Bot.guit[pos].queue.Count == 0 && Bot.guit[pos].playing)
                {
                    var eb = new DiscordEmbedBuilder
                    {
                        Color = new DiscordColor("#68D3D2"),
                        Title = "Current Queue",
                        Description = "more",
                        ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl
                    };
                    var que = Bot.guit[pos].queue;
                    eb.WithDescription("**__Now Playing:__**");
                    string time1 = "";
                    string time2 = "";
                    if (Bot.guit[pos].playnow.LavaTrack.Length.Hours < 1)
                    {
                        time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                        time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"mm\:ss");
                    }
                    else
                    {
                        time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                        time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"hh\:mm\:ss");
                    }
                    eb.AddField($"{Bot.guit[pos].playnow.LavaTrack.Title} ({time1}/{time2})", $"By **{Bot.guit[pos].playnow.LavaTrack.Author}** [Link]({Bot.guit[pos].playnow.LavaTrack.Uri}) **||** Requested by {Bot.guit[pos].playnow.requester.Mention}\n-----");
                    await ctx.RespondAsync(embed: eb.Build());
                    return;
                }
                int pagamount = Bot.guit[pos].queue.Count / 5;
                int pagrest = Bot.guit[pos].queue.Count % 5;
                bool stuff = false;
                int offset = 0;
                if (Bot.guit[pos].playnow.requester != null || Bot.guit[pos].queue[0].addtime != Bot.guit[pos].playnow.addtime)
                {
                    if (Bot.guit[pos].queue[0].addtime != Bot.guit[pos].playnow.addtime)
                    {
                        if (pagrest == 4)
                        {
                            pagrest = 1;
                            pagamount++;
                        }
                        else
                        {
                            pagrest++;
                        }
                        stuff = true;
                    }
                }
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
                            if (pagup == 0 && Bot.guit[pos].playnow.requester != null)
                            {
                                var que = Bot.guit[pos].queue;
                                eb.WithDescription("**__Now Playing:__**");
                                string time1 = "";
                                string time2 = "";
                                if (Bot.guit[pos].playnow.LavaTrack.Length.Hours < 1)
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                                    time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"mm\:ss");
                                }
                                else
                                {
                                    time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                                    time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"hh\:mm\:ss");
                                }
                                eb.AddField($"{Bot.guit[pos].playnow.LavaTrack.Title} ({time1}/{time2})", $"By **{Bot.guit[pos].playnow.LavaTrack.Author}** [Link]({Bot.guit[pos].playnow.LavaTrack.Uri}) **||** Requested by {Bot.guit[pos].playnow.requester.Mention}\n-----");
                                if (stuff)
                                {
                                    offset = 1;
                                }
                            }
                            else
                            {
                                var que = Bot.guit[pos].queue;
                                string time2 = "";

                                if (que[pagup - offset].LavaTrack.Length.Hours < 1)
                                {
                                    time2 = que[pagup - offset].LavaTrack.Length.ToString(@"mm\:ss");
                                }
                                else
                                {
                                    time2 = que[pagup - offset].LavaTrack.Length.ToString(@"hh\:mm\:ss");
                                }
                                eb.AddField($"{pagup - offset}.{que[pagup - offset].LavaTrack.Title} ({time2})", $"By **{que[pagup - offset].LavaTrack.Author}** [Link]({que[pagup - offset].LavaTrack.Uri}) **||** Requested by {que[pagup - offset].requester.Mention}\nᵕ");
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
                        if (resup == 0 && pagamount == 0 && Bot.guit[pos].playnow.requester != null)
                        {
                            var que = Bot.guit[pos].queue;
                            eb.WithDescription("**__Now Playing:__**");
                            string time1 = "";
                            string time2 = "";
                            if (Bot.guit[pos].playnow.LavaTrack.Length.Hours < 1)
                            {
                                time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"mm\:ss");
                                time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"mm\:ss");
                            }
                            else
                            {
                                time1 = Bot.guit[pos].LLGuild.CurrentState.PlaybackPosition.ToString(@"hh\:mm\:ss");
                                time2 = Bot.guit[pos].playnow.LavaTrack.Length.ToString(@"hh\:mm\:ss");
                            }
                            eb.AddField($"{Bot.guit[pos].playnow.LavaTrack.Title} ({time1}/{time2})", $"By **{Bot.guit[pos].playnow.LavaTrack.Author}** [Link]({Bot.guit[pos].playnow.LavaTrack.Uri}) **||** Requested by {Bot.guit[pos].playnow.requester.Mention}\n**-----**");
                            if (stuff)
                            {
                                offset = 1;
                            }
                        }
                        else
                        {
                            var que = Bot.guit[pos].queue;
                            string time2 = "";
                            if (que[resup - offset].LavaTrack.Length.Hours < 1)
                            {
                                time2 = que[resup - offset].LavaTrack.Length.ToString(@"mm\:ss");
                            }
                            else
                            {
                                time2 = que[resup - offset].LavaTrack.Length.ToString(@"hh\:mm\:ss");
                            }
                            eb.AddField($"{resup - offset}.{que[resup - offset].LavaTrack.Title} ({time2})", $"By **{que[resup - offset].LavaTrack.Author}** [Link]({que[resup - offset].LavaTrack.Uri}) **||** Requested by {que[resup - offset].requester.Mention}\nᵕ");
                        }
                        resup++;
                    }
                    dapa.Add(new Page
                    {
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            await Task.CompletedTask;
        }

        public Task queueRemove(int pos, int num)
        {
            Bot.guit[pos].queue.RemoveAt(num);
            return Task.CompletedTask;
        }

        public Task queueRemoveSome(int pos, int num, int maxVal)
        {
            Bot.guit[pos].queue.RemoveRange(num, maxVal);
            return Task.CompletedTask;
        }
    }
}
