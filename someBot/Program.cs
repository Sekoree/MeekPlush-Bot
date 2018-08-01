using System;
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
