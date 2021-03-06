﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Gangstabit.Business.Model;

namespace Gangstabit.Business.Ports
{
    public interface IGameRepository
    {
        Task SaveGameAsync(Game game);
        Task<Analysis> GetAnalysisAsync();
    }
}
