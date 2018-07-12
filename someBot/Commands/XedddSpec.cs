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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Net;

namespace someBot
{
    class XedddSpec
    {
        [Command("role"), Description("get a role to access a vocaloid channel")] //yeet
        public async Task VocRole(CommandContext ctx, string role)
        {
            if(role.ToLower() == "len")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466716801682767882))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466716801682767882));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466716801682767882));
            }
            else if (role.ToLower() == "rin")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466719035636187154))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466719035636187154));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466719035636187154));
            }
            else if (role.ToLower() == "miku")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466719209292824586))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466719209292824586));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466719209292824586));
            }
            else if (role.ToLower() == "kaito")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466719459311091732))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466719459311091732));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466719459311091732));
            }
            else if (role.ToLower() == "meiko")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466719766988718090))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466719766988718090));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466719766988718090));
            }
            else if (role.ToLower() == "luka")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(4667198593932984343))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466719859393298434));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466719859393298434));
            }
            else if (role.ToLower() == "gumi")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466720077752827904))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466720077752827904));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466720077752827904));
            }
            else if (role.ToLower() == "gakupo")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466720148657668127))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466720148657668127));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466720148657668127));
            }
            else if (role.ToLower() == "fukase")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466720244849704971))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466720244849704971));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466720244849704971));
            }
            //UTAU
            else if (role.ToLower() == "defoko")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466720505031032842))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466720505031032842));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466720505031032842));
            }
            else if (role.ToLower() == "teto")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466720581618761738))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466720581618761738));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466720581618761738));
            }
            else if (role.ToLower() == "momo")
            {
                if (!ctx.Member.Roles.Contains(ctx.Guild.GetRole(466720730017562633))) await ctx.Member.GrantRoleAsync(ctx.Guild.GetRole(466720730017562633));
                else await ctx.Member.RevokeRoleAsync(ctx.Guild.GetRole(466720730017562633));
            }

            else
            {
                await ctx.RespondAsync("For Vocaloid Channels: len, rin, miku, kaito, meiko, luka, gumi, gakupo or fukase \n" +
                    "For UTAU Channels: defoko, teto, momo \n" +
                    "Usage: ``m!role <rolename>`` \n" +
                    "using the command again will remove the role");
            }
        }
    }
}
