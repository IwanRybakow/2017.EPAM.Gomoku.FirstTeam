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
        public PatternCollection ownPatterns; // patterns for this algorithm
        public PatternCollection opponentPatterns; // patterns for opponent

        public MiniMax(int qtyCellsForWin)
        {
            this.qtyCellsForWin = qtyCellsForWin;
            ownPatterns = new PatternCollection(qtyCellsForWin, 1);
            opponentPatterns = new PatternCollection(qtyCellsForWin, 2);
        }

        public int EvaluateCell (int[,] board, Tuple<int, int> move, int sign = 1, int depth = 6)
        {
            int result = Evaluate(board, move, sign);

            // base case
            if (result >= 2000000 || depth == 0)
            {
                return result;
            }

            int oppSign = sign == 1 ? 2 : 1;
            depth--;
            int[,] newBoard = (int[,])board.Clone();
            newBoard[move.Item1, move.Item2] = sign;
            HashSet<Tuple<int, int>> cells = Utils.FindMoves(newBoard);
            Dictionary<Tuple<int, int>, int> scores = new Dictionary<Tuple<int, int>, int>();
            foreach (Tuple<int, int> cell in cells)
            {
                int[,] b = (int[,])newBoard.Clone();
                b[cell.Item1, cell.Item2] = oppSign;
                int s = Evaluate(b, cell, oppSign);
                scores[cell] = s;
            }
            var items = from i in scores orderby i.Value descending select i.Key;
            List<Tuple<int, int>> cellsToCheckList = items.Take(2).ToList(); // take only two top results for further processing

            int score = int.MinValue;
            foreach (Tuple<int, int> item in cellsToCheckList)
            {
                int temp = EvaluateCell(newBoard, item, oppSign, depth);
                if (temp > score)
                {
                    score = temp;
                }
            }
            return result - score; //substruct best next move of opponent from our               
        }

        //reduces quantity of moves for deep analysis
        public List<KeyValuePair<Tuple<int, int>, int>> ReduceMoves (int[,] board, IEnumerable<Tuple<int, int>> CellsToCheck)
        {
            Dictionary<Tuple<int, int>, int> iscores = new Dictionary<Tuple<int, int>, int>();
            foreach (Tuple<int, int> cell in CellsToCheck)
            {
                int[,] newBoard = (int[,])board.Clone();
                newBoard[cell.Item1, cell.Item2] = 1;
                int estimation = Evaluate(newBoard, cell, 1);
                iscores[cell] = estimation;
            }
            var items = from i in iscores orderby i.Value descending select i;
            List<KeyValuePair<Tuple<int, int>, int>> cellsToCheckList = items.Take(10).ToList();
            if (cellsToCheckList[0].Value > 430000) //urgently react to most obvious moves
            {
                return cellsToCheckList.Take(1).ToList();
            }
            return cellsToCheckList;
        }

        // returns estimation for move
        int Evaluate(int[,] board, Tuple<int, int> move, int sign)
        {
            int oppSign = sign == 1 ? 2 : 1;
            int result = 0;
            board[move.Item1, move.Item2] = sign;
            List<string> StringsToCheck = Utils.FindStrings(board, new int[] { move.Item1, move.Item2 }, qtyCellsForWin);
            board[move.Item1, move.Item2] = oppSign;
            StringsToCheck.AddRange(Utils.FindStrings(board, new int[] { move.Item1, move.Item2 }, qtyCellsForWin));
            board[move.Item1, move.Item2] = 0;
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
