using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleField;
using EPAM.TicTacToe;

namespace HumanPlayer
{
    public class HPlayer : IPlayer
    {
        public CellCoordinates NextMove(CellState.cellState[,] CurrentState, byte qtyCellsForWin, bool isHuman, TimeSpan remainingTimeForGame, int remainingQtyMovesForGame)
        {
            Console.WriteLine("Please enter coordinates of your next move according to followin pattern: number_of_row number_of_column (for example: 1 1) ");  
            bool ok;
            int r = 0;
            int c = 0;
            do
            {
                string move = Console.ReadLine();
                ok = false;
                try
                {
                    string[] coordinates = move.Split(' ');

                    r = Convert.ToInt32(coordinates[0]) - 1;
                    c = Convert.ToInt32(coordinates[1]) - 1; ;
                }
                catch (Exception)
                {
                    Console.WriteLine("You've entered wrong cell coordinates. Please try again");
                    ok = true;
                }
            } while (ok);


            return new CellCoordinates() { X = (byte)r, Y = (byte)c }; ;
        }
    }
}
