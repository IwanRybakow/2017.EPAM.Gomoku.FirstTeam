using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM.TicTacToe;
using _2017.EPAM.Gomoku.FirstTeam.Infrastructure.Zaitsev;

namespace TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            CellState.cellState[,] board = new CellState.cellState[15, 15];
            byte line = 5;
            Player player = new Player();
            Player hPlayer = new Player();

            CellCoordinates coords;
            while(true)
            {
                coords = player.NextMove(board, line, false, new TimeSpan(0), 0);
                board[coords.X, coords.Y] = CellState.cellState.X;
                Console.WriteLine("Player {0} moved to {1},{2}", CellState.cellState.X, coords.X, coords.Y);
                coords = hPlayer.NextMove(board, line, true, new TimeSpan(0), 0);
                board[coords.X, coords.Y] = CellState.cellState.O;
                Console.WriteLine("Player {0} moved to {1},{2}", CellState.cellState.O, coords.X, coords.Y);

            }
        }
    }
}
