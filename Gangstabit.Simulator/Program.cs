using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gangstabit.Business;
using Gangstabit.Business.Controllers;
using Gangstabit.Business.Model;
using Newtonsoft.Json;

namespace Gangstabit.Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainAsync().Wait();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.Read();
        }


        static async Task MainAsync()
        {
            var results = new ConcurrentDictionary<ControllerSettings, Player>();
            var simulationRunner = new SimulationRunner();
            var games = await simulationRunner.LoadGames(new DateTime(2018, 09, 01, 06, 30, 0), new DateTime(2018, 09, 12, 17, 30, 0)).ConfigureAwait(false);
            games = games.OrderBy(g => g.Id).ToList();
            simulationRunner = new SimulationRunner();
            var player = new Player()
            {
                Wallet = 45000
            };

            var controllerSettings = new ControllerSettings()
            {
                BaseBet = 5,
                Multiplier =2.7,
                PassGames = 3,
                Reducer = 0.005,
                Target = 2.02
            };

            var controller = new RouletteController(
                player, controllerSettings.BaseBet,
                controllerSettings.Target,
                controllerSettings.Multiplier,
                controllerSettings.PassGames,
                controllerSettings.Reducer);

            simulationRunner.StartWith(player, controller, games);
            player.ComputeStats();
            player.ControllerResults = controller.ToString();
            results.TryAdd(controllerSettings, player);
            Console.WriteLine(player);
            Console.WriteLine(controller);

        }

        static async Task MainSearchAsync()
        {
            var results = new ConcurrentDictionary<ControllerSettings, Player>();
                    var simulationRunner = new SimulationRunner();
            var games = await simulationRunner.LoadGames(new DateTime(2018, 09, 02, 16, 32, 0), new DateTime(2018, 09, 9, 13, 0, 0)).ConfigureAwait(false);
            games.LightShuffle();
            var allGames = new List<List<Game>>();
            for (int i = 0; i < 10; i++)
            {
                games.LightShuffle();
                allGames.Add(games.ToList());
            }
           
            //games = games.OrderBy(g => g.Id).ToList();
            int simulationCount = 0;
            Parallel.ForEach(Enumerable.Range(1, 10000000),
                new ParallelOptions {MaxDegreeOfParallelism = 8},
                (count) =>
                {
                    var players = new List<Player>();
                    var controllerSettings = GenerateRandomControllerSettings();
                    foreach (var allGame in allGames)
                    {
                        simulationCount++;
                        simulationRunner = new SimulationRunner();
                        var player = new Player()
                        {
                            Wallet = 37500
                        };

                        var controller = new RouletteController(
                            player, controllerSettings.BaseBet,
                            controllerSettings.Target,
                            controllerSettings.Multiplier,
                            controllerSettings.PassGames,
                            controllerSettings.Reducer);

                        simulationRunner.StartWith(player, controller, allGame);
                        player.ComputeStats();
                        player.Bets.Clear();
                        player.ControllerResults = controller.ToString();
                        players.Add(player);
                    }
                    if (players.Any(p => p.Roi <= 0 ||  double.IsNaN(p.Roi)))
                    {

                    }
                    else
                    {
                        var averagePlayer = new Player()
                        {
                            Benefits = players.Sum(p => p.Benefits) / players.Count,
                            Roi = players.Sum(p => p.Roi) / players.Count,
                            Wallet = players.Sum(p => p.Wallet) / players.Count,
                        };
                        results.TryAdd(controllerSettings, averagePlayer);
                    }

                    //Console.WriteLine(player);
                    //Console.WriteLine(controller);

                    if (simulationCount % 5000 == 0)
                    {
                        try
                        {
                            AnalyzeResults(results.ToList(), simulationCount);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                });
        }

        static void AnalyzeResults(IList<KeyValuePair<ControllerSettings,Player>> results, int count)
        {
            try
            {
                var orderedResults = results.OrderByDescending(r => r.Value.Benefits).ToList();

                for (int i = 0; i < 10; i++)
                {
                    var item = orderedResults[i];
                    var json = JsonConvert.SerializeObject(item);
                    var logPath = $@"C:\GangstabitResults\results_best_{i}_{count}.json";
                    var logFile = System.IO.File.Create(logPath);
                    var logWriter = new System.IO.StreamWriter(logFile);
                    logWriter.WriteLine(json);
                    logWriter.Dispose();
                }
            }
            catch (Exception e)
            {
            }
           
       
        }

        static ControllerSettings GenerateRandomControllerSettings()
        {
            return new ControllerSettings
            {
                BaseBet = StaticRandom.Rand(100),
                Target = (double)(StaticRandom.Rand(300)+10)/10,
                Multiplier = (double)(StaticRandom.Rand(300)+50)/100,
                PassGames =StaticRandom.Rand(20),
                Reducer = (double)StaticRandom.Rand(10)/100,
            };
        }
    }
}
