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
            CellState.cellState[,] playBoard;
            int BoardSize = 15;
            byte lineqty = 5;
            Player player;
            Player hPlayer;
            playBoard = new CellState.cellState[BoardSize, BoardSize];            
            player = new Player();
            hPlayer = new Player();
            CellCoordinates coords;
            while(true)
            {
                coords = player.NextMove(playBoard, lineqty, false, new TimeSpan(0), 0);
                playBoard[coords.X, coords.Y] = CellState.cellState.X;
                Console.WriteLine("Player {0} moved to {1}, {2}", CellState.cellState.X, coords.X, coords.Y);
                coords = hPlayer.NextMove(playBoard, lineqty, true, new TimeSpan(0), 0);
                playBoard[coords.X, coords.Y] = CellState.cellState.O;
                Console.WriteLine("Player {0} moved to {1}, {2}", CellState.cellState.O, coords.X, coords.Y);

            }
        }
    }
}
