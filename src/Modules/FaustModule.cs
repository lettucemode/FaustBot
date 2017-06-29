using Discord;
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
        private static double[] probs = new double[]
        {
            2.3d,   //helium
            3.4d,   //platform
            3.7d,   //100-ton weight
            3.15d,  //black hole
            5.15d,  //meteors
            4.95d,  //oil
            11.05d, //chocolate
            9.95d,  //poison
            8.45d,  //doughnut
            12.05d, //bomb
            12.4d,  //hammer
            15.5d,  //mini faust
            1.7d,   //fireworks
            .5d,    //massive meteor
            .95d,   //golden hammer
            1.9d,   //big faust
            .75d,   //golden chocolate
            .75d,   //box of doughnuts
            .4d,    //1000-ton weight
        };
        //image album: https://imgur.com/a/y1qev
        private static string[] items = new string[] 
        {
            "https://i.imgur.com/L8iqDFE.png",
            "https://i.imgur.com/vull3Pn.png",
            "https://i.imgur.com/wpLjyjY.png",
            "https://i.imgur.com/Ie1MWKC.png",
            "http://www.dustloop.com/wiki/images/thumb/8/89/GGXRD_Faust_Meteors.png/174px-GGXRD_Faust_Meteors.png",
            "https://i.imgur.com/DEdTB4c.png",
            "https://i.imgur.com/iyJWfzS.png",
            "https://i.imgur.com/HpoZuAc.png",
            "https://i.imgur.com/2ky0DcA.png",
            "https://i.imgur.com/lN78xNm.png",
            "https://i.imgur.com/GwNY1W5.png",
            "https://i.imgur.com/devW3ba.png",
            "https://pbs.twimg.com/media/C_ZP9_1UMAIwHqK.jpg",
            "https://i1.wp.com/glitchcat.com/wp-content/uploads/2017/02/Faust-new-item_Giant-Meteor.jpg",
            "https://i.imgur.com/1Xznk8g.png",
            "http://www.dustloop.com/wiki/images/5/50/GGXRD_Faust_ChibiFaust.png",
            "https://i.imgur.com/dGuLZGv.png",
            "https://i.imgur.com/NoWf8jl.png",
            "https://i.imgur.com/o9Tk9vs.png"
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
        public async Task Item(int meter = 0)
        {
            if (meter != 0 && meter != 50 && meter != 100) meter = 0;

            //nani ga deru kana?
            int punctRoll = rand.Next(3);
            string punct = (punctRoll == 0 ? "?" : punctRoll == 1 ? "!" : "!?");
            string nani = (meter == 100 ? "nanananananananananani" : meter == 50 ? "Na-na-na-nani" : "Nani") + " ga deru kana";
            await ReplyAsync(nani + punct);

            int itemNum = meter == 100 ? 10 : meter == 50 ? 4 : 1;
            for (int k = 0; k < itemNum; ++k)
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

                var embed = new EmbedBuilder().WithImageUrl(items[i]);
                await ReplyAsync("", embed: embed);
                //await ReplyAsync(items[i]);
            }

        }

        [Command("kanchou")]
        [Remarks("Kanchou a friend!")]
        [MinPermissions(AccessLevel.User)]
        public async Task Kanchou()
        {
            //await ReplyAsync("test");
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
