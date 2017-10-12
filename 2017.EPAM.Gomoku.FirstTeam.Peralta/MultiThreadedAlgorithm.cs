using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using _2017.EPAM.Gomoku.FirstTeam.Algorithm.Rybakov;

namespace TicTakToe
{
    public class Multithreading
    {
        private MiniMax algorithmMiniMax;
        public Multithreading(int signInRowToWin)
        {
            algorithmMiniMax = new MiniMax(signInRowToWin);
        }
        public int[] GetOptimalStep(int[,] playField, IEnumerable<int[]> CellsToCheck)
        {
            List<KeyValuePair<int[], int>> cellsToCheckList = algorithmMiniMax.ReduceMoves(playField, CellsToCheck);
            if (cellsToCheckList.Count == 1 )
            {
                return cellsToCheckList[0].Key;
            }
            HybridDictionary dictionary = new HybridDictionary();

            Parallel.ForEach(cellsToCheckList, element => dictionary.Add(element.Key, algorithmMiniMax.EvaluateCell(playField, element.Key)));

            int[] coordinateNextStep = new int[2];
            int temp = 0;

            foreach (DictionaryEntry d in dictionary)
            {
                if (Convert.ToInt32(d.Value) > temp)
                {
                    temp = Convert.ToInt32(d.Value);
                    coordinateNextStep = (int[])d.Key;
                }
            }
            return coordinateNextStep;

        }
    }
}
