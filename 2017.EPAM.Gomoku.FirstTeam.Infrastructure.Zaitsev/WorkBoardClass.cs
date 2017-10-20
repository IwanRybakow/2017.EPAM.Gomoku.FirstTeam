using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2017.EPAM.Gomoku.FirstTeam.Infrastructure.Zaitsev
{
    /// <summary>
    /// Класс для усечения игрового поля. Определяется рабочее область
    /// </summary>
    public class WorkBoardClass
    {
        // Массив с координатами рабочей области.
        // 0 - левая граница (x), 1 - верхняя граница (у)
        // 2 - правая граница (х), 3 - нижняя граница (у)
        int[] BoundsOfWorkBoard { get; set; }
        List<int[]> dangerousCellsList;             // список опасных ячейки
        const double MAX_DISTANCE = 2.82;       // максимальное расстояние от рабочей области (корень из 8-ми)
        private const int MIN_QUANTITY_EMPTY_CELLS = 4; // минимальное количество сводных ячеек в рабочей области
        int size;   //размеры игрового поля
        bool workBoardNeed;     // а нужно ли рабочеее поле, если размер игрового поля мал      
        int initSize = 1;       // размер, на который устанавливается или расширяется рабочая область
        List<int[]> outOfBoundsList;
        int minimumWorkBoardSize = 7;
        // конструктор. Инициализирует границы рабочего поля
        // <param name="size"> размер игрового поля </param>
        // <param name="firstMove"> первый ход в игре. вокруг него строится игровое поле</param>
        public WorkBoardClass(int size, int[] firstMove) 
        {
            this.size = size;
            if (size < minimumWorkBoardSize)
            {
                workBoardNeed = false;
                SetDefaultBounds();
            }
            else
            {
                workBoardNeed = true;
                outOfBoundsList = new List<int[]>();
                InitBoundsOfWorkBoard(firstMove);
            }

        }

        // установка границ по умолчанию - в размер игрового поля
        private void SetDefaultBounds()
        {
            BoundsOfWorkBoard[0] = BoundsOfWorkBoard[1] = 0;
            BoundsOfWorkBoard[2] = BoundsOfWorkBoard[3] = size - 1;
        }

        // инициализирует начальное рабочую область
        private void InitBoundsOfWorkBoard(int[] firstMove)
        {            
            BoundsOfWorkBoard = new int[4];
            BoundsOfWorkBoard[0] = firstMove[1] - initSize;
            BoundsOfWorkBoard[1] = firstMove[0] - initSize;
            BoundsOfWorkBoard[2] = firstMove[1] + initSize;
            BoundsOfWorkBoard[3] = firstMove[0] + initSize;
            CheckBounds();
        }

        // проверка границ рабочей области за выход границ игрового поля
        void CheckBounds()
        {
            for(int i = 0; i < 4; i++)
            {
                if(BoundsOfWorkBoard[i] < 0)
                {
                    BoundsOfWorkBoard[i] = 0;
                }
                else if (BoundsOfWorkBoard[i] > size)
                {
                    BoundsOfWorkBoard[i] = size;
                }
            }
        }

        // Установка новой рабочей области, возвращает координаты внутри рабочей обаласти
        // <param name="newBoard">массив новго поля</param>
        public List<int[]> SetWorkBoard(int[,] newBoard, List<int[]> newMovesList)
        {
            List<int[]> coords;
            // если рабочее поле не требуется, возвращаем координаты
            if (!workBoardNeed)
            {
                return coords = GetCoordsListInWorkBoard(newBoard);
            }
            try
            {
                // находим ячейки за пределами рабочего поля
                outOfBoundsList.AddRange(FindOutOfBoundCells(newMovesList));
                // находим опасные ячейки
                dangerousCellsList = FindDangerousCells();
                // определяем новые границы рабочего поля
                NewBoundsOfWorkBoard();
            }
            catch (ArgumentNullException)
            {
                // либо нет ячеек за пределами рабочего поля, либо не найдены опасные
            }
            finally
            {
                // Составляем список координат ячеек в рабочей области
                coords = GetCoordsListInWorkBoard(newBoard);
            }
            // если свободных ячеек маало расширяем границы рабочей области
            if (coords.Count <= MIN_QUANTITY_EMPTY_CELLS)
            {
                coords = ExpandBoundsOfWorkBoard(newBoard);
            }
            return coords;


        }

        // расширяет границы рабочей области
        private List<int[]> ExpandBoundsOfWorkBoard(int[,] newBoard)
        {
            // расширяем границы рабочей области во все стороны
            BoundsOfWorkBoard[0] -= initSize;
            BoundsOfWorkBoard[1] -= initSize;
            BoundsOfWorkBoard[2] += initSize;
            BoundsOfWorkBoard[3] += initSize;
            // проверяем границы рабочей области за выход из пределов игрового поля
            CheckBounds();
            return GetCoordsListInWorkBoard(newBoard);
        }

        
        // находит ячеки за пределами рабочей области
        // <param name="newBoard">игровое поле </param>        
        private List<int[]> FindOutOfBoundCells(List<int[]> cellList)
        {
            // список ячеек за пределами рабочей области
            List<int[]> outOfBound = new List<int[]>();

            // среди новых ходов определяем те, что за пределами действующей рабочей области
            foreach (int[] cell in cellList)
            {
                if (!(cell[0] >= BoundsOfWorkBoard[1] && cell[1] >= BoundsOfWorkBoard[0] &&
                      cell[0] <= BoundsOfWorkBoard[3] && cell[1] <= BoundsOfWorkBoard[2]))
                {
                    outOfBound.Add(cell);
                }
            }
            
            // Если все ячейки внутри рабочей области
            if (outOfBound.Count == 0)
            {
                return null;
            }
            return outOfBound;
        }

        // поиск опасных ячеек за пределами рабочей области
        private List<int[]> FindDangerousCells()
        {
            double? distance;
            List<int[]> dangerousCells = new List<int[]>();   // спиок близкорасположенных ячеек (наиболее опасные)
            List<int[]> distant = new List<int[]>(); // список удаленных (далеко) ячеек от рабочей области

            foreach (int[] cell in outOfBoundsList)
            {
                // если ячейка в области "креста"
                distance = IsInAreaDistance(cell);
                if (distance == null)
                {
                    distance = DistanceToCorner(cell);
                }
                // определяем бликолежащие (наиболее опасные) и удаленные ячейки
                if (distance <= MAX_DISTANCE)
                {
                    dangerousCells.Add(cell);
                }
                else
                {
                    distant.Add(cell);
                }
            }

            // определяем близкие к друг к другу (потенциально опасные) удаленные ячейки
            dangerousCells.AddRange(FindCloseCells(distant, distant));

            // определяем близкие друг к другу ячейки из удаленных и близколежащих
            dangerousCells.AddRange(FindCloseCells(distant, dangerousCells));

            // ячейки повторяются, но все работает без потери производительности
            //List<int[]> distinctDangerousCells = dangerousCells.Distinct().ToList();
            return dangerousCells;
        }


        // Создает список координат свободных ячеек в рабочей области        
        private List<int[]> GetCoordsListInWorkBoard(int[,] newBoard)
        {
            List<int[]> coords = new List<int[]>();
            for (int i = BoundsOfWorkBoard[1]; i <= BoundsOfWorkBoard[3]; i++)
            {
                for (int j = BoundsOfWorkBoard[0]; j <= BoundsOfWorkBoard[2]; j++)
                {
                    // Если ячейка пуста, то добавляем ее в список
                    if (newBoard[i, j] == 0)
                    {
                        // удаляем ячейку из списка "за пределами рабочей области" (за бугром :-) )
                        outOfBoundsList.RemoveAll(x => (x[0] == i && x[1] == j));
                        // вносим в список координат
                        coords.Add(new int[] { i, j });
                    }
                }
            }
            return coords;
        }

        // поиск близких к друг другу (опасных) ячеек
        private List<int[]> FindCloseCells(List<int[]> cellList, List<int[]> anotherCellList)
        {
            double distance;
            List<int[]> result = new List<int[]>();
            foreach (int[] cell in cellList)
            {
                foreach (int[] anotherCell in anotherCellList)
                {
                    // Вычисляем расстоние между ячейками
                    distance = DistancePointToPoint(cell, anotherCell);
                    if (cell != anotherCell && distance <= MAX_DISTANCE && !result.Contains(cell))
                    {
                        result.Add(cell);
                    }
                }
            }
            return result;
        }

        // вычисляет расстояние между двумя точками. Координаты точек - массивы
        private double DistancePointToPoint(int[] p1, int[] p2)
        {
            return Math.Sqrt(Math.Pow(p1[0] - p2[0], 2) + Math.Pow(p1[1] - p2[1], 2));
        }

        // вычисляет расстояние между двумя точками. Координаты точек - массив и отдельно X и Y
        private double DistancePointToPoint(int[] p1, int p2Coord_X, int p2Coord_Y)
        {
            return Math.Sqrt(Math.Pow(p1[1] - p2Coord_X, 2) + Math.Pow(p1[0] - p2Coord_Y, 2));
        }

        // наикротчайшее расстояние от точки до угла рабочей области 
        private double? DistanceToCorner(int[] cell)
        {
            double[] distance = new double[4];
            // левый верх
            distance[0] = DistancePointToPoint(cell, BoundsOfWorkBoard[0], BoundsOfWorkBoard[1]);
            // правый верх
            distance[1] = DistancePointToPoint(cell, BoundsOfWorkBoard[2], BoundsOfWorkBoard[1]);
            // левый низ
            distance[2] = DistancePointToPoint(cell, BoundsOfWorkBoard[0], BoundsOfWorkBoard[3]);
            // правый низ
            distance[3] = DistancePointToPoint(cell, BoundsOfWorkBoard[2], BoundsOfWorkBoard[3]);

            return distance.Min();
        }

        // находит расстояние от ячеек располженных в области "креста" расбочей области
        private double? IsInAreaDistance(int[] cell)
        {
            // ячейка сверху или снизу рабочей области 
            if (cell[1] > BoundsOfWorkBoard[0] && cell[1] < BoundsOfWorkBoard[2])
            {
                return DistanceToLinePerpendicular(cell[0], BoundsOfWorkBoard[1], BoundsOfWorkBoard[3]);
            }
            // ячейка слева или справа от рабочей области
            else if (cell[0] > BoundsOfWorkBoard[1] && cell[0] < BoundsOfWorkBoard[3])
            {
                return DistanceToLinePerpendicular(cell[1], BoundsOfWorkBoard[0], BoundsOfWorkBoard[2]);
            }
            return null;
        }

        // расстояние от точки до отрезка по перпендикуляру
        private double DistanceToLinePerpendicular(int cellCoord, int coordLine1, int coordLine2)
        {
            double[] distance = new double[2];
            distance[0] = Math.Abs(coordLine1 - cellCoord);
            distance[1] = Math.Abs(coordLine2 - cellCoord);
            return distance.Min();
        }

        // новые границы 
        private void NewBoundsOfWorkBoard()
        {
            // граница слева
            GetMininimumCoordOutOfBound(1, 0);

            // граница справа
            GetMaximumCoordOutOfBound(1, 2);

            // граница сверху
            GetMininimumCoordOutOfBound(0, 1);

            // граница снизу
            GetMaximumCoordOutOfBound(0, 3);

        }

        // Поиск минимальной координаты
        private void GetMininimumCoordOutOfBound(int cellIndex, int boundIndex)
        {
            try
            {
                IEnumerable<int[]> coords = from cell in dangerousCellsList
                                            where cell[cellIndex] < BoundsOfWorkBoard[boundIndex]
                                            select cell;
                int min = coords.Min(cellIndex);
                if (min == - 1)
                {
                    return;
                }
                BoundsOfWorkBoard[boundIndex] = min - 1;
                if (BoundsOfWorkBoard[boundIndex] < 0)
                {
                    BoundsOfWorkBoard[boundIndex] = 0;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // минимум не найден оставляем границы прежними
            }

        }

        // поиск максимальной координаты
        private void GetMaximumCoordOutOfBound(int cellIndex, int boundIndex)
        {
            try
            {
                IEnumerable<int[]> coords = from cell in dangerousCellsList
                                            where cell[cellIndex] > BoundsOfWorkBoard[boundIndex]
                                            select cell;
                int max = coords.Max(cellIndex);
                if (coords.Max(cellIndex) == -1)
                {
                    return;
                }
                BoundsOfWorkBoard[boundIndex] = coords.Max(cellIndex) + 1;
                if (BoundsOfWorkBoard[boundIndex] > size - 1)
                {
                    BoundsOfWorkBoard[boundIndex] = size - 1;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // максимум не найден оставляем границы прежними
            }
        }

    }
}