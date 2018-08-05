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
using Newtonsoft.Json;
using static someBot.XedddSpec;

namespace someBot
{
    class Xeddd
    {
        Bot y;
        public Xeddd(Bot form)
        {
            y = form;
        }


        int xCheck = 0;

        public void xedddUp()
        {
            if (File.ReadAllText(@"/dbots/someBot/logs/xeddd" + xCheck + ".txt").Length > 250000) //the richtextbox limit is much higher but the program will start to lag when the textfile is over 1mb in size, this is about 250kb
            {
                xCheck++;
                xedddUp();
            }
        }

        public void xedddMsgAddTxt(string addMsg)
        {
                if (File.ReadAllText(@"/dbots/someBot/logs/xeddd" + xCheck + ".txt").Length > 250000) //the richtextbox limit is much higher but the program will start to lag when the textfile is over 1mb in size, this is about 250kb
                {
                    xCheck++;
                    File.CreateText(@"/dbots/someBot/logs/xeddd" + xCheck + ".txt");
                    xedddMsgAddTxt(addMsg);
                }
                else
                {
                    File.AppendAllText(@"/dbots/someBot/logs/xeddd" + xCheck + ".txt", addMsg + Environment.NewLine); //adds it to the File, but since u dont have that i commented that out
                }
        }
        //fun begins from here uwu
        bool bxeddd = true;
        ulong xlast_boi = 0;
        public Task Bot_XedddMessageCreated(MessageCreateEventArgs e)
        {
            if (!(e.Message.Channel.Type.ToString() == "Private")) //cheack if it aint in DM's 
            {
                if (e.Guild.Id == 373635826703400960) //check if it in Xeddd server
                {
                    string proxy_urls = ""; //basically the discord url it spits out for uploaded files
                    string attlist = "";
                    string BotCheck = "";

                    if (e.Message.Attachments != null)
                    {
                        foreach (var files in e.Message.Attachments)
                        {
                            proxy_urls += "\n  " + files.ProxyUrl; //puts the file urls in a nice list
                        }
                    }

                    if (!(proxy_urls == "")) attlist = "\n Attachments:" + proxy_urls; //if there are attachments it will add "Attachments:" in front of it, for extra neatness
                    if (e.Message.Author.IsBot) BotCheck = "[Bot] "; //if a bot sent that it it adds [bot] infront
                    if (e.Message.Author.Id == xlast_boi && !(proxy_urls == "")) //when u upload 2 images thes are treated as 2 messages, so if the author is the same it just sends the file link
                    {
                        xlast_boi = e.Message.Author.Id;
                        xedddMsgAddTxt(proxy_urls);
                    }
                    else
                    {
                        xlast_boi = e.Message.Author.Id; //for the attachment thing
                        if (bxeddd == true)
                        {
                            bxeddd = false;
                        }
                        xedddMsgAddTxt("[" + e.Message.Channel.Name + "] " + BotCheck + e.Message.Author.Username + "(" + e.Guild.GetMemberAsync(e.Author.Id).Result.Nickname + "): " + e.Message.Content + attlist);
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task Bot_XedddMessageEdited(MessageUpdateEventArgs e) //same as the thing before but for edited messages, yes the bot log every version of a message, i also put and [edited] at the end *neat*
        {
            if (!(e.Message.Channel.Type.ToString() == "Private"))
            {
                if (e.Guild.Id == 373635826703400960)
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
                    if (e.Message.Author.Id == xlast_boi && !(proxy_urls == ""))
                    {
                        xlast_boi = e.Message.Author.Id;
                        xedddMsgAddTxt(proxy_urls + " [Edited]");
                    }
                    else
                    {
                        xlast_boi = e.Message.Author.Id;
                        if (bxeddd == true)
                        {
                            bxeddd = false;
                        }
                        xedddMsgAddTxt("[" + e.Message.Channel.Name + "] " + BotCheck + e.Message.Author.Username + "(" + e.Guild.GetMemberAsync(e.Author.Id).Result.Nickname + "): " + e.Message.Content + attlist + " [Edited]");
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task Bot_XedddBoiLeave(GuildMemberRemoveEventArgs e) //sends a message to owo if someone leaves the server
        {
            if (e.Guild.Id == 373635826703400960)
            {
                string leaver = e.Member.DisplayName + " (" + e.Member.Nickname + ") " + "left uwu";
                y.sendToChannel(391872124840574986, leaver); //the "e" does not provide a method to send stuff to another channel so thats the reason the method in form1 exists
            }
            return Task.CompletedTask;
        }

        public Task Bot_Dump(MessageCreateEventArgs e) //sends a message to owo if someone leaves the server
        {
            if (!(e.Message.Channel.Type.ToString() == "Private"))
            {
                StreamReader r = new StreamReader("XedddGroups.json");
                string json = r.ReadToEnd();
                var roles = JsonConvert.DeserializeObject<List<RootObject>>(json);
                //var roles = JsonConvert.DeserializeObject<List<RootObject>>(responseFromServer);
                //int select = roles.FindIndex(x => x.Name == role);
                if (e.Guild.Id == 373635826703400960)
                {
                    if (roles.Any(x => Convert.ToUInt64(x.ChannelID) == e.Channel.Id))
                    {
                        string proxy_urls = "";
                        string attlist = "";

                        if (e.Message.Attachments != null)
                        {
                            foreach (var files in e.Message.Attachments)
                            {
                                proxy_urls += "\n  " + files.ProxyUrl;
                            }
                        }
                        if (!(proxy_urls == "")) attlist = proxy_urls;
                        if (e.Message.Content.Contains("http"))
                        {
                            attlist += e.Message.Content;
                        }
                        if (attlist != "")
                        {
                            y.sendToChannel(466715815866269716, attlist);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
