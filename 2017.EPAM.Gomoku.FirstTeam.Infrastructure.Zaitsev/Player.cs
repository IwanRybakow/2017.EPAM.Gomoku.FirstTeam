using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using EPAM.TicTacToe;
using TicTakToe;
using TicTacToe;
using System.Threading.Tasks;

namespace _2017.EPAM.Gomoku.FirstTeam.Infrastructure.Zaitsev
{
    [TeamName("First team")]
    public class Player : IPlayer
    {
        WorkBoardClass WorkBoard { get; set; }  // Объект для определения рабочего поля 
        private Multithreading solver;          // Объект для вызова алгоритма в многопоточном режиме
        private Form1 GUI;                      // GUI для игры с человеком
        public int[,] Board { get; private set; } // локальное игровое поле ,предается на обратку алгоритму
        byte[] firstCoord;                      // координата первого хода, если я хожу первым
        bool firtStep;                          // флаг показывает сделан ли только что первый ход
        bool iMoveFirst;                        // флаг я хожу первым. Нужен для инициализации игрового поля.
        int playerID;                           // ID игрока 1 - Х, 2 - 0 нужно для GUI.
        bool isHuman;
        public Player()
        {
            firtStep = true;
            iMoveFirst = true;
            firstCoord = new byte[2] { 0, 0 };
        }

        // реализация интерфейса IPlayer
        public CellCoordinates NextMove(CellState.cellState[,] CurrentState, byte qtyCellsForWin, bool isHuman, TimeSpan remainingTimeForGame, int remainingQtyMovesForGame)
        {
            this.isHuman = isHuman;
            // инициализация локального игрового поля
            CreateLocalBoard(CurrentState, qtyCellsForWin, isHuman);

            // если играет человек
            if (isHuman)
            {
                // создаем GUI
                if (GUI == null)
                {
                    GUI = new Form1();
                    GUI.SetGUI(Board.GetLength(0), playerID);
                }
                // передаем GUI 
                GUI.GetBoard(Board);
                // ожидаем хода человека
                if (GUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return new CellCoordinates() { X = (byte)GUI.HumanMove[0], Y = (byte)GUI.HumanMove[1] };
                }
            }

            int[] myMove = { 0, 0 };
            // Создаем экземпляр рабочего поля
            if (WorkBoard == null)
            {
                WorkBoard = new WorkBoardClass(Board.GetLength(0));
            }

            // список координат рабочего поля
            List<int[]> workBoardCoords = WorkBoard.SetWorkBoard(Board);

            // если поле пустое, то сразу занимаем ячеку вблизи середины поля
            if (firstCoord[0] != 0)
            {
                byte[] temp = firstCoord;
                firstCoord = new byte[2] { 0, 0 };
                return new CellCoordinates() { X = temp[0], Y = temp[1] };

            }
            // Вызов алгоритма в многопоточном режиме
            myMove = solver.GetOptimalStep(Board, workBoardCoords);

            // создаем CellCoordinates и возвращаем наш ход
            return new CellCoordinates() { X = (byte)myMove[0], Y = (byte)myMove[1] };
        }

        // реализация интерфейса IPlayer
        public void RefreshUI(CellState.cellState[,] CurrentState)
        {
            if (isHuman)
            {
                CreateLocalBoard(CurrentState, 0, true);
                GUI.GetBoard(Board);
                GUI.ShowDialog();
            }
        }

        // Инициализация локального поля        
        private void CreateLocalBoard(CellState.cellState[,] currentState, byte qtyCellsForWin, bool isHuman)
        {
            // первый ход данного игрока в партии; да - создаем поле int[,], инициализируем алгоритм
            if (firtStep)
            {
                if (!isHuman)
                {
                    // если играет ИИ создаем экземляр для исполнения алгоритма ИИ в многопоточном режиме
                    solver = new Multithreading(qtyCellsForWin);
                }
                Board = new int[currentState.GetLength(0), currentState.GetLength(0)];
                playerID = 1;   // инизиализируем ID крестиком
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
                            playerID = 2;
                        }
                        // Инициализация локального поля
                        // первым хожу я 
                        if (iMoveFirst)
                        {
                            Board[i, j] = (int)currentState[i, j];
                        }
                        else
                        {
                            // Хожу вторым
                            // для человека
                            if (isHuman)
                            {
                                Board[i, j] = (int)currentState[i, j];
                            }
                            else
                            {
                                // Инициализация локального поля ИИ, если первым ходит соперник
                                // инвертируем значение ячейки. Необходимо для работы алгоритма
                                // Алгоритм вне зависимости от того, играем ли мы за крестик или нолик, считает идентификатором себя - 1
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