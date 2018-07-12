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
    class My
    {
        Bot y;
        public My(Bot form)
        {
            y = form;
        }

        int myCheck = 0;

        public void myUp()
        {
            if (File.ReadAllText(@"/dbots/someBot/logs/my" + myCheck + ".txt").Length > 250000)
            {
                myCheck++;
                myUp();
            }
        }

        public void myMsgAddTxt(string addMsg)
        {
                if (File.ReadAllText(@"/dbots/someBot/logs/my" + myCheck + ".txt").Length > 250000)
                {
                    myCheck++;
                    File.CreateText(@"/dbots/someBot/logs/my" + myCheck + ".txt");
                    myMsgAddTxt(addMsg);
                }
                else
                {
                    File.AppendAllText(@"/dbots/someBot/logs/my" + myCheck + ".txt", addMsg + Environment.NewLine);
                }
        }

        bool bmylog = true;
        ulong mlast_boi = 0;

        public Task Bot_MyMessageCreated(MessageCreateEventArgs e)
        {
            if (!(e.Message.Channel.Type.ToString() == "Private"))
            {
                if (e.Guild.Id == 401419401011920907 && !(e.Message.ChannelId == 401419609946980352) && !(e.Message.ChannelId == 445989871589392404) && !(e.Message.ChannelId == 445999293921230863) && !(e.Message.ChannelId == 4358676259319316881))
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
                    if (e.Message.Author.Id == mlast_boi && !(proxy_urls == ""))
                    {
                        mlast_boi = e.Message.Author.Id;
                        myMsgAddTxt(proxy_urls);
                    }
                    else
                    {
                        mlast_boi = e.Message.Author.Id;
                        if (bmylog == true)
                        {
                            bmylog = false;
                        }
                        myMsgAddTxt("[" + e.Message.Channel.Name + "] " + BotCheck + e.Message.Author.Username + "(" + e.Guild.GetMemberAsync(e.Author.Id).Result.Nickname + "): " + e.Message.Content + attlist);
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task Bot_MyMessageEdited(MessageUpdateEventArgs e)
        {
            if (!(e.Message.Channel.Type.ToString() == "Private"))
            {
                if (e.Guild.Id == 401419401011920907 && !(e.Message.ChannelId == 401419609946980352) && !(e.Message.ChannelId == 445989871589392404) && !(e.Message.ChannelId == 445999293921230863) && !(e.Message.ChannelId == 4358676259319316881))
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
                    if (e.Message.Author.Id == mlast_boi && !(proxy_urls == ""))
                    {
                        mlast_boi = e.Message.Author.Id;
                        myMsgAddTxt(proxy_urls + " [Edited]");
                    }
                    else
                    {
                        mlast_boi = e.Message.Author.Id;
                        if (bmylog == true)
                        {
                            bmylog = false;
                        }
                        myMsgAddTxt("[" + e.Message.Channel.Name + "] " + BotCheck + e.Message.Author.Username + "(" + e.Guild.GetMemberAsync(e.Author.Id).Result.Nickname + "): " + e.Message.Content + attlist + " [Edited]");
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}