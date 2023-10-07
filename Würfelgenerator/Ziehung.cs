using System.Collections.Generic;

namespace Würfelgenerator
{
    internal class Ziehung : List<int>
    {
        public int Punktzahl
        {
            get
            {
                int erg = 0;
                for (int i = 0; i < Count; i++)
                    erg += this[i];
                return erg;
            }
        }

        public override string ToString()
        {
            string erg = string.Empty;
            for (int i = 0; i < Count; i++)
                erg += this[i] + " ";
            return erg;
        }
    }
}