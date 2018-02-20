using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    class ExceptionHelper
    {
        public void PutMessage(string AddInfo, Exception ex)
        {
            Console.WriteLine("Error occurred in " + AddInfo + ": " + ex.Message + " " + ex.TargetSite);
            Console.ReadLine();
        }
    }
}
