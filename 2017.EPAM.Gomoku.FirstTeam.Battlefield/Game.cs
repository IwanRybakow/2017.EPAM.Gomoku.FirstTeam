using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM.TicTacToe;

namespace BattleField
{
    public class Game
    {
        private int fieldSize;
        private CellState.cellState[,] board;
        private byte goal;
        private IPlayer[] PlayersArray;
        private int movesCount = 0;

        public Game(int size, byte max, IPlayer first, IPlayer second)
        {
            fieldSize = size;
            goal = max;
            PlayersArray = new IPlayer[] { first, second };
            board = new CellState.cellState [size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = CellState.cellState.Empty;
                }
            }
        }

        public void StartGame()
        {
            int currentPlayer = 0;
            CellState.cellState id = CellState.cellState.X;
            CellCoordinates move;
            while (true)
            {
                try
                {
                    move = AddTurn(PlayersArray[currentPlayer], id);
                    PrintState();
                }
                catch (CellNotEmptyException)
                {
                    Console.WriteLine("This cell is already taken. Player {0} lost.", id);
                    return;
                }
                //catch (ArgumentOutOfRangeException)
                //{
                //    Console.WriteLine("Turn is out of range of the field. Player {0} lost.", id);
                //    return;
                //}
                if (checkResult(id, move))
                {
                    Console.WriteLine("Game is over. Player {0} won.", id);
                    return;
                }
                else if (movesCount == fieldSize*fieldSize) //if number of moves equals to the number of cells - there is no more empty cells - it's a tie
                {
                    Console.WriteLine("It's a tie!");
                    return;
                }
                else
                {
                    currentPlayer = (currentPlayer == 0) ? 1 : 0; // change player
                    id = id == CellState.cellState.X ? CellState.cellState.O : CellState.cellState.X;
                }

            }
        }

        private bool checkResult(CellState.cellState id, CellCoordinates move)
        {
            if (checkBackwardDiagonal(id, new byte[] {move.X, move.Y }) || checkForwardDiagonal(id, new byte[] { move.X, move.Y }) || checkHorizntal(id, move.X) || checkVertical(id, move.Y))
            {
                return true;
            }
            return false;
        }

        private bool checkHorizntal(CellState.cellState id, byte row)
        {
            //check only row of last turn
            int sameInRowCount = 0;
            for (int i = 0; i < fieldSize - (goal - 1); i++)
            {
                while (board[row, i] == id)
                {
                    i++;
                    sameInRowCount++;
                    if (sameInRowCount == goal)
                    {
                        return true;
                    }
                }
                sameInRowCount = 0;
            }
            return false;
        }
        private bool checkVertical(CellState.cellState id, byte column)
        {
            //check only column of last turn
            int sameInRowCount = 0;
            for (int i = 0; i < fieldSize - (goal - 1); i++)
            {
                while (board[i, column] == id)
                {
                    i++;
                    sameInRowCount++;
                    if (sameInRowCount == goal)
                    {
                        return true;
                    }
                }
                sameInRowCount = 0;
            }
            return false;
        }
        private bool checkForwardDiagonal(CellState.cellState id, byte[] move)
        {
            //check only diagonal of last turn, looking top left to bottom right
            int min = move.Min();
            //define starting poit
            int row = move[0] - min;
            int column = move[1] - min;
            int numOfCells = fieldSize - column - row;
            if (numOfCells < goal) // check if diagonal is long enough
            {
                return false;
            }
            int sameInRowCount = 0;
            for (int i = 0; i < numOfCells - (goal-1); i++)
            {
                while (board[row, column] == id)
                {
                    i++;
                    row++;
                    column++;
                    sameInRowCount++;
                    if (sameInRowCount == goal)
                    {
                        return true;
                    }
                }
                row++;
                column++;
                sameInRowCount = 0;                
            }
            return false;
        }
        private bool checkBackwardDiagonal(CellState.cellState id, byte[] move)
        {
            //check only diagonal of last turn, looking bottom left to top right
            //define starting poit
            int row = move[0];
            int column = move[1];
            while (row < fieldSize -1 && column > 0)
            {
                row++;
                column--;
            }
            int numOfCells = row - column + 1;
            if (numOfCells < goal) // check if diagonal is long enough
            {
                return false;
            }
            int sameInRowCount = 0;
            for (int i = 0; i < numOfCells - (goal - 1); i++)
            {
                while (board[row, column] == id)
                {
                    i++;
                    row--;
                    column++;
                    sameInRowCount++;
                    if (sameInRowCount == goal)
                    {
                        return true;
                    }
                }
                row--;
                column++;
                sameInRowCount = 0;
                
            }
            return false;
        }

        public CellCoordinates AddTurn(IPlayer player, CellState.cellState id)
        {
            CellCoordinates newTurn = player.NextMove(board, goal, false, new TimeSpan(), 0);
            CellState.cellState currentValueOfCell = board[newTurn.X, newTurn.Y];

            if (currentValueOfCell != CellState.cellState.Empty)
            {
                Console.WriteLine("Cell {0}{1} is taken", newTurn.X, newTurn.Y);
                throw new CellNotEmptyException();
            }
            else
            {
                board[newTurn.X, newTurn.Y] = id;
                movesCount++;
                return newTurn;
            }
        }

        public void PrintState()
        {
            StringBuilder border = new StringBuilder();
            border.Append('-', fieldSize);
            Console.WriteLine("");
            //Console.Clear();
            Console.Write("  |");
            for (int i = 0; i < fieldSize; i++)
            {
                Console.Write("{0, 2}|",i+1);
                
            }
            Console.WriteLine("");
            for (int i = 0; i < fieldSize; i++)
            {
                Console.Write("{0, 2}", i + 1);
                Console.Write("|");
                for (int j = 0; j < fieldSize; j++)
                {
                    if (board[i, j] == CellState.cellState.Empty)
                    {
                        Console.Write("{0, 3}", "|");
                    }
                    else if (board[i, j] == CellState.cellState.X)
                    {
                        Console.Write("{0, 3}", "X|");
                    }
                    else if (board[i, j] == CellState.cellState.O)
                    {
                        Console.Write("{0, 3}", "0|");
                    }
                }
                Console.WriteLine("");
            }
        }
    }
}
