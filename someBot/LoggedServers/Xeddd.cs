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

        string[] badbois = new string[100];
        int[] vioAm = new int[100];
        int ripperoni = 0;

        public Task Bot_XedddLawEnforce(MessageCreateEventArgs e) //we dont use this, but i made it, its nowhere near perfect and will probably never be used, there are probably easier ways to do this
        {
            if (e.Guild.Id == 373635826703400960)
            {
                //Rule#1
                if (e.Message.Content.ToLower().Contains("fuck you") || e.Message.Content.ToLower().Contains("fuck off") || e.Message.Content.ToLower().Contains("is a bitch") || e.Message.Content.ToLower().Contains("an asshole") || e.Message.Content.ToLower().Contains("you nigga") || e.Message.Content.ToLower().Contains("please die"))
                {
                    if (ripperoni == 100)
                    {
                        ripperoni = 0;
                        badbois[ripperoni] = "";
                        vioAm[ripperoni] = 0;
                    }
                    e.Message.RespondAsync("Please do NOT insult or harass others! (Rule#1)");
                    badbois[ripperoni] = e.Message.Author.Id.ToString();
                    vioAm[ripperoni] = vioAm[ripperoni] + 1;
                }
                //Rule#3
                if ((e.Message.Content.ToLower().Contains("i want to die") && e.Message.Content.ToLower().Contains("really")) && !(e.Message.Channel.Id == 424971266181824523))
                {
                    e.Message.RespondAsync("Please use #suicice-serious if this was a serious statement (Rule#3)");
                }
                //Rule#7
                if (e.Message.Content.ToLower().Contains("fucking Mod") || e.Message.Content.ToLower().Contains("fuck off admin") || e.Message.Content.ToLower().Contains("fuck the memebois") || e.Message.Content.ToLower().Contains("fuck the meme bois") || e.Message.Content.ToLower().Contains("mods should fuck off") || e.Message.Content.ToLower().Contains("admins should fuck off") || e.Message.Content.ToLower().Contains("mods fuck off") || e.Message.Content.ToLower().Contains("admins fuck off"))
                {
                    if (ripperoni == 100)
                    {
                        ripperoni = 0;
                        badbois[ripperoni] = "";
                        vioAm[ripperoni] = 0;
                    }
                    e.Message.RespondAsync("Please respect the Mods! If you dont bother us, we wont bother you! (Rule#7)");
                    badbois[ripperoni] = e.Message.Author.Id.ToString();
                    vioAm[ripperoni] = vioAm[ripperoni] + 1;
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
            ulong[] Vocas = { 435257131789320233, 435257195106664458, 435257232133849098, 435257330603393036, 435257510560268298, 435257651312590868, 437088673654243338, 460630459270037504, 462360419101835283 };
            ulong[] Utas = { 460412979507101717, 460413000109654039, 460413038743257089 };
            if (e.Guild.Id == 373635826703400960)
            {
                if (Vocas.Contains(e.Channel.Id) || Utas.Contains(e.Channel.Id))
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
                    if (!(proxy_urls == "")) attlist =  proxy_urls;
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
            return Task.CompletedTask;
        }
    }
}
