using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gangstabit.Business;
using Gangstabit.Business.Controllers;
using Gangstabit.Business.Model;
using Newtonsoft.Json;

namespace Gangstabit.Simulator
{
    class Program
    {
        private static System.Threading.Timer timer;

        static void Main(string[] args)
        {
            Console.WriteLine("hello");
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
            int initialWallet = 45000;
            var results = new ConcurrentDictionary<ControllerSettings, Player>();
            var simulationRunner = new SimulationRunner();
            var games = await simulationRunner.LoadGames(new DateTime(2018, 09, 13, 22, 30, 0), new DateTime(2018, 09, 14, 6, 30, 0)).ConfigureAwait(false);
            games = games.OrderBy(g => g.Id).ToList();
            simulationRunner = new SimulationRunner();
            var player = new Player()
            {
                Wallet = initialWallet
            };

            var controllerSettings = new ControllerSettings()
            {
                BaseBet = 4,
                Multiplier =2.09,
                PassGames = 7,
                Reducer = 0.001,
                Target = 7
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
            var controllerResults = controller.GetControllerResults();

            var simulationResult = new SimulationResult(controllerSettings, controllerResults, player, initialWallet);
            results.TryAdd(controllerSettings, player);
            Console.WriteLine(player);
            Console.WriteLine(controller);
            Console.WriteLine(simulationResult);

        }

        static async Task MainSearchAsync()
        {
            int initialWallet = 28000;
            int simulationCount = 0;
            var results = new ConcurrentDictionary<double, SimulationResult>();
                    var simulationRunner = new SimulationRunner();

            timer = new Timer((e) =>
            {
                try
                {
                    AnalyzeResults(results.ToList(), simulationCount);
                    Console.WriteLine($"Saved at {simulationCount} runs");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }, null, TimeSpan.FromMilliseconds(5000), TimeSpan.FromMilliseconds(5000));

            var games = await simulationRunner.LoadGames(new DateTime(2018, 09, 13, 22, 30, 0), new DateTime(2018, 09, 14, 6, 30, 0)).ConfigureAwait(false);
            games = games.OrderBy(g => g.Id).ToList();

            Console.WriteLine("launching Foreach");

            Parallel.ForEach(Enumerable.Range(1, 10000000),
                new ParallelOptions {MaxDegreeOfParallelism = 8},
                (count) =>
                {
                    var controllerSettings = GenerateRandomControllerSettings();
                        simulationCount++;
                        simulationRunner = new SimulationRunner();
                        var player = new Player()
                        {
                            Wallet = 45000
                        };

                        var controller = new RouletteController(
                            player, controllerSettings.BaseBet,
                            controllerSettings.Target,
                            controllerSettings.Multiplier,
                            controllerSettings.PassGames,
                            controllerSettings.Reducer);

                        simulationRunner.StartWith(player, controller, games);
                        player.ComputeStats();
                        player.Bets.Clear();
                    var controllerResults = controller.GetControllerResults();

                    var simulationResult = new SimulationResult(controllerSettings, controllerResults, player, initialWallet);
                    player.ControllerResults = controller.ToString();
                    results.TryAdd(simulationResult.TotalScore, simulationResult);
                });
        }

        static void AnalyzeResults(IList<KeyValuePair<double, SimulationResult>> results, int count)
        {
            try
            {
                var orderedResults = results.OrderByDescending(r => r.Key).Take(20).ToList();

                var json = JsonConvert.SerializeObject(orderedResults);
                var logPath = $@"C:\GangstabitResults\results_best20_{DateTime.UtcNow.ToString("MM-dd-yyyy")}_{count}.json";
                var logFile = System.IO.File.Create(logPath);
                var logWriter = new System.IO.StreamWriter(logFile);
                logWriter.WriteLine(json);
                logWriter.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static ControllerSettings GenerateRandomControllerSettings()
        {
            return new ControllerSettings
            {
                BaseBet = StaticRandom.Rand(20),
                Target = (double)(StaticRandom.Rand(100)+10)/10,
                Multiplier = (double)(StaticRandom.Rand(300)+50)/100,
                PassGames =StaticRandom.Rand(10),
                Reducer = (double)StaticRandom.Rand(10)/1000,
            };
        }
    }
}
