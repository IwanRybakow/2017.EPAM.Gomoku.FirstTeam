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
            byte r = 0;
            byte c = 0;
            do
            {
                string move = Console.ReadLine();
                ok = true;
                try
                {
                    string[] coordinates = move.Split(' ');

                    r = Convert.ToByte(coordinates[0]);
                    c = Convert.ToByte(coordinates[1]);
                }
                catch (Exception)
                {
                    Console.WriteLine("You've entered wrong cell coordinates. Please try again");
                    ok = false;
                }
            } while (ok);


            return new CellCoordinates() { X = r, Y = c }; ;
        }
    }
}
