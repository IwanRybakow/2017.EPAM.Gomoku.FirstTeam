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
            HashSet<Tuple<int, int>> NearCellsList = Utils.FindMoves(playField);
            foreach (int[] item in CellsToCheck)
            {
                NearCellsList.Add(new Tuple<int, int>(item[0], item[1]));
            }
            List<KeyValuePair<Tuple<int, int>, int>> cellsToCheckList = algorithmMiniMax.ReduceMoves(playField, NearCellsList);
            if (cellsToCheckList.Count == 1)
            {
                Tuple<int, int> res = cellsToCheckList[0].Key;
                return new int[]{ res.Item1, res.Item2 };
            }
            HybridDictionary dictionary = new HybridDictionary();

            Parallel.ForEach(cellsToCheckList, element => dictionary.Add(element.Key, algorithmMiniMax.EvaluateCell((int[,])playField.Clone(), element.Key)));

            int[] coordinateNextStep = new int[2] { cellsToCheckList[0].Key.Item1, cellsToCheckList[0].Key.Item2 };
            int temp = int.MinValue;

            foreach (DictionaryEntry d in dictionary)
            {
                if (Convert.ToInt32(d.Value) > temp)
                {
                    temp = Convert.ToInt32(d.Value);
                    Tuple<int, int> res = (Tuple<int, int>)d.Key;
                    coordinateNextStep = new int[] { res.Item1, res.Item2 };
                }
            }
            return coordinateNextStep;

        }
    }
}
