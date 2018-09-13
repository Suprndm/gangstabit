using System;
using System.Collections.Generic;
using System.Text;
using Gangstabit.Business.Model;

namespace Gangstabit.Business.Controllers
{
    public interface IController:IDisposable
    {
        Decision Play(IList<Game> previousGames);
        ControllerResults GetControllerResults();
    }
}
