using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2017.EPAM.Gomoku.FirstTeam.Infrastructure.Zaitsev
{
    static class CellsListExtenssions
    {
        public static int Min(this IEnumerable<int[]> list, int index)
        {
            int currentMin = list.ElementAt(0)[index];
            foreach(int[] item in list)
            {
                if(item[index] < currentMin)
                {
                    currentMin = item[index];
                }
            }
            return currentMin;
        }

        public static int Max(this IEnumerable<int[]> list, int index)
        {
            int currentMax = list.ElementAt(0)[index];
            foreach (int[] item in list)
            {
                if (item[index] > currentMax)
                {
                    currentMax = item[index];
                }
            }
            return currentMax;
        }
    }
}
