using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.IO;	
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;


namespace someBot
{
    class Random //note i was just too lazy to add descriptions to these, will be there someday
    {
        //!!!you can define groups with [Group("groupName")] BUT its added to the command so if id call this group random, it'd be !random emotes

        [Command("emotes"),Description("lists all customs server emotes")] //same as if u just type emotes, 
        public async Task Emotes(CommandContext ctx)
        {
            string emotelist = "";

            foreach (var emotes in ctx.Guild.Emojis)
            {
                emotelist += emotes;
            }
            await ctx.RespondAsync(emotelist);
        }

        [Command("baguette"),Description("yeet")] //yeet
        public async Task Baguette(CommandContext ctx)
        {
            await ctx.RespondAsync(":french_bread::french_bread::french_bread::french_bread::french_bread:");
        }

        [Command("test"),Description("this should be called different but nah")] //i think u same some people use this, pretty simple
        public async Task Test(CommandContext ctx, [RemainingText]string oof)
        {
            await ctx.RespondAsync(oof);
        }

        [Command("roles"), Description("list roles")] //i think u same some people use this, pretty simple
        public async Task roles(CommandContext ctx)
        {
            foreach(var oof in ctx.Guild.Roles)
            {
                if (oof.Id != 373635826703400960)
                {
                    await ctx.Member.SendMessageAsync($"``{oof.Name} {oof.Id}``");
                }
            }
        }

        [Command("big"),Description("useless lol")] //kinda same as if u type big gay but commands arent allowed to contain spaces
        public async Task Gay(CommandContext ctx, string gay)
        {
            if (gay == "gay") //lmao
            {
                await ctx.RespondAsync("*useless command*");
            }
        }

        [Command("dj-links"), Aliases("music-links")] //i use sinusbot so, yea, long test is long, this was before i discovered Embeds
        public async Task DjLinks(CommandContext ctx)
        {
                await ctx.RespondAsync("**Webinterface**: https://srgg.de:8012/ \n" +
                    "**Youtube Search**: https://srgg.de:8012/scripts/youtube/ (Webinterface login required, if u have no personal account use 'dj' as username and password)\n" +
                    "**SyncWatch**: https://srgg.de:8012/scripts/syncwatch/ (for this Method 3 is required + webinterface account needs to be bound to your discord account (!music for details)) !yt <ytlink> to watch videos of the sent link together");
        }

