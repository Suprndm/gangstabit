using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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


            var files = Directory.EnumerateFiles(
                @"C:\Gangstabit",
                "*",
                SearchOption.AllDirectories).ToList();

            int count = 0;
            foreach (string file in files)
            {
                try
                {
                    var logReader = new System.IO.StreamReader(file);
                    var html = await logReader.ReadToEndAsync();
                    html = HttpUtility.HtmlDecode(html);
                    var game = interpretor.InterpreteGameFromHtml(html, file);
                    logReader.Dispose();

                    Console.WriteLine($"{file} interpreted");
                    await gameRepository.SaveGameAsync(game);
                    Console.WriteLine($"{file} saved");

                    count++;
                    Console.WriteLine($"{count} / {files.Count()} {(double)count/ files.Count*100}%");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
        }

    }
}
