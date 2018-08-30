using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using DSharpPlus.VoiceNext;
using DSharpPlus.Lavalink;
using System;
using System.Linq;
using NYoutubeDL;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Threading;
using DSharpPlus.Net.Udp;
using DiscordBotsList.Api;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace someBot
{
    public class Bot : IDisposable
    {
        private DiscordClient bot;
        private InteractivityExtension interactivity;
        private CommandsNextExtension commands;
        static VoiceNextExtension voice;
        public LavalinkExtension llink { get; }
        public LavalinkNodeConnection LavalinkNode { get; private set; }
        private CancellationTokenSource _cts;
        public static LavalinkConfiguration lcfg = new LavalinkConfiguration
        {
            Password = "",
            SocketEndpoint = new ConnectionEndpoint { Hostname = "localhost", Port = 8089 },
            RestEndpoint = new ConnectionEndpoint { Hostname = "localhost", Port = 2335 }
        };
        public static List<Gsets> guit = new List<Gsets>();

        public Bot()
        {
            Panda PandaClass = new Panda(this); //attaches all exta classes with their specific methos to the bot task (and the "this" is to allow usage of form things in there) ducumentes in Xeddd.cs
            Xeddd XedddClass = new Xeddd(this);
            My MyClass = new My(this);

            PandaClass.pUp();
            XedddClass.xedddUp();
            MyClass.myUp();

            bot = new DiscordClient(new DiscordConfiguration()
            {
                AutoReconnect = true,
                //EnableCompression = true,
                LogLevel = LogLevel.Debug,
                Token = "",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true
            });
            interactivity = bot.UseInteractivity(new InteractivityConfiguration { }); //add the ineractivity stuff to it

            _cts = new CancellationTokenSource();

            commands = bot.UseCommandsNext(new CommandsNextConfiguration()
            {
                //StringPrefixes = (new[] { "m!" }),
                EnableDefaultHelp = false,
                IgnoreExtraArguments = false,
                CaseSensitive = false,
                PrefixResolver = PreGet
            });

            commands.RegisterCommands<Random>(); //registers all the coammds from the different classes
            commands.RegisterCommands<YouTube>();
            commands.RegisterCommands<Wiki>();
            commands.RegisterCommands<Help>();
            commands.RegisterCommands<VUTDB>();
            commands.RegisterCommands<YTDLC>();
            commands.RegisterCommands<Commands.RanImg.Derpy>();
            commands.RegisterCommands<Commands.RanImg.MeekMoe>();
            commands.RegisterCommands<Commands.RanImg.Nadeko>();
            commands.RegisterCommands<Commands.RanImg.NekosLife>();
            commands.RegisterCommands<Commands.RanImg.Other>();
            commands.RegisterCommands<XedddSpec>();
            commands.RegisterCommands<VoWiki>();
            commands.RegisterCommands<Commands.Voice>();

            commands.CommandErrored += Bot_CMDErr;

            voice = bot.UseVoiceNext();
            llink = bot.UseLavalink();

            llink.NodeDisconnected += async le =>
            {
                while(!le.LavalinkNode.IsConnected)
                {
                    await Task.Delay(5000);
                    await llink.ConnectAsync(lcfg);
                }
            };

            bot.Ready += OnReadyAsync;
            bot.MessageCreated += this.Bot_MessageCreated;
            bot.MessageCreated += PandaClass.Bot_PandaMessageCreated;
            bot.MessageCreated += XedddClass.Bot_XedddMessageCreated;
            bot.MessageCreated += MyClass.Bot_MyMessageCreated;
            bot.MessageCreated += XedddClass.Bot_Dump;
            bot.MessageUpdated += this.Bot_MessageEdited;
            bot.MessageUpdated += PandaClass.Bot_PandaMessageEdited;
            bot.MessageUpdated += XedddClass.Bot_XedddMessageEdited;
            bot.MessageUpdated += MyClass.Bot_MyMessageEdited;
            bot.GuildMemberRemoved += XedddClass.Bot_XedddBoiLeave;
            bot.MessageReactionAdded += PandaClass.Bot_PandaGumiQuotes;
            bot.ClientErrored += this.Bot_ClientErrored;
            bot.VoiceStateUpdated += async e => {
                try
                {
                    var pos = guit.FindIndex(x => x.GID == e.Guild.Id);
                    if (pos != -1)
                    {
                        await Task.Delay(500);
                        if (guit[pos].LLGuild.Channel.Id == e.Before.Channel.Id)
                        {
                            if (guit[pos].LLGuild.Channel.Users.Where(x => x.IsBot == false).Count() == 0)
                            {
                                guit[pos].alone = true;
                            }
                            else
                            {
                                guit[pos].alone = false;
                            }
                            if (guit[pos].LLGuild.Channel.Users.Where(x => x.IsBot == false).Count() == 0 && guit[pos].queue.Count > 0 && guit[pos].LLGuild.Channel.Id == e.Before.Channel.Id && !guit[pos].paused)
                            {
                                await e.Guild.GetChannel(guit[pos].cmdChannel).SendMessageAsync("Playback was paused since everybody left the voicechannel, use ``m!resume`` to unpause");
                                guit[pos].LLGuild.Pause();
                                guit[pos].paused = true;
                            }
                            handleVoidisc(pos);
                        }
                    }
                }
                catch
                {
                    try
                    {
                        var pos = guit.FindIndex(x => x.GID == e.Guild.Id);
                        if (pos != -1)
                        {
                            handleVoidisc(pos);
                        }
                    }
                    catch{}
                }
                await Task.CompletedTask;
            };
            //bot.VoiceStateUpdated += TimoutODisc;
            bot.GuildMemberAdded += async e =>
            {
                if (e.Guild.Id == 483279257431441410)
                {
                    await e.Member.GrantRoleAsync(e.Guild.GetRole(483280207927574528));
                }
                await Task.CompletedTask;
            };
            bot.Heartbeated += async e =>
            {
                AuthDiscordBotListApi DblApi = new AuthDiscordBotListApi(465675368775417856, "");
                var me = await DblApi.GetMeAsync();
                await me.UpdateStatsAsync(e.Client.Guilds.Count);
            };
            bot.GuildDownloadCompleted += async e =>
            {
                DiscordActivity test = new DiscordActivity
                {
                    Name = "m!help or m!help-dev for commands uwu",
                    ActivityType = ActivityType.Playing
                };
                await bot.UpdateStatusAsync(activity: test, userStatus: UserStatus.Online);
                await Task.Delay(500);
                try
                {
                    guit.Add(new Gsets
                    {
                        GID = 0,
                        prefix = new List<string>(new string[] { "m!" }),
                        queue = new List<Gsets2>(),
                        playnow = new Gsets3(),
                        repeat = false,
                        offtime = DateTime.Now,
                        timeout = false,
                        shuffle = false,
                        LLinkCon = await llink.ConnectAsync(lcfg),
                        LLGuild = null,
                        playing = false,
                        rAint = 0,
                        repeatAll = false,
                        alone = false,
                        paused = true,
                        stoppin = false
                    });
                    foreach (var guilds in e.Client.Guilds)
                    {
                        guit.Add(new Gsets {
                            GID = guilds.Value.Id,
                            prefix = new List<string>(new string[] { "m!" }),
                            queue = new List<Gsets2>(),
                            playnow = new Gsets3(),
                            repeat = false,
                            offtime = DateTime.Now,
                            timeout = false,
                            shuffle = false,
                            LLGuild = null,
                            playing = false,
                            rAint = 0,
                            repeatAll = false,
                            alone = false,
                            paused = true,
                            audioPlay = new Commands.Audio.Playback(),
                            audioFunc = new Commands.Audio.Functions(),
                            audioQueue = new Commands.Audio.Queue(),
                            audioEvents = new Commands.Audio.Events(),
                            stoppin = false
                        });
                    }
                    await Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            };
            bot.GuildCreated += async e => {
                var pos = guit.FindIndex(x => x.GID == e.Guild.Id);
                if (pos == -1)
                {
                    guit.Add(new Gsets
                    {
                        GID = e.Guild.Id,
                        prefix = new List<string>(new string[] { "m!" }),
                        queue = new List<Gsets2>(),
                        playnow = new Gsets3(),
                        repeat = false,
                        offtime = DateTime.Now,
                        timeout = false,
                        shuffle = false,
                        LLGuild = null,
                        playing = false,
                        rAint = 0,
                        repeatAll = false,
                        alone = false,
                        paused = true,
                        audioPlay = new Commands.Audio.Playback(),
                        audioFunc = new Commands.Audio.Functions(),
                        audioQueue = new Commands.Audio.Queue(),
                        audioEvents = new Commands.Audio.Events(),
                        stoppin = false
                    });
                }
                await Task.CompletedTask;
            };
        }

        public async Task RunAsync()
        {
            await bot.ConnectAsync();
            await WaitForCancellationAsync();
        }

        private async Task WaitForCancellationAsync()
        {
            while (!_cts.IsCancellationRequested)
                await Task.Delay(500);
        }

        private Task<int> PreGet(DiscordMessage msg)
        {
            int pos = guit.FindIndex(x => x.GID == msg.Channel.GuildId);
            var wtf = msg.Content;
            if (pos != -1)
            {
                var multiprefloc = guit[pos].prefix.FindIndex(x => wtf.StartsWith(x));
                int prefloc;
                if (multiprefloc != -1)
                {
                    prefloc = msg.GetStringPrefixLength(guit[pos].prefix[multiprefloc]);

                    if (prefloc != -1)
                    {
                        return Task.FromResult(prefloc);
                    }
                }
            }
            return Task.FromResult(msg.GetStringPrefixLength(guit[0].prefix[0]));
        }

        private async Task OnReadyAsync(ReadyEventArgs e)
        {
            await Task.Yield();
        }

        public void Dispose()
        {
            this.bot.Dispose();
            this.interactivity = null;
            this.commands = null;
        }

        internal void WriteCenter(string value, int skipline = 0)
        {
            for (int i = 0; i < skipline; i++)
                Console.WriteLine();

            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            Console.WriteLine(value);
        }

        public int emojiCount(ulong channelId, ulong messageId, ulong emojiId) //for the gumiquotes in Panda.cs
        {
            var countnum = Convert.ToInt32(bot.GetChannelAsync(channelId).Result.GetMessageAsync(messageId).Result.GetReactionsAsync(DSharpPlus.Entities.DiscordEmoji.FromGuildEmote(bot, emojiId)).Result.Count);
            return countnum;
        }

        public void sendToChannel(ulong channelId, string msg) //if a message needs to be sent to another channel, in commands this is not needed
        {
            bot.GetChannelAsync(channelId).Result.SendMessageAsync(msg);
        }

        public async void handleVoidisc(int pos) //if a message needs to be sent to another channel, in commands this is not needed
        {
            try
            {
                guit[pos].offtime = DateTime.Now;
                while (guit[pos].alone || guit[pos].queue.Count < 1)
                {
                    if (DateTime.Now.Subtract(guit[pos].offtime).TotalMinutes > 5)
                    {
                        guit[pos].LLGuild.PlaybackFinished -= guit[pos].audioEvents.PlayFin;
                        guit[pos].LLGuild.TrackStuck -= guit[pos].audioEvents.PlayStu;
                        guit[pos].LLGuild.TrackException -= guit[pos].audioEvents.PlayErr;
                        guit[pos].LLGuild.Disconnect();
                        guit[pos].LLGuild = null;
                        guit[pos].offtime = DateTime.Now;
                        guit[pos].paused = false;
                        break;
                    }
                    else
                    {
                        await Task.Delay(10000);
                    }

                }
            }
            catch{ }
        }

        public async Task Bot_MessageCreated(MessageCreateEventArgs e)
        {
            try
            {
                if (e.Message.Content.ToLower().StartsWith("speyd3r is retarded")) await e.Message.RespondAsync("no u");
                //e.Message.RespondAsync(e.Message.Channel.Id.ToString() + " " + e.Message.Id);

                if (e.Message.Channel.Type.ToString() == "Private") //DM Messages
                {
                    if (e.Message.Author.IsBot == false)
                    {
                        await e.Message.RespondAsync("no u");
                    }
                }

                if (e.Message.Content.ToLower().StartsWith("uwu") && !(e.Message.Author.Id == 412572113191305226)) //the uwu react
                {
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("🇴"));
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("🇼"));
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromGuildEmote(bot, 455504120825249802)); // lengthy isnt it?
                }

                if (e.Message.Content.ToLower().StartsWith("uwu") && e.Message.Author.Id == 412572113191305226) //just to mess with monike from blodrodsgorls server hehe
                {
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("🇴"));
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromUnicode("🇲"));
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    await e.Message.CreateReactionAsync(DiscordEmoji.FromGuildEmote(bot, 455504120825249802));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                //return Task.CompletedTask;
            }
            //await Task.CompletedTask;
        }

        private async Task Bot_CMDErr(CommandErrorEventArgs e) //if bot error
        {
            if (e.Exception.Message.ToLower().Contains("countdown"))
            {
                var er = await e.Context.RespondAsync("Error, please wait 5 seconds before issuing that command again");
                await Task.Delay(2500);
                await er.DeleteAsync();
            }
            //e.Context.RespondAsync($"**Error:**\n```{e.Exception.Message}```");
            Console.WriteLine(e.Exception.Message);
            Console.WriteLine(e.Exception.StackTrace);
            await Task.CompletedTask;
        }

        private Task Bot_ClientErrored(ClientErrorEventArgs e) //if bot error
        {
                Console.WriteLine(e.Exception.Message);
                Console.WriteLine(e.Exception.StackTrace);
            return Task.CompletedTask;
        }

        private Task Bot_Inend(TimeoutException e) //if bot error
        {
            return Task.CompletedTask;
        }

        public Task Bot_MessageEdited(MessageUpdateEventArgs e)
        {
            return Task.CompletedTask;
        }

        public Task itError(CommandErrorEventArgs oof)
        {
            if (oof.Exception.HResult != -2146233088)
            {
                oof.Context.RespondAsync(oof.Command.Description); //as i explained above
            }
            return Task.CompletedTask;
        }
    }

    public class Gsets
    {
        [JsonProperty("GID")]
        public ulong GID { get; set; }
        [JsonProperty("LLinkCon")]
        public LavalinkNodeConnection LLinkCon { get; set; }
        [JsonProperty("LLGuild")]
        public LavalinkGuildConnection LLGuild { get; set; }
        [JsonProperty("prefix")]
        public List<string> prefix { get; set; }
        [JsonProperty("queue")]
        public List<Gsets2> queue { get; set; }
        [JsonProperty("playingnow")]
        public Gsets3 playnow { get; set; }
        [JsonProperty("offtime")]
        public DateTime offtime { get; set; }
        [JsonProperty("repeat")]
        public bool repeat { get; set; }
        [JsonProperty("repeatAll")]
        public bool repeatAll { get; set; }
        [JsonProperty("rtAll")]
        public int rAint { get; set; }
        [JsonProperty("shuffle")]
        public bool shuffle { get; set; }
        [JsonProperty("playing")]
        public bool playing { get; set; }
        [JsonProperty("timeout")]
        public bool timeout { get; set; }
        [JsonProperty("alone")]
        public bool alone { get; set; }
        [JsonProperty("paused")]
        public bool paused { get; set; }
        [JsonProperty("stoppin")]
        public bool stoppin { get; set; }
        [JsonProperty("cmdChannel")]
        public ulong cmdChannel { get; set; }
        [JsonProperty("audioPlay")]
        public Commands.Audio.Playback audioPlay { get; set; }
        [JsonProperty("audioFunc")]
        public Commands.Audio.Functions audioFunc { get; set; }
        [JsonProperty("audioQueue")]
        public Commands.Audio.Queue audioQueue { get; set; }
        [JsonProperty("audioEvents")]
        public Commands.Audio.Events audioEvents { get; set; }
    }

    public class Gsets2
    {
        [JsonProperty("requester")]
        public DiscordMember requester { get; set; }
        [JsonProperty("LavaTrack")]
        public LavalinkTrack LavaTrack { get; set; }
        [JsonProperty("addtime")]
        public DateTime addtime { get; set; }
    }

    public class Gsets3
    {
        [JsonProperty("requester")]
        public DiscordMember requester { get; set; }
        [JsonProperty("LavaTrack")]
        public LavalinkTrack LavaTrack { get; set; }
        [JsonProperty("sstop")]
        public bool sstop { get; set; }
        [JsonProperty("addtime")]
        public DateTime addtime { get; set; }
    }
}
