using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2017.EPAM.Gomoku.FirstTeam.Algorithm.Rybakov
{
    public class MiniMax
    {
        private int qtyCellsForWin;
        public PatternCollection ownPatterns;
        public PatternCollection opponentPatterns;

        public MiniMax(int qtyCellsForWin)
        {
            this.qtyCellsForWin = qtyCellsForWin;
            ownPatterns = new PatternCollection(qtyCellsForWin, 1);
            opponentPatterns = new PatternCollection(qtyCellsForWin, 2);
        }

        public int EvaluateCell (int[,] board, int[] move, int sign = 1, int depth = 2)
        {
            int result = Evaluate(board, move, sign);

            // base case
            if (result >= 20000000 || depth == 0)
            {
                return result;
            }

            int oppSign = sign == 1 ? 2 : 1;
            depth--;
            int[,] newBoard = (int[,])board.Clone();
            newBoard[move[0], move[1]] = sign;
            HashSet<int[]> cells = Utils.FindMoves(newBoard);
            Dictionary<int[], int> scores = new Dictionary<int[], int>();
            foreach (int[] cell in cells)
            {
                int[,] b = (int[,])newBoard.Clone();
                b[cell[0], cell[1]] = oppSign;
                int s = Evaluate(b, cell, oppSign);
                scores[cell] = s;
            }
            var items = from i in scores orderby i.Value descending select i.Key;
            List<int[]> cellsToCheckList = items.Take(2).ToList();

            int score = int.MinValue;
            foreach (int[] item in cellsToCheckList)
            {
                int temp = EvaluateCell(newBoard, item, oppSign, depth);
                if (temp > score)
                {
                    score = temp;
                }
            }
            return result - score;               
        }

        public List<KeyValuePair<int[], int>> ReduceMoves (int[,] board, IEnumerable<int[]> CellsToCheck)
        {
            Dictionary<int[], int> iscores = new Dictionary<int[], int>();
            foreach (int[] cell in CellsToCheck)
            {
                int[,] newBoard = (int[,])board.Clone();
                newBoard[cell[0], cell[1]] = 1;
                int estimation = Evaluate(newBoard, cell, 1);
                iscores[cell] = estimation;
            }
            var items = from i in iscores orderby i.Value descending select i;
            List<KeyValuePair<int[], int>> cellsToCheckList = items.Take(10).ToList();
            if (cellsToCheckList[0].Value > 430000)
            {
                return cellsToCheckList.Take(1).ToList();
            }
            return cellsToCheckList;
        }

        int Evaluate(int[,] board, int[] move, int sign)
        {
            int oppSign = sign == 1 ? 2 : 1;
            int result = 0;
            board[move[0], move[1]] = sign;
            List<string> StringsToCheck = Utils.FindStrings(board, move, qtyCellsForWin);
            board[move[0], move[1]] = oppSign;
            StringsToCheck.AddRange(Utils.FindStrings(board, move, qtyCellsForWin));
            board[move[0], move[1]] = 0;
            PatternCollection patterns = sign == 1 ? ownPatterns : opponentPatterns;
            foreach (string item in StringsToCheck)
            {
                foreach (Pattern pattern in patterns.PatternList)
                {
                    if (item.Contains(pattern.PatternString))
                    {
                        result += pattern.Weight;
                    }
                }
            }
            return result;
        }
    }
}
