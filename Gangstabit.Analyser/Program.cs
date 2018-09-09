using System;
using System.IO;
using System.Threading.Tasks;
using Gangstabit.Business.Service;
using Gangstabit.DataAccess;
using Newtonsoft.Json;

namespace Gangstabit.Analyser
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
            var gameRepository = new GameRepository();

            var analysis = await gameRepository.GetAnalysisAsync().ConfigureAwait(false);

            var analysisJson = JsonConvert.SerializeObject(analysis);
            var gamesJson = JsonConvert.SerializeObject(analysis.Games);
            var playersJson = JsonConvert.SerializeObject(analysis.Players);

           var logPath = $@"C:\GangstabitResults\analysisJson.json";
           var logFile = System.IO.File.Create(logPath);
           var logWriter = new System.IO.StreamWriter(logFile);
           logWriter.WriteLine(analysisJson);
           logWriter.Dispose();

            logPath = $@"C:\GangstabitResults\gamesJson.json";
            logFile = System.IO.File.Create(logPath);
            logWriter = new System.IO.StreamWriter(logFile);
            logWriter.WriteLine(gamesJson);
            logWriter.Dispose();

            logPath = $@"C:\GangstabitResults\playersJson.json";
            logFile = System.IO.File.Create(logPath);
            logWriter = new System.IO.StreamWriter(logFile);
            logWriter.WriteLine(playersJson);
            logWriter.Dispose();

            Console.WriteLine("done");
        }

    }
}
