using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPAM.TicTacToe;
using TicTakToe;
using _2017.EPAM.Gomoku.FirstTeam.Algorithm.Rybakov;
{
namespace _2017.EPAM.Gomoku.FirstTeam.Infrastructure.Zaitsev
{
    [TeamName("First team")]
    public class Player : IPlayer
    {
        /// <summary>
        /// Объект для определения рабочего поля 
        /// </summary>
        WorkBoardClass WorkBoard { get; set; }
        /// <summary>
        /// локальное игровое поле ,предается на обратку алгоритму
        /// </summary>
        private Multithreading solver;//Ivan
        public int[,] Board { get; private set; }
        byte[] firstCoord;  // координата первого хода, если я хожу первым
        bool firtStep;   // флаг показывает сделан ли только что первый ход
        bool iMoveFirst; // флаг я хожу первым. Нужен для инициализации игрового поля. Я всегда - 1

        public Player()
        {
            firtStep = true;
            iMoveFirst = true;
            firstCoord = new byte[2] { 0, 0 };
        }

        // реализация интерфейса IPlayer
        public CellCoordinates NextMove(CellState.cellState[,] CurrentState, byte qtyCellsForWin, bool isHuman, TimeSpan remainingTimeForGame, int remainingQtyMovesForGame)
        {
            // инициализация локального игрового поля
            CreateLocalBoard(CurrentState, qtyCellsForWin);

            int[] myMove = { 0, 0 };
            //if (WorkBoard == null)
            //{
            //    WorkBoard = new WorkBoardClass(Board.GetLength(0));
            //}
            
            // список координат рабочего поля
            //List<int[]> workBoardCoords = WorkBoard.SetWorkBoard(Board);

            // если поле пустое, сразу занимаем ячеку вблизи середины поля
            if (firstCoord[0] != 0)
            {
                byte[] temp = firstCoord;
                firstCoord = new byte[2] { 0, 0 };
                return new CellCoordinates() { X = temp[0], Y = temp[1] };
                
            }
            // Вызов алгоритма в многопоточном режиме
            //myMove = solver.GetOptimalStep(Board, workBoardCoords);
            myMove = solver.GetOptimalStep(Board, Utils.FindMoves(Board));

            // создаем CellCoordinates и возвращаем наш ход
            return new CellCoordinates() { X = (byte)myMove[0], Y = (byte)myMove[1] };
        }

        /// <summary>
        //// Инициализация локального поля
        /// </summary>
        /// <param name="currentState">текущее состояние игрового поля</param>
        private void CreateLocalBoard(CellState.cellState[,] currentState, byte qtyCellsForWin)
        {
            // первый ход в игре; да - создаем поле int[,], инициализируем алгоритм
            if (firtStep)
            {
                solver = new Multithreading(qtyCellsForWin); //Ivan
                Board = new int[currentState.GetLength(0), currentState.GetLength(0)];
            }

            // проходим по всему полю
            for (int i = 0; i < currentState.GetLength(0); i++)
            {
                for (int j = 0; j < currentState.GetLength(0); j++)
                {
                    // ячейка поля отличается от ячейки локального поля
                    if ((int)currentState[i, j] != Board[i, j])
                    {
                        if (firtStep)
                        {
                            // на первом ходе уже есть заполненная ячейка.
                            // Значит первым ходит соперник
                            iMoveFirst = false;
                        }
                        // Инициализация локального поля, если первым хожу я 
                        if (iMoveFirst)
                        {
                            Board[i, j] = (int)currentState[i, j];
                        }
                        else
                        {
                            // Инициализация локального поля, если первым ходит соперник
                            // инвертируем знначение ячейки
                            switch (currentState[i, j])
                            {
                                case CellState.cellState.X:
                                    Board[i, j] = 2;
                                    break;
                                case CellState.cellState.O:
                                    Board[i, j] = 1;
                                    break;
                            }

                        }
                    }
                }
            }
            // если поле пустое и я хожу первым, занимаем ячеку где-то в центре поля
            if (iMoveFirst && firtStep)
            {
                firstCoord[0] = (byte)(Board.GetLength(0) / 2);
                firstCoord[1] = (byte)(Board.GetLength(0) / 2);
            }
            firtStep = false;
        }
    }
}