using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace someBot.Commands.Audio
{
    public class Functions
    {
        public Task Pause(int pos)
        {
            Bot.guit[pos].paused = true;
            Bot.guit[pos].LLGuild.Pause();
            return Task.CompletedTask;
        }

        public Task Resume(int pos)
        {
            Bot.guit[pos].paused = false;
            Bot.guit[pos].LLGuild.Resume();
            return Task.CompletedTask;
        }
        public Task Repeat(int pos)
        {
            Bot.guit[pos].repeat = !Bot.guit[pos].repeat;
            return Task.CompletedTask;
        }
        public Task RepeatAll(int pos)
        {
            Bot.guit[pos].repeatAll = !Bot.guit[pos].repeatAll;
            return Task.CompletedTask;
        }
        public Task Shuffle(int pos)
        {
            Bot.guit[pos].shuffle = !Bot.guit[pos].shuffle;
            return Task.CompletedTask;
        }
        public Task Skip(int pos)
        {
            Bot.guit[pos].paused = false;
            Bot.guit[pos].LLGuild.Stop();
            return Task.CompletedTask;
        }
        public Task Stop(int pos)
        {
            Bot.guit[pos].paused = false;
            Bot.guit[pos].stoppin = true;
            Bot.guit[pos].playing = false;
            Bot.guit[pos].LLGuild.Stop();
            return Task.CompletedTask;
        }
    }
}
