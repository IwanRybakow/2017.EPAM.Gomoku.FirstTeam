using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2017.EPAM.Gomoku.FirstTeam.Algorithm.Rybakov
{
    public static class Utils
    {
        /*search for possible moves near cells which are already taken*/

        public static HashSet<Tuple<int, int>> FindMoves (int[,] board)
        {
            int searchRange = 2;
            int bLength = board.GetLength(0);
            HashSet<Tuple<int, int>> pointsList = new HashSet<Tuple<int, int>>();

            for (int i = 0; i < bLength; i++)
            {
                for (int j = 0; j < bLength; j++)
                {
                    if (board[i,j] == 0)
                    {
                        continue;
                    }
                    int x0 = Math.Max(0, i - searchRange), x1 = Math.Min(bLength - 1, i + searchRange);
                    int y0 = Math.Max(0, j - searchRange), y1 = Math.Min(bLength - 1, j + searchRange);
                    for (int k = x0; k <= x1; k++)
                    {
                        for (int l = y0; l <= y1; l++)
                        {
                            if (board[k,l] == 0)
                            {
                                pointsList.Add(new Tuple<int, int>(item1:k, item2:l));
                            }
                        }
                    }
                }
            }
            return pointsList;
        }

        /* 
         *  search for strings near move(in center),
         *        x    x    x
         *          x  x  x
         *           x x x
         *       x x x x x x x
         *           x x x
         *          x  x  x
         *        x    x    x
         *  creates 4 strins: horizontal, vertical, 2 diagonals
         *  then algorithm looks for patterns in this strings 
         */

        public static List<string> FindStrings(int[,] board, int[] move, int signsInRowToWin)
        {
            List<string> resultList = new List<string>();

            StringBuilder horizontalLine = new StringBuilder();
            int startH = move[1] - (signsInRowToWin - 1) < 0 ? 0 : move[1] - (signsInRowToWin - 1);
            int finishH = move[1] + signsInRowToWin > board.GetLength(1) ? board.GetLength(1) : move[1] + signsInRowToWin;
            for (int i = startH; i < finishH; i++)
            {
                horizontalLine.Append(board[move[0], i].ToString());
            }
            resultList.Add(horizontalLine.ToString());

            StringBuilder verticalLine = new StringBuilder();
            int startV = move[0] - signsInRowToWin < 0 ? 0 : move[0] - signsInRowToWin;
            int finishV = move[0] + signsInRowToWin > board.GetLength(1) ? board.GetLength(1) : move[0] + signsInRowToWin;
            for (int i = startV; i < finishV; i++)
            {
                verticalLine.Append(board[i, move[1]].ToString());
            }
            resultList.Add(verticalLine.ToString());


            StringBuilder ForwardDiagonal = new StringBuilder();
            int startFwdRow = move[0] + (signsInRowToWin - 1);
            int startFwdColumn = move[1] - (signsInRowToWin - 1);
            while (startFwdRow > board.GetLength(1) - 1 || startFwdColumn < 0)
            {
                startFwdRow--;
                startFwdColumn++;
            }
            int finishFwdRow = move[0] - (signsInRowToWin - 1);
            int finishFwdColumn = move[1] + signsInRowToWin;
            while (finishFwdColumn > board.GetLength(1) || finishFwdRow < 0)
            {
                finishFwdRow++;
                finishFwdColumn--;
            }
            for (int i = startFwdColumn; i < finishFwdColumn; i++)
            {
                ForwardDiagonal.Append(board[startFwdRow, i].ToString());
                startFwdRow--;
            }
            if (ForwardDiagonal.Length >= signsInRowToWin)
            {
                resultList.Add(ForwardDiagonal.ToString());
            }


            StringBuilder BackwardDiagonal = new StringBuilder();
            int startBwdRow = move[0] - (signsInRowToWin - 1);
            int startBwdColumn = move[1] - (signsInRowToWin - 1);
            while (startBwdRow < 0 || startBwdColumn < 0)
            {
                startBwdRow++;
                startBwdColumn++;
            }
            int finisBwdRow = move[0] + signsInRowToWin;
            int finishBwdColumn = move[1] + signsInRowToWin;
            while (finishBwdColumn > board.GetLength(1) || finisBwdRow > board.GetLength(1))
            {
                finisBwdRow--;
                finishBwdColumn--;
            }
            for (int i = startBwdColumn; i < finishBwdColumn; i++)
            {
                BackwardDiagonal.Append(board[startBwdRow, i].ToString());
                startBwdRow++;
            }
            if (BackwardDiagonal.Length >= signsInRowToWin)
            {
                resultList.Add(BackwardDiagonal.ToString());
            }

            return resultList;
        }
    }
}