        //https://www.youtube.com/embed?listType=search&list
        [Command("nnd"), Description("See the Hourly, Daily, Weekly and Monthly NND Vocaloid Rankinglist" +
                    "Please select a ranking list:\n" +
                    "``m!nnd hourly``\n" +
                    "``m!nnd daily``\n" +
                    "``m!nnd weekly``\n" +
                    "``m!nnd monthly``")] //see what i did there with the command error and the description? Note! if u just type !nnd it errors cause no string was defined
        public async Task NND(CommandContext ctx, string listPick)//this was crazy
        {
            try { 
            int offset = 0;
            if (listPick.ToLower() == "hourly") //u see later why the offset
            {
                offset = 0;
            }

            else if (listPick.ToLower() == "daily")
            {
                offset = 20;
            }

            else if (listPick.ToLower() == "weekly")
            {
                offset = 40;
            }

            else if (listPick.ToLower() == "monthly")
            {
                offset = 60;
            }

            else //if u just type it wrong it spits out the help too
            {
                await ctx.RespondAsync("Please select a ranking list:\n" +
                    "``m!nnd hourly``\n" +
                    "``m!nnd daily``\n" +
                    "``m!nnd weekly``\n" +
                    "``m!nnd monthly``");
                return;
            }
            var inter = ctx.Client.GetInteractivityModule(); //for selecting pages n stuff
            var init = await ctx.RespondAsync("this may take a while, be patient uwu");
            IWebDriver driver; //now this shit is lit, it calles SeleniumWebdriver is is kinda JS n Bootstrap in C#
            var chromeDriverService = FirefoxDriverService.CreateDefaultService(); //it calles crome cause i used chrome before, but firefox can be used without shoing the browser window!
            chromeDriverService.HideCommandPromptWindow = true; //selenium shows a commandprompt, this hides it
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            FirefoxOptions options = new FirefoxOptions(); 
            options.AddArguments("--headless"); //to tell firefox to start in windowless mode

            driver = new FirefoxDriver(chromeDriverService, options); //starts up firefox it in the directory already (called gecko)
            driver.Navigate().GoToUrl("http://ex.nicovideo.jp/vocaloid/ranking"); //ig goes to the ranking pages
            //var allRanking = driver.FindElements(By.ClassName("ttl")); i just put that in the foreach to save space i guess
            string[] allranks = new string[80]; // there are always 80 songs on that page! this is for the names
            string[] allURLS = new string[80]; //it also gets the urls
            int yeet = 0; //to add stuff to the array
            bool noExit = true; //for the interactivity part later
            foreach (var vids in driver.FindElements(By.ClassName("ttl"))) //lit ay? all items in the ranking have the classname ttl
            {
                allranks[yeet] = vids.Text; //it gest the inner text which is the Name
                allURLS[yeet] = driver.FindElement(By.LinkText(vids.Text)).GetAttribute("href"); //and the link from the "a" tag thats arond the Songname
                yeet++;
            }
            var empty = ctx.Client.GetGuildAsync(401419401011920907).Result.GetEmojiAsync(435447550196318218).Result; //i have an emoji on my server thats just blank, i use it for spaces in shit here
            var hourly = new DiscordEmbedBuilder //here it creates a discord embed
            {
                Color = new DiscordColor("#289b9a"), //whice cuz yea
                Title = "NicoNicoDouga " + listPick + " Vocaloid Ranking", //what u picked in the command
                Description = "Top 20 " + listPick + " songs!\n" + empty,
                ThumbnailUrl = "https://japansaucedotnet.files.wordpress.com/2016/05/photo-22.gif" //the niconico icon i used
            };
            for (int i = (offset + 0); i < (offset + 5); i++) //gets the first 5
            {
                if (i < (offset + 4))
                {
                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i]);
                }
                else
                {
                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i] + "\n" + empty);
                }
            }
            int pages = 1;
            hourly.AddField("Page", pages + "/4\nEnter Page number or type ``exit`` to exit");
            await ctx.Guild.GetChannel(ctx.Channel.Id).GetMessageAsync(init.Id).Result.ModifyAsync("", embed: hourly.Build()); //here it modifies the "please wait" and instead puts the embed

            while (noExit == true) //indefinetly
            {
                    try
                    {
                        var pSelect = await inter.WaitForMessageAsync(xm => xm.Author.Id == ctx.Message.Author.Id, TimeSpan.FromSeconds(60));
                        if (pSelect.Message.Content == "1")
                        {
                            hourly.ClearFields();//alwas clears the fields, fields are the stuff below the embed heading, description (and image i guess?)
                            for (int i = (offset + 0); i < (offset + 5); i++)
                            {
                                if (i < (offset + 4))
                                {
                                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i]);
                                }
                                else
                                {
                                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i] + "\n" + empty);
                                }
                            }
                            pages = 1;
                            hourly.AddField("Page", pages + "/4\nEnter Page number or type ``exit`` to exit");
                        }
                        else if (pSelect.Message.Content == "2")
                        {
                            hourly.ClearFields();
                            for (int i = (offset + 5); i < (offset + 10); i++)
                            {
                                if (i < (offset + 9))
                                {
                                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i]);
                                }
                                else
                                {
                                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i] + "\n" + empty);
                                }
                            }
                            pages = 2;
                            hourly.AddField("Page", pages + "/4\nEnter Page number or type ``exit`` to exit");
                        }
                        else if (pSelect.Message.Content == "3")
                        {
                            hourly.ClearFields();
                            for (int i = (offset + 10); i < (offset + 15); i++)
                            {
                                if (i < (offset + 14))
                                {
                                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i]);
                                }
                                else
                                {
                                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i] + "\n" + empty);
                                }
                            }
                            pages = 3;
                            hourly.AddField("Page", pages + "/4\nEnter Page number or type ``exit`` to exit");
                        }
                        else if (pSelect.Message.Content == "4")
                        {
                            hourly.ClearFields();
                            for (int i = (offset + 15); i < (offset + 20); i++)
                            {
                                if (i < (offset + 19))
                                {
                                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i]);
                                }
                                else
                                {
                                    hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i] + "\n" + empty);
                                }
                            }
                            pages = 4;
                            hourly.AddField("Page", pages + "/4\nEnter Page number or type ``exit`` to exit");
                        }
                        else if (pSelect.Message.Content.ToLower() == "exit") //so the while ends
                        {
                            noExit = false;
                        }
                        else // if u just type something else
                        {
                            await ctx.RespondAsync("Please type ``exit`` to exit the Ranking Menu!");
                        }

                        if (noExit == true)
                        {
                            await ctx.Guild.GetChannel(ctx.Message.Channel.Id).GetMessageAsync(pSelect.Message.Id).Result.DeleteAsync(); //deletes the users page pick, i know i could probably do this with reacts but thats what i worked with at the time xD
                            await ctx.Guild.GetChannel(ctx.Channel.Id).GetMessageAsync(init.Id).Result.ModifyAsync("", embed: hourly.Build());
                        }
                    }
                    catch
                    {
                        driver.Quit();
                        return;
                    }
            }
            driver.Quit();
            }
            catch
            {
                await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }//invitehttps://discordapp.com/oauth2/authorize?client_id=451362137571328000&scope=bot&permissions=67497024

        [Command("invite"),Aliases("link"),Description("Invitelink for the bot")] //guess....
        public async Task Invite(CommandContext ctx)
        {
            await ctx.RespondAsync("https://discordapp.com/oauth2/authorize?client_id=465675368775417856&scope=bot&permissions=67497025");
        }
    }
}