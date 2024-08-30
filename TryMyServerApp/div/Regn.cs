using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryMyServerApp.div
{
    /// <summary>
    /// Denne klasse kan foretage forskellige regnearter
    /// </summary>
    public class Regn
    {

        /// <summary>
        /// dividerer to tal
        /// </summary>
        /// <param name="x">tal der divideres - tæller</param>
        /// <param name="y">tal der divideres op i tallet - nævner</param>
        /// <returns>resultatet af x / y</returns>
        /// <exception cref="DivideByZeroException">Hvis nævner er nul</exception>
        public int Div(int x, int y)
        {
            if (y == 0) throw new DivideByZeroException();

            return x / y;
        }
    }
}
