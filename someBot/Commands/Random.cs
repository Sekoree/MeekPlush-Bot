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
        public async Task NND(CommandContext ctx, string listPick = null)//this was crazy
        {
            try
            {
                int offset = 0;
                if (listPick.ToLower() == "hourly") offset = 0;
                else if (listPick.ToLower() == "daily") offset = 20;
                else if (listPick.ToLower() == "weekly") offset = 40;
                else if (listPick.ToLower() == "monthly") offset = 60;
                else //if u just type it wrong it spits out the help too
                {
                    await ctx.RespondAsync(ctx.Command.Description);
                    return;
                }
                var inter = ctx.Client.GetInteractivityModule();
                var init = await ctx.RespondAsync("this may take a while, be patient uwu");
                IWebDriver driver;
                var chromeDriverService = FirefoxDriverService.CreateDefaultService();
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
                DiscordColor meek = new DiscordColor("#289b9a");
                foreach (var vids in driver.FindElements(By.ClassName("ttl"))) //lit ay? all items in the ranking have the classname ttl
                {
                    allranks[yeet] = vids.Text; //it gest the inner text which is the Name
                    allURLS[yeet] = driver.FindElement(By.LinkText(vids.Text)).GetAttribute("href"); //and the link from the "a" tag thats arond the Songname
                    yeet++;
                }
                var empty = ctx.Client.GetGuildAsync(401419401011920907).Result.GetEmojiAsync(435447550196318218).Result; //i have an emoji on my server thats just blank, i use it for spaces in shit here
                List<Page> pgs = new List<Page>();
                DiscordEmbedBuilder bassc = new DiscordEmbedBuilder
                {
                    Color = meek,
                    Title = "NicoNicoDouga " + listPick + " Vocaloid Ranking",
                    Description = "Top 20 " + listPick + " songs!\n" + empty,
                    ThumbnailUrl = "https://japansaucedotnet.files.wordpress.com/2016/05/photo-22.gif"
                };
                //DiscordEmbedBuilder[] hourly = new DiscordEmbedBuilder[4];
                int adv = 0;
                int adv2 = 5;
                for (int j = 0; j < 4; j++)
                {
                    if (j == 1)
                    {
                        adv = 5;
                        adv2 = 10;
                    }
                    if (j == 2)
                    {
                        adv = 10;
                        adv2 = 15;
                    }
                    if (j == 3)
                    {
                        adv = 15;
                        adv2 = 20;
                    }
                    Console.WriteLine("whut");
                    DiscordEmbedBuilder hourly = new DiscordEmbedBuilder
                    {
                        Color = meek,
                        Title = "NicoNicoDouga " + listPick + " Vocaloid Ranking",
                        Description = "Top 20 " + listPick + " songs!\n" + empty,
                        ThumbnailUrl = "https://japansaucedotnet.files.wordpress.com/2016/05/photo-22.gif"
                    };
                    for (int i = (offset + adv); i < (offset + adv2); i++) //gets the first 5
                    {
                        if (i < (offset + (adv2 - 1)))
                        {
                            hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i]);
                        }
                        else
                        {
                            hourly.AddField("#" + ((i - offset) + 1) + " " + allranks[i], allURLS[i] + "\n" + empty);
                        }
                    }
                    hourly.AddField("Page", $"{(j+1)}/4");
                    pgs.Add(new Page { Embed = hourly.Build() });
                }
                driver.Quit();
                //hourly.AddField("Page", pages + "/4\nEnter Page number or type ``exit`` to exit");
                await init.DeleteAsync();
                await inter.SendPaginatedMessage(ctx.Channel, ctx.Message.Author, pgs, timeoutoverride: TimeSpan.FromMinutes(2));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                //await ctx.RespondAsync("Hi! I'm missing either the Manage Messages or Embed Links Permission \nPlease add those so my commands work uwu");
            }
        }//invitehttps://discordapp.com/oauth2/authorize?client_id=451362137571328000&scope=bot&permissions=67497024

        [Command("invite"),Aliases("link"),Description("Invitelink for the bot")] //guess....
        public async Task Invite(CommandContext ctx)
        {//Object reference not set to an instance of an object.
            //at someBot.Random.< NND > d__6.MoveNext() in / dbots / someBot / someBot / someBot / Commands / Random.cs:line 161
            await ctx.RespondAsync("https://discordapp.com/oauth2/authorize?client_id=465675368775417856&scope=bot&permissions=67497025");
        }
    }
}