using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleField;
using HumanPlayer;
using EPAM.TicTacToe;
using _2017.EPAM.Gomoku.FirstTeam.Infrastructure.Zaitsev;

namespace Tictactoe
{
    class Program
    {
        static void Main(string[] args)
        {
            byte field = 15;
            byte win = 5;
            IPlayer p1;
            Console.WriteLine("Would you like to play 1 - yes:");
            int c;
            Int32.TryParse(Console.ReadLine(), out c);
            if (c == 1)
            {
                p1 = new HPlayer();
            }
            else
            {
                p1 = new Player();
            }

            IPlayer p2 = new Player();
            Game game = new Game(field, win, p1, p2);
            game.StartGame();
            Console.ReadKey();
        }
    }
}
