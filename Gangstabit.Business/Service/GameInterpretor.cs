using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Gangstabit.Business.Model;

namespace Gangstabit.Business.Service
{
    public class GameInterpretor
    {
        public Game InterpreteGameFromHtml(string html, string fileName)
        {
            try
            {
                // Date
                var split1 = fileName.Split('\\');
                var split2 = split1.Last().Split('.');
                var endDate = new DateTime(long.Parse(split2.First()));

                // Multiplier
                var split3 = html.Split(new string[]{"\" style=\"color: rgb(46, 204, 113);\">"}, StringSplitOptions.None);
                var split4 = split3[1].Split(new string[] { "</a></td>" }, StringSplitOptions.None);
                var truncated1 = split4[0].Remove(split4[0].Length - 1);
                var multiplier = double.Parse(truncated1, CultureInfo.InvariantCulture);
                var htmlSplit = html.Split('\r');

                return new Game() { EndDate = endDate };
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to parse game date from File");
                return null;
            }


        }
    }
}
