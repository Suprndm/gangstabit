using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Gangstabit.Business.Ports;
using Gangstabit.Business.Service;
using Microsoft.AspNetCore.Mvc;

namespace Gangstabit.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly GameInterpretor _gameInterpretor;


        public GameController(IGameRepository gameRepository, GameInterpretor gameInterpretor)
        {
            _gameRepository = gameRepository;
            _gameInterpretor = gameInterpretor;
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]string value)
        {
            value = value.ToHtml();
            var logPath = $@"C:\Gangstabit\{DateTime.UtcNow.Ticks}.html";
            var logFile = System.IO.File.Create(logPath);
            var logWriter = new System.IO.StreamWriter(logFile);
            logWriter.WriteLine(value);
            logWriter.Dispose();

            value = HttpUtility.HtmlDecode(value);
            var game = _gameInterpretor.InterpreteGameFromHtml(value, logPath);
            await _gameRepository.SaveGameAsync(game).ConfigureAwait(false);
        }
    }
}
