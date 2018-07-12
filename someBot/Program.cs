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
using DSharpPlus.Interactivity;
using System.IO;

namespace someBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            using (var b = new Bot())
            {
                b.RunAsync().Wait();
            }
        }
    }
}
