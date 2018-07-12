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
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using WikiClientLibrary.Pages;
using WikiClientLibrary.Client;
using WikiClientLibrary.Sites;
using System.Net;

namespace someBot
{
    class Wiki
    {
        [Command("knowledge"), Description("Links you to a random Wikipedia article")]
        public async Task WikiRan(CommandContext ctx)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://en.wikipedia.org/wiki/Special:Random");
            req.AllowAutoRedirect = true;
            HttpWebResponse myResp = (HttpWebResponse)req.GetResponse();
            await ctx.RespondAsync(myResp.ResponseUri.ToString());
        }

        [Command("wiki"), Description("Search Wikipedia!")]
        public async Task Wikipedia(CommandContext ctx, [RemainingText] string stuff)
        {
            var wikiClient = new WikiClient
            {
                ClientUserAgent = "Discord DSharpPlus Bot",
            };
            // Create a MediaWiki Site instance with the URL of API endpoint.
            var site = new WikiSite(wikiClient, "https://en.wikipedia.org/w/api.php");
            await site.Initialization;
            try
            {
                var ent = site.OpenSearchAsync(stuff).Result[0];
                await ctx.RespondAsync("Found " + ent.Title + "\nLink: " + ent.Url);
            }
            catch
            {
                await ctx.RespondAsync("nothing found uwu");
            }
        }

        [Command("touhouwiki"), Description("Search Touhouwiki!"), Aliases("tohowiki")]
        public async Task tohowiki(CommandContext ctx, [RemainingText] string stuff)
        {
            var wikiClient = new WikiClient
            {
                ClientUserAgent = "Discord DSharpPlus Bot",
            };
            // Create a MediaWiki Site instance with the URL of API endpoint.
            var site = new WikiSite(wikiClient, "https://en.touhouwiki.net/api.php");
            await site.Initialization;
            try
            {
                var ent = site.OpenSearchAsync(stuff).Result[0];
                await ctx.RespondAsync("Found " + ent.Title + "\nLink: " + ent.Url);
            }
            catch
            {
                await ctx.RespondAsync("nothing found uwu");
            }
        }
    }
}
