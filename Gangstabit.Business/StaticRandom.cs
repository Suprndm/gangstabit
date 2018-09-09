using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Gangstabit.Business
{
    public class StaticRandom
    {
        static int seed = Environment.TickCount;

        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static int Rand(int i)
        {
            return random.Value.Next(i);
        }
    }
}
