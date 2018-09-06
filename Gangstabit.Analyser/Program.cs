using System;
using System.IO;
using System.Threading.Tasks;
using Gangstabit.Business.Service;
using Gangstabit.DataAccess;

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
        }

    }
}
