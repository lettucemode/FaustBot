using Discord.Commands;
using FaustBot.Preconditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaustBot.Modules
{
    [Name("Faust")]
    public class FaustModule : ModuleBase<SocketCommandContext>
    {
        private List<string> items = new List<string>() { ":hammer:", ":bomb:", ":doughnut:", ":chocolate_bar:",
            ":skull_crossbones::cloud:", ":comet:", ":fireworks:", ":oil:", ":black_circle:" };
        private Random rand = new Random();

        [Command("item")]
        [Remarks("Toss a random item!")]
        [MinPermissions(AccessLevel.User)]
        public async Task Item()
        {
            int punct = rand.Next(3);
            string nani = "Nani ga deru kana" + (punct == 0 ? "?" : punct == 1 ? "!" : "!?");
            await ReplyAsync(nani + " " + items[rand.Next(items.Count)]);
        }
    }
}
