using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _2017.EPAM.Gomoku.FirstTeam.Infrastructure.Zaitsev;
namespace _2017.EPAM.Gomoku.FirstTeam.TestInfranstructure
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInitWorkBoard()
        {
            WorkBoardClass wb = new WorkBoardClass(16);
        }

        [TestMethod]
        public void TestCloseCells()
        {
            WorkBoardClass wb = new WorkBoardClass(16);
            byte[,] board = new byte[16, 16];
            board[5, 5] = 1;
            board[7, 5] = 2;
            board[10, 5] = 1;
            board[4, 8] = 2;
            board[11, 7] = 1;
            board[11, 10] = 2;
            board[8, 11] = 2;
            board[5, 11] = 1;
            wb.SetWorkBoard(board);
        }

        [TestMethod]
        public void TestDistantDangerousCells()
        {
            WorkBoardClass wb = new WorkBoardClass(16);
            byte[,] board = new byte[16, 16];
            board[5, 5] = 1;
            board[7, 5] = 2;
            board[10, 5] = 1;
            board[4, 8] = 2;
            board[11, 7] = 1;
            board[11, 10] = 2;
            board[8, 11] = 2;
            board[5, 11] = 1;
            board[12, 0] = 1;
            board[10, 0] = 1;
            wb.SetWorkBoard(board);
        }

        [TestMethod]
        public void TestAllCellInWorkBoard()
        {
            WorkBoardClass wb = new WorkBoardClass(16);
            byte[,] board = new byte[16, 16];
            board[6, 6] = 1;
            board[7, 7] = 2;            
            wb.SetWorkBoard(board);
        }

        [TestMethod]
        public void TestSmallSizeOfBoard()
        {
            WorkBoardClass wb = new WorkBoardClass(6);
            byte[,] board = new byte[6, 6];            
            wb.SetWorkBoard(board);
        }

    }
}
