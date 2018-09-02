using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gangstabit.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            value = value.ToHtml();
            var logPath = $@"C:\Gangstabit\{DateTime.UtcNow.Ticks}.html";
            var logFile = System.IO.File.Create(logPath);
            var logWriter = new System.IO.StreamWriter(logFile);
            logWriter.WriteLine(value);
            logWriter.Dispose();
        }
    }
}
