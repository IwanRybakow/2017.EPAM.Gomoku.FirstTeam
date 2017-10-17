using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2017.EPAM.Gomoku.FirstTeam.Infrastructure.Zaitsev
{
    /// <summary>
    /// Класс для усечения игрового поля. Определяется рабочее поле
    /// </summary>
    public class WorkBoardClass
    {
        // Массив с координатами рабочей области.
        // 0 - левая граница (x), 1 - верхняя граница (у)
        // 2 - правая граница (х), 3 - нижняя граница (у)
        int[] BoundOfWorkBoard { get; set; }
        List<int[]> dangerousCells;             // список опасных ячейки
        const double MAX_DISTANCE = 2.82;       // максимальное расстояние от рабочей области (корень из 8-ми)
        private const int MIN_QUANTITY_EMPTY_CELLS = 4; // минимальное количество сводных ячее в рабочей области
        int size;   //размеры игрового поля
        bool workBoundNeed;     // а нужно ли рабочеее поле, если размер игрового поля мал

        // конструктор. Инициализирует границы рабочего поля
        // <param name="size"> размер игрового поля </param>
        public WorkBoardClass(int size)
        {
            // если размеры игрового поля малы, границы рабочего поля = границы игрового поля           
            if (size < 7)
            {
                workBoundNeed = false;
                BoundOfWorkBoard = new int[4];
                BoundOfWorkBoard[0] = 0;
                BoundOfWorkBoard[1] = 0;
                BoundOfWorkBoard[2] = size - 1;
                BoundOfWorkBoard[3] = size - 1;
                return;
            }
            // рабочее поле нужно
            workBoundNeed = true;
            this.size = size;
            // инициализируем границы рабочего поля в зависимости от четности размеров игрового поля
            if (size % 2 == 0)
            {
                InitBoundOfWorkBoard(1);
            }
            else
            {
                InitBoundOfWorkBoard(0);
            }
        }

        // инициализирует начальное рабочее поле
        private void InitBoundOfWorkBoard(int param)
        {
            int initSize = 1;
            BoundOfWorkBoard = new int[4];
            BoundOfWorkBoard[0] = (size / 2) - initSize - param;
            BoundOfWorkBoard[1] = (size / 2) - initSize - param;
            BoundOfWorkBoard[2] = (size / 2) + initSize;
            BoundOfWorkBoard[3] = (size / 2) + initSize;
        }


        // Установка нового игрового поля, возвращает координаты внутри рабочей обаласти
        // <param name="newBoard"></param>
        public List<int[]> SetWorkBoard(int[,] newBoard)
        {
            List<int[]> coords;
            // если рабочее поле не требуется, возвращаем координаты
            if (!workBoundNeed)
            {
                return coords = GetCoordListInWorkBoard(newBoard);
            }
            try
            {
                // находим ячейки за пределами рабочего поля
                List<int[]> outOfBound = FindOutOfBoundCells(newBoard);
                // находим опасные ячейки
                dangerousCells = FindDangerousCells(outOfBound);
                // определяем новые границы рабочего поля
                NewBoundsOfWorkBoard();
            }
            catch (NullReferenceException)
            {
                // либо нет ячеек за пределами рабочего поля, либо не найдены опасные
            }
            finally
            {
                // Составляем список координат ячеек в рабочей области
                coords = GetCoordListInWorkBoard(newBoard);
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
            // расширяем границы с проверкой выхода из га
            if (BoundOfWorkBoard[0] != 0)
            {
                BoundOfWorkBoard[0]--;
            }
            if (BoundOfWorkBoard[1] != 0)
            {
                BoundOfWorkBoard[1]--;
            }
            if (BoundOfWorkBoard[2] != size - 1)
            {
                BoundOfWorkBoard[2]++;
            }
            if (BoundOfWorkBoard[3] != size - 1)
            {
                BoundOfWorkBoard[3]++;
            }
            return GetCoordListInWorkBoard(newBoard);
        }

        
        // находит ячеки за пределами рабочей области
        // <param name="newBoard">игровое поле </param>        
        private List<int[]> FindOutOfBoundCells(int[,] newBoard)
        {
            List<int[]> outOfBound = new List<int[]>();

            for (int i = 0; i < newBoard.GetLength(0); i++)
            {
                for (int j = 0; j < newBoard.GetLength(1); j++)
                {
                    // если ячейка за пределами рабочей области и занята, добавляем ее в список
                    if (!(i >= BoundOfWorkBoard[1] && j >= BoundOfWorkBoard[0] &&
                         i <= BoundOfWorkBoard[3] && j <= BoundOfWorkBoard[2]) &&
                       newBoard[i, j] != 0)
                    {
                        outOfBound.Add(new int[] { i, j });
                    }
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
        private List<int[]> FindDangerousCells(List<int[]> outOfBound)
        {
            double? distance;
            List<int[]> dangerousCells = new List<int[]>();   // спиок близкорасположенных ячеек (наиболее опасные)
            List<int[]> distant = new List<int[]>(); // список удаленных (далеко) ячеек от рабочей области

            foreach (int[] cell in outOfBound)
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
        private List<int[]> GetCoordListInWorkBoard(int[,] newBoard)
        {
            List<int[]> coords = new List<int[]>();
            for (int i = BoundOfWorkBoard[1]; i <= BoundOfWorkBoard[3]; i++)
            {
                for (int j = BoundOfWorkBoard[0]; j <= BoundOfWorkBoard[2]; j++)
                {
                    // Если ячейка пуста, то добавляем ее в список
                    if (newBoard[i, j] == 0)
                    {
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
            distance[0] = DistancePointToPoint(cell, BoundOfWorkBoard[0], BoundOfWorkBoard[1]);
            // правый верх
            distance[1] = DistancePointToPoint(cell, BoundOfWorkBoard[2], BoundOfWorkBoard[1]);
            // левый низ
            distance[2] = DistancePointToPoint(cell, BoundOfWorkBoard[0], BoundOfWorkBoard[3]);
            // правый низ
            distance[3] = DistancePointToPoint(cell, BoundOfWorkBoard[2], BoundOfWorkBoard[3]);

            return distance.Min();
        }

        // находит расстояние от ячеек располженных в области "креста" расбочей области
        private double? IsInAreaDistance(int[] cell)
        {
            // ячейка сверху или снизу рабочей области 
            if (cell[1] > BoundOfWorkBoard[0] && cell[1] < BoundOfWorkBoard[2])
            {
                return DistanceToLinePerpendicular(cell[0], BoundOfWorkBoard[1], BoundOfWorkBoard[3]);
            }
            // ячейка слева или справа от рабочей области
            else if (cell[0] > BoundOfWorkBoard[1] && cell[0] < BoundOfWorkBoard[3])
            {
                return DistanceToLinePerpendicular(cell[1], BoundOfWorkBoard[0], BoundOfWorkBoard[2]);
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
                IEnumerable<int[]> coords = from cell in dangerousCells
                                            where cell[cellIndex] < BoundOfWorkBoard[boundIndex]
                                            select cell;
                int min = coords.Min(cellIndex);
                if (min == - 1)
                {
                    return;
                }
                BoundOfWorkBoard[boundIndex] = min - 1;
                if (BoundOfWorkBoard[boundIndex] < 0)
                {
                    BoundOfWorkBoard[boundIndex] = 0;
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
                IEnumerable<int[]> coords = from cell in dangerousCells
                                            where cell[cellIndex] > BoundOfWorkBoard[boundIndex]
                                            select cell;
                int max = coords.Max(cellIndex);
                if (coords.Max(cellIndex) == -1)
                {
                    return;
                }
                BoundOfWorkBoard[boundIndex] = coords.Max(cellIndex) + 1;
                if (BoundOfWorkBoard[boundIndex] > size - 1)
                {
                    BoundOfWorkBoard[boundIndex] = size - 1;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // максимум не найден оставляем границы прежними
            }
        }

    }
}