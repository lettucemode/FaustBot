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
        private static string[] items = new string[] 
        {
            ":doughnut:",
            ":chocolate_bar:",
            ":hammer:",
            "http://www.dustloop.com/wiki/images/5/50/GGXRD_Faust_ChibiFaust.png",
            ":bomb:",
            ":comet:",
            "http://www.dustloop.com/wiki/images/thumb/a/aa/GGXRD_Faust_Poison.png/140px-GGXRD_Faust_Poison.png",
            "http://www.dustloop.com/wiki/images/thumb/8/83/GGXRD_Faust_100TonWeight.png/175px-GGXRD_Faust_100TonWeight.png",
            ":oil:",
            "http://www.dustloop.com/wiki/images/thumb/7/7b/GGXRD_Faust_JumpPad.png/175px-GGXRD_Faust_JumpPad.png",
            "http://www.dustloop.com/wiki/images/1/13/GGXRD_Faust_BlackHole.png",
            "http://www.dustloop.com/wiki/images/thumb/9/98/GGXRD_Faust_HeliumGas.png/118px-GGXRD_Faust_HeliumGas.png"
        };
        private static double[] probs = new double[]
        {
            9.6d,   //donut
            8.3d,   //chocolate
            16.6d,  //hammer
            16.9d,  //minifaust
            14.4d,  //bomb
            5.5d,   //meteors
            9d,     //poison
            3.7d,   //weight
            6d,     //oil
            3.6d,   //jump pad
            4d,     //black hole
            2.4d    //helium
        };
        private double[] accum;
        private string kanchou1 = "<:kanchou1:303272976998989824>";
        private string kanchou2 = "<:kanchou2:303273897330212874>";
        private Random rand = new Random();

        public FaustModule()
        {
            //precalc accumulated probability array for item tossing
            accum = new double[items.Length];
            double total = 0d;
            for (int i = 0; i < items.Length; ++i)
            {
                total += probs[i];
                accum[i] = total;
            }
        }

        [Command("item")]
        [Remarks("Toss a random item!")]
        [MinPermissions(AccessLevel.User)]
        public async Task Item()
        {
            //binary search
            double roll = rand.NextDouble() * accum[accum.Length - 1];
            int L = 0;
            int R = accum.Length - 1;
            int i = -1;
            while (i == -1)
            {
                int M = (L + R) / 2;
                double left = M == 0 ? 0 : accum[M - 1];
                double right = accum[M];
                if (left <= roll && roll < right) i = M;
                else if (roll < left) R = M - 1;
                else if (roll > right) L = M + 1;
            }

            //send it
            int punct = rand.Next(3);
            string nani = "Nani ga deru kana" + (punct == 0 ? "?" : punct == 1 ? "!" : "!?");
            await ReplyAsync(nani + " " + items[i]);
        }

        [Command("kanchou")]
        [Remarks("Kanchou a friend!")]
        [MinPermissions(AccessLevel.User)]
        public async Task Kanchou()
        {
            await ReplyAsync(kanchou1 + kanchou2);
        }

        [Command("mutip")]
        [Remarks("Get a tip.")]
        [MinPermissions(AccessLevel.User)]
        public async Task MUTip()
        {
            string tip = Configuration.Load().Tips.Ino[rand.Next(2)];
            await ReplyAsync(tip);
        }
    }
}
