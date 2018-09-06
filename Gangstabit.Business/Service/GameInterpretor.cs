using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Gangstabit.Business.Model;

namespace Gangstabit.Business.Service
{
    public class GameInterpretor
    {
        public Game InterpreteGameFromHtml(string html, string fileName)
        {
            try
            {
                // Date
                var split1 = fileName.Split('\\');
                var split2 = split1.Last().Split('.');
                var endDate = new DateTime(long.Parse(split2.First()));

                // Multiplier
                var split3 = html.Split(new string[]{"\" style=\"color: rgb(46, 204, 113);\">"}, StringSplitOptions.None);
                var split4 = split3[1].Split(new string[] { "</a></td>" }, StringSplitOptions.None);
                var truncated1 = split4[0].Remove(split4[0].Length - 1);
                var multiplier = double.Parse(truncated1, CultureInfo.InvariantCulture);
                var htmlSplit = html.Split('\r');

                var game = new Game()
                {
                    EndDate = endDate,
                    Multiplier = multiplier
                };

                var bets = new List<Bet>();

                // Players
                var split5 = html.Split(new string[] { "href=\"/user/" }, StringSplitOptions.None);
                for (int i = 1; i < split5.Length; i++)
                {
                    var playerPart = split5[i];
                    var split6 = playerPart.Split('"');

                    var playerName = split6[0];
                    var split7 = playerPart.Split(new string[] { "<td class=\"text-right\">" }, StringSplitOptions.None);

                    var split8 = split7[1].Split(new string[] { "</td>" }, StringSplitOptions.None);
                    var truncated2 = split8[0].Remove(split8[0].Length - 1);
                    var target = double.Parse(truncated2, CultureInfo.InvariantCulture);

                    var split9 = split7[2].Split(new string[] { "</td>" }, StringSplitOptions.None);
                    var wage = double.Parse(split9[0], CultureInfo.InvariantCulture);

                    var bet = new Bet()
                    {
                        Game = game,
                        Player = new Player()
                        {
                            Name = playerName
                        },
                        Target = target,
                        Wage = wage
                    };

                    bets.Add(bet);
                }

                game.Bets = bets;

                return game;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to parse game date from File");
                return null;
            }


        }
    }
}
