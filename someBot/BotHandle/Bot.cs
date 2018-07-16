using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Threading;

namespace someBot
{
    public class Bot : IDisposable
    {
        private DiscordClient bot;
        private InteractivityModule interactivity;
        private CommandsNextModule commands;
        private CancellationTokenSource _cts;

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
                EnableCompression = true,
                LogLevel = LogLevel.Debug,
                Token = "",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true
            });
            interactivity = bot.UseInteractivity(new InteractivityConfiguration
            {
                //PaginationBehaviour = Bot_Inend()
            }); //add the ineractivity stuff to it

            _cts = new CancellationTokenSource();

            commands = bot.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefix = "m!",
                EnableDefaultHelp = false,
                IgnoreExtraArguments = false
            });

            commands.RegisterCommands<Random>(); //registers all the coammds from the different classes
            commands.RegisterCommands<YouTube>();
            commands.RegisterCommands<Wiki>();
            commands.RegisterCommands<Help>();
            commands.RegisterCommands<VUTDB>();
            commands.RegisterCommands<RanPics>();
            commands.RegisterCommands<XedddSpec>();
            commands.RegisterCommands<VoWiki>();
            commands.RegisterCommands<TestStuff>();

            commands.CommandErrored += Bot_CMDErr;

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
            bot.MessageReactionAdded += e =>
            {
                try { return Task.CompletedTask; }
                catch { return Task.CompletedTask; }
            };
            bot.Ready += e =>
            {
                DiscordGame test = new DiscordGame
                {
                    Name = "m!help or m!help-dev for commands uwu",
                    Details = "useless",
                    State = "dippin nachos"
                };
                bot.UpdateStatusAsync(game: test);
                return Task.CompletedTask;
            };
        }

        public async Task RunAsync()
        {
            await bot.ConnectAsync();
            await WaitForCancellationAsync();
        }

        private async Task WaitForCancellationAsync()
        {
            while(!_cts.IsCancellationRequested)
                await Task.Delay(500);
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

        public Task Bot_MessageCreated(MessageCreateEventArgs e)
        {
                if (e.Message.Content.ToLower().StartsWith("speyd3r is retarded")) e.Message.RespondAsync("no u");
                //e.Message.RespondAsync(e.Message.Channel.Id.ToString() + " " + e.Message.Id);

                if (e.Message.Channel.Type.ToString() == "Private") //DM Messages
                {
                    if (e.Message.Author.IsBot == false)
                    {
                        e.Message.RespondAsync("no u");
                    }
                    return Task.CompletedTask;
                }

                if (e.Message.Content.ToLower().StartsWith("uwu") && !(e.Message.Author.Id == 412572113191305226)) //the uwu react
                {
                    e.Message.CreateReactionAsync(DSharpPlus.Entities.DiscordEmoji.FromUnicode("🇴")); //this its not a normal o, look up the twitemoji preview page to get the emojis
                    Thread.Sleep(500);
                    e.Message.CreateReactionAsync(DSharpPlus.Entities.DiscordEmoji.FromUnicode("🇼"));
                    Thread.Sleep(500);
                    e.Message.CreateReactionAsync(DSharpPlus.Entities.DiscordGuildEmoji.FromGuildEmote(bot, 455504120825249802)); // lengthy isnt it?
                }

                if (e.Message.Content.ToLower().StartsWith("uwu") && e.Message.Author.Id == 412572113191305226) //just to mess with monike from blodrodsgorls server hehe
                {
                    e.Message.CreateReactionAsync(DSharpPlus.Entities.DiscordEmoji.FromUnicode("🇴"));
                    Thread.Sleep(500);
                    e.Message.CreateReactionAsync(DSharpPlus.Entities.DiscordEmoji.FromUnicode("🇲"));
                    Thread.Sleep(500);
                    e.Message.CreateReactionAsync(DSharpPlus.Entities.DiscordGuildEmoji.FromGuildEmote(bot, 455504120825249802));
                }
            return Task.CompletedTask;
        }

        private Task Bot_CMDErr(CommandErrorEventArgs e) //if bot error
        {
            //e.Context.RespondAsync($"error + {e.StackTrace}");
            return Task.CompletedTask;
        }

        private Task Bot_ClientErrored(ClientErrorEventArgs e) //if bot error
        {
            return Task.CompletedTask;
        }

        private Task Bot_Inend(TimeoutException e) //if bot error
        {
            return Task.CompletedTask;
        }

        public Task Bot_MessageEdited(MessageUpdateEventArgs e)
        {
            if (e.Message.Channel.Type.ToString() == "Private" && e.Message.Author.IsBot == false) //responds with no u whena message is edited in dms NEVER FORGET THE "isBot" or it will dm u infinitly
            {
                e.Message.RespondAsync("no u");
            }
            return Task.CompletedTask;
        }

        public Task itError(CommandErrorEventArgs oof)
        {
            //DSharpPlus.CommandsNext.CommandEventArgs.Command.get
            //oof.Context.RespondAsync(oof.Exception.HResult.ToString());
            if (oof.Exception.HResult != -2146233088)
            {
                oof.Context.RespondAsync(oof.Command.Description); //as i explained above
            }
            return Task.CompletedTask;
        }
    }
}
