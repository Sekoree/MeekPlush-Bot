using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using System.IO;

namespace someBot
{
    class Panda
    {
        Bot y;
        public Panda(Bot form)
        {
            y = form;
        }

        bool bpanda = true;
        ulong last_boi = 0;

        int pCheck = 0;

        public void pUp()
        {
            if (File.ReadAllText(@"/dbots/someBot/logs/panda" + pCheck + ".txt").Length > 250000)
            {
                pCheck++;
                pUp();
            }
        }

        public void pMsgAddTxt(string addMsg)
        {
                if (File.ReadAllText(@"/dbots/someBot/logs/panda" + pCheck + ".txt").Length > 250000)
                {
                    pCheck++;
                    File.CreateText(@"/dbots/someBot/logs/panda" + pCheck + ".txt");
                    pMsgAddTxt(addMsg);
                }
                else
                {
                   File.AppendAllText(@"/dbots/someBot/logs/panda" + pCheck + ".txt", addMsg + Environment.NewLine);
                }
        }

        // called when the bot receives a message
        public Task Bot_PandaMessageCreated(MessageCreateEventArgs e)
        {
            if (!(e.Message.Channel.Type.ToString() == "Private"))
            {
                if (e.Guild.Id == 448241243496120321)
                {
                    string proxy_urls = "";
                    string attlist = "";
                    string BotCheck = "";

                    if (e.Message.Attachments != null)
                    {
                        foreach (var files in e.Message.Attachments)
                        {
                            proxy_urls += "\n  " + files.ProxyUrl;
                        }
                    }

                    if (!(proxy_urls == "")) attlist = "\n Attachments:" + proxy_urls;
                    if (e.Message.Author.IsBot) BotCheck = "[Bot] ";
                    if (e.Message.Author.Id == last_boi && !(proxy_urls == ""))
                    {
                        last_boi = e.Message.Author.Id;
                        pMsgAddTxt(proxy_urls);
                    }
                    else
                    {
                        last_boi = e.Message.Author.Id;
                        if (bpanda == true)
                        {
                            bpanda = false;
                        }
                        pMsgAddTxt("[" + e.Message.Channel.Name + "] " + BotCheck + e.Message.Author.Username + "(" + e.Guild.GetMemberAsync(e.Author.Id).Result.Nickname + "): " + e.Message.Content + attlist);
                    }
                }
            }
            //412572113191305226 monika d-id
            return Task.CompletedTask;
        }

        public Task Bot_PandaMessageEdited(MessageUpdateEventArgs e)
        {
            if (!(e.Message.Channel.Type.ToString() == "Private"))
            {
                if (e.Guild.Id == 448241243496120321 && !(e.Message.ChannelId == 451575473772953611))
                {
                    string proxy_urls = "";
                    string attlist = "";
                    string BotCheck = "";

                    if (e.Message.Attachments != null)
                    {
                        foreach (var files in e.Message.Attachments)
                        {
                            proxy_urls += "\n  " + files.ProxyUrl;
                        }
                    }

                    if (!(proxy_urls == "")) attlist = "\n Attachments:" + proxy_urls;
                    if (e.Message.Author.IsBot) BotCheck = "[Bot] ";
                    if (e.Message.Author.Id == last_boi && !(proxy_urls == ""))
                    {
                        last_boi = e.Message.Author.Id;
                        pMsgAddTxt(proxy_urls + " [Edited]");
                    }
                    else
                    {
                        last_boi = e.Message.Author.Id;
                        if (bpanda == true)
                        {
                            bpanda = false;
                        }
                        pMsgAddTxt("[" + e.Message.Channel.Name + "] " + BotCheck + e.Message.Author.Username + "(" + e.Guild.GetMemberAsync(e.Author.Id).Result.Nickname + "): " + e.Message.Content + attlist + " [Edited]");
                    }
                }
            }
            return Task.CompletedTask;
        }

        string[] lastStars = new string[100];
        int cUp = 0;

        public Task Bot_PandaGumiQuotes(MessageReactionAddEventArgs e)
        {
            //var gumi2 = DSharpPlus.Entities.DiscordEmoji.FromGuildEmote(Bot.Client, 450861876805632010);
            var gumi = e.Channel.Guild.GetEmojiAsync(450861876805632010).Result;
            var countnum = y.emojiCount(e.Channel.Id, e.Message.Id, 450861876805632010);
            //var countnum = Convert.ToInt32(e.Message.GetReactionsAsync(gumi).Result.Count);
            var msg = e.Message.Content;
            if (countnum == 3 && e.Channel.GuildId == 448241243496120321)
            {
                if (!lastStars.Contains(msg + "\n- <@" + e.Message.Author.Id + ">"))
                {
                    if (cUp <= 99)
                    {
                        lastStars[cUp] = msg + "\n- <@" + e.Message.Author.Id + ">";
                        cUp++;
                    } //<@174970757468651520>
                    else
                    {
                        cUp = 1;
                        lastStars[cUp] = msg + "\n- <@" + e.Message.Author.Id + ">";
                    }
                    if (!msg.Contains("@everyone") || !msg.Contains("@here") || !lastStars.Contains(msg + "\n- <@" + e.Message.Author.Id + ">"))
                    {
                        //Bot.Client.GetChannelAsync(448262707896909824).Result.SendMessageAsync(msg + "\n- <@" + e.Message.Author.Id + ">");
                        y.sendToChannel(456445338010517515, msg + "\n- <@" + e.Message.Author.Id + ">");
                    }//448262707896909824
                }//456445338010517515 quotechannel
            }
            return Task.CompletedTask;
        }
    }
}
