using System;
using System.IO;
using System.Threading.Tasks;
using Gangstabit.Business.Service;
using Gangstabit.DataAccess;

namespace Gangstabit.Uploader
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
            var interpretor = new GameInterpretor();
            var gameRepository = new GameRepository();


            foreach (string file in Directory.EnumerateFiles(
                @"C:\Gangstabit",
                "*",
                SearchOption.AllDirectories)
            )
            {

                var logReader = new System.IO.StreamReader(file);
                var html = await logReader.ReadToEndAsync();
                var game = interpretor.InterpreteGameFromHtml(html, file);
                logReader.Dispose();

                await gameRepository.SaveGameAsync(game);
                Console.WriteLine($"interprated game at {game.EndDate}");
            }
        }

    }
}
