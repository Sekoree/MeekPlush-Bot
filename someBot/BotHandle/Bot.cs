using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
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
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;
using someBot.Commands.MusicEx;

namespace someBot
{
    public class Bot : IDisposable
    {
        private DiscordClient bot;
        private InteractivityExtension interactivity;
        private CommandsNextExtension commands;
        public LavalinkExtension llink { get; }
        public LavalinkNodeConnection LavalinkNode { get; private set; }
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

            commands = bot.UseCommandsNext(new CommandsNextConfiguration()
            {
                //StringPrefixes = new[] { "m!" },
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
            commands.RegisterCommands<Commands.Music>();

            commands.CommandErrored += Bot_CMDErr;

            llink = bot.UseLavalink();

            llink.NodeDisconnected += async le =>
            {
                while(!le.LavalinkNode.IsConnected)
                {
                    await Task.Delay(5000);
                    var ok = await llink.ConnectAsync(lcfg);
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
            bot.VoiceStateUpdated += async e =>
            {
                try
                {
                    var con = guit[0].LLinkCon;
                    var pos = guit.FindIndex(x => x.GID == e.Guild.Id);
                    if (pos == -1 || !con.IsConnected || con == null) { await Task.CompletedTask; return; }
                    var norm = e?.Channel?.Id;
                    var afte = e?.After?.Channel?.Id;
                    var befo = e?.Before?.Channel?.Id;
                    if (norm == guit[pos].LLGuild?.Channel?.Id || afte == guit[pos].LLGuild?.Channel?.Id || befo == guit[pos].LLGuild?.Channel?.Id)
                    {
                        if (guit[pos].LLGuild?.Channel?.Users.Where(x => !x.IsBot).Count() == 0)
                        {
                            guit[pos].paused = true;
                            await Task.Run(() => guit[pos].AudioFunctions.Pause(pos));
                            await e.Guild.GetChannel(guit[pos].cmdChannel).SendMessageAsync("Playback was paused since everybodey left the channel! uns ``m!resume`` to resume, otherwise I'll also disconnect in ~5min uwu");
                            var haDi = handleVoidisc(pos);
                            haDi.Wait(millisecondsTimeout: 2500);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                await Task.CompletedTask;
            };
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
                    Name = "m!help for commands uwu",
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
                        LLinkCon = await llink.ConnectAsync(lcfg)
                    });
                    foreach (var guilds in e.Client.Guilds)
                    {
                        guit.Add(new Gsets
                        {
                            GID = guilds.Value.Id,
                            prefix = new List<string>(new string[] { "m!" }),
                            queue = new List<Gsets2>(),
                            playnow = new Gsets3(),
                            repeat = false,
                            offtime = DateTime.Now,
                            shuffle = false,
                            LLGuild = null,
                            playing = false,
                            rAint = 0,
                            repeatAll = false,
                            AudioEvents = new LLEvents(),
                            AudioFunctions = new Functions(),
                            AudioQueue = new Queue(),
                            sstop = false,
                            paused = false
                        });
                    }
                    Console.WriteLine("GuildList done!");
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
                        shuffle = false,
                        LLGuild = null,
                        playing = false,
                        rAint = 0,
                        repeatAll = false,
                        AudioEvents = new LLEvents(),
                        AudioFunctions = new Functions(),
                        AudioQueue = new Queue(),
                        sstop = false,
                        paused = false
                    });
                }
                await Task.CompletedTask;
            };
        }

        public async Task RunAsync()
        {
            await bot.ConnectAsync();
            await Task.Delay(-1);
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

        public async Task handleVoidisc(int pos) //if a message needs to be sent to another channel, in commands this is not needed
        {
            try
            {
                guit[pos].offtime = DateTime.Now;
                await Task.CompletedTask;
                while (guit[pos].LLGuild.Channel.Users.Where(x => !x.IsBot).Count() == 0 || guit[pos].queue.Count < 1)
                {
                    //Console.WriteLine("Disc");
                    if (DateTime.Now.Subtract(guit[pos].offtime).TotalMinutes > 5)
                    {
                        guit[pos].LLGuild.PlaybackFinished -= guit[pos].AudioEvents.PlayFin;
                        guit[pos].LLGuild.TrackStuck -= guit[pos].AudioEvents.PlayStu;
                        guit[pos].LLGuild.TrackException -= guit[pos].AudioEvents.PlayErr;
                        guit[pos].LLGuild.Disconnect();
                        guit[pos].paused = false;
                        guit[pos].rAint = 0;
                        guit[pos].repeat = false;
                        guit[pos].repeatAll = false;
                        guit[pos].shuffle = false;
                        guit[pos].queue.Clear();
                        guit[pos].playnow = new Gsets3();
                        guit[pos].playing = false;
                        guit[pos].LLGuild = null;
                        guit[pos].offtime = DateTime.Now;
                        break;
                    }
                    else
                    {
                        await Task.Delay(10000);
                    }
                }
            }
            catch { }
            await Task.CompletedTask;
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

        private async Task Bot_CMDErr(CommandErrorEventArgs ex) //if bot error
        {
            if (ex.Exception is ChecksFailedException exx)
            {
                foreach (CheckBaseAttribute a in exx.FailedChecks)
                {
                    if (a is CooldownAttribute cd)
                    {
                        await ex.Context.RespondAsync($"Cooldown, {(int)cd.GetRemainingCooldown(ex.Context).TotalSeconds}s left");
                    }
                    if (a is RequireBotPermissionsAttribute bpa)
                    {
                        await ex.Context.Member.SendMessageAsync($"Heya, sorry for the DM but I need to be able to ``Send Messages`` and ``Embed Links`` in order to use the music functions!");
                    }
                }
            }
            //e.Context.RespondAsync($"**Error:**\n```{e.Exception.Message}```");
            Console.WriteLine(ex.Exception.Message);
            Console.WriteLine(ex.Exception.StackTrace);
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
        public ulong GID { get; set; }
        public LavalinkNodeConnection LLinkCon { get; set; }
        public LavalinkGuildConnection LLGuild { get; set; }
        public List<string> prefix { get; set; }
        public List<Gsets2> queue { get; set; }
        public Gsets3 playnow { get; set; }
        public DateTime offtime { get; set; }
        public bool repeat { get; set; }
        public bool repeatAll { get; set; }
        public int rAint { get; set; }
        public bool shuffle { get; set; }
        public bool playing { get; set; }
        public bool sstop { get; set; }
        public ulong cmdChannel { get; set; }
        public bool paused { get; set; }
        public LLEvents AudioEvents { get; set; }
        public Functions AudioFunctions { get; set; }
        public Queue AudioQueue { get; set; }
    }

    public class Gsets2
    {
        public DiscordMember requester { get; set; }
        public LavalinkTrack LavaTrack { get; set; }
        public DateTime addtime { get; set; }
    }

    public class Gsets3
    {
        public DiscordMember requester { get; set; }
        public LavalinkTrack LavaTrack { get; set; }
        public DateTime addtime { get; set; }
    }
}
