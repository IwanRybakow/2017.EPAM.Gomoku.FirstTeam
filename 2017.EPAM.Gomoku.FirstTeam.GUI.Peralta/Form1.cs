using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTakToe;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        public int?[] HumanMove { get; set; }   // ход человека
        int figure; //хранится фигура, которой играет ползователь 1 - X, 2 - 0
        int countFields;
        List<Square> listSquares = new List<Square>();
        int size, x, y;
        bool checkSelectedSquare;
        int fieldValue; //хранит размер игорвого поля, например 4x4
        List<int[]> listCoordinates = new List<int[]>(); //хранятся координаты пустых ячеек
        int[,] playField; //двумерный массив (матрица), в котором хранится игровое поле

        public Form1()
        {
            InitializeComponent();
        }

        // установка GUI
        public void SetGUI(int sizeBoard, int figureID)
        {
            fieldValue = sizeBoard;
            CreatePlayField(sizeBoard);
            CreateSquares();
            CheckFigure();
            figure = figureID;
            if (figure == 1)
            {
                buttonX.BackColor = Color.Gray;
            }
            else if (figure == 2)
            {
                button0.BackColor = Color.Gray;
            }
        }

        private void CheckFigure()
        {
            if (figure == 1)
            {
                buttonX.BackColor = Color.Gray;
            }
            else if (figure == 2)
            {
                button0.BackColor = Color.Gray;
            }
        }

        private void playGround_Paint(object sender, PaintEventArgs e)
        {
            foreach (Square s in listSquares)
            {
                Pen myPen = new Pen(Color.Black, 3);
                e.Graphics.DrawRectangle(myPen, s.Location.X, s.Location.Y, size, size);
                e.Graphics.FillRectangle(Brushes.LightGray, s.Location.X, s.Location.Y, size, size);
                if (s.Value == 1)
                {
                    e.Graphics.FillRectangle(Brushes.Gray, s.Location.X, s.Location.Y, size, size);
                    if (fieldValue == 3)
                    {
                        e.Graphics.DrawImage(Properties.Resources.cross, s.Location.X, s.Location.Y, 80, 80);
                    }
                    else if (fieldValue == 4)
                    {
                        e.Graphics.DrawImage(Properties.Resources.cross, s.Location.X, s.Location.Y);
                    }
                    else if (fieldValue >= 5 && fieldValue <= 8)
                    {
                        e.Graphics.DrawImage(Properties.Resources.cross, s.Location.X, s.Location.Y, 60, 60);
                    }
                    else if (fieldValue == 9 || fieldValue == 10)
                    {
                        e.Graphics.DrawImage(Properties.Resources.cross, s.Location.X, s.Location.Y, 50, 50);
                    }
                    else if (fieldValue >= 11)
                    {
                        e.Graphics.DrawImage(Properties.Resources.cross, s.Location.X, s.Location.Y, 20, 20);
                    }
                }
                else if (s.Value == 2)
                {
                    e.Graphics.FillRectangle(Brushes.Gray, s.Location.X, s.Location.Y, size, size);
                    if (fieldValue == 3)
                    {
                        e.Graphics.DrawImage(Properties.Resources.circle, s.Location.X, s.Location.Y, 80, 80);
                    }
                    else if (fieldValue == 4)
                    {
                        e.Graphics.DrawImage(Properties.Resources.circle, s.Location.X, s.Location.Y);
                    }
                    else if (fieldValue >= 5 && fieldValue <= 8)
                    {
                        e.Graphics.DrawImage(Properties.Resources.circle, s.Location.X, s.Location.Y, 60, 60);
                    }
                    else if (fieldValue == 9 || fieldValue == 10)
                    {
                        e.Graphics.DrawImage(Properties.Resources.circle, s.Location.X, s.Location.Y, 50, 50);
                    }
                    else if (fieldValue >= 11)
                    {
                        e.Graphics.DrawImage(Properties.Resources.circle, s.Location.X, s.Location.Y, 20, 20);
                    }
                }
            }
        }

        void CreateSquares()
        {
            listSquares.Clear();
            countFields = (int)fieldValue;
            size = 60;
            int coordinateX = 0;
            int coordinateY = 0;
            int startPoint = 10;
            if (countFields > 8 && countFields < 11)
            {
                size = 50;
            }
            else if (countFields >= 11)
            {
                size = 20;
            }
            else
            {
                switch (countFields)
                {
                    case 3:
                        startPoint = 155;
                        size = 80;
                        break;
                    case 4:
                        startPoint = 135;
                        size = 70;
                        break;
                    case 5:
                        startPoint = 115;
                        break;
                    case 6:
                        startPoint = 80;
                        break;
                    case 7:
                        startPoint = 45;
                        break;
                    default:
                        break;
                }
            }
            for (int i = 0; i < countFields; i++)
            {
                for (int j = 0; j < countFields; j++)
                {
                    listSquares.Add(new Square(new Point(startPoint + coordinateX, startPoint + coordinateY), 0, i, j));
                    coordinateX = coordinateX + size + 5;
                }
                coordinateX = 0;
                coordinateY = coordinateY + size + 5;
            }
            playGround.Invalidate();
        }
        void CreatePlayField(int sizeBoard)
        {
            playField = new int[sizeBoard, sizeBoard];
        }
        private void playGround_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            listCoordinates.Clear();
            foreach (Square s in listSquares)
            {
                if ((s.Location.X < point.X && point.X < s.Location.X + size) && (s.Location.Y < point.Y && point.Y < s.Location.Y + size) && s.Value == 0)
                {
                    HumanMove = null;
                    s.Value = figure;
                    playField[s.CoordX, s.CoordY] = figure;

                    HumanMove = new int?[2] { s.CoordX, s.CoordY };

                    if (figure == 1)
                    {
                        playField[s.CoordX, s.CoordY] = 2;
                        s.Value = 2;
                    }
                    else
                    {
                        playField[s.CoordX, s.CoordY] = 1;
                        s.Value = 1;
                    }
                    playGround.Invalidate();
                    this.DialogResult = DialogResult.OK;

                    break;


                }
            }
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutProgram = new About();
            aboutProgram.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Properties.Settings ps = Properties.Settings.Default;
            this.Top = ps.Top;
            this.Left = ps.Left;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings ps = Properties.Settings.Default;
            ps.Top = this.Top;
            ps.Left = this.Left;
            ps.Save();
        }

        private void playGround_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            var g = playGround.CreateGraphics();
            foreach (Square s in listSquares)
            {
                if ((s.Location.X < point.X && point.X < s.Location.X + size) && (s.Location.Y < point.Y && point.Y < s.Location.Y + size) && s.Value == 0)
                {
                    g.FillRectangle(Brushes.Gray, s.Location.X, s.Location.Y, size, size);
                    x = s.Location.X;
                    y = s.Location.Y;
                    checkSelectedSquare = true;
                    break;
                }
                else if (checkSelectedSquare == true)
                {
                    g.FillRectangle(Brushes.LightGray, x, y, size, size);
                    checkSelectedSquare = false;
                    break;
                }
            }
        }
        public void GetBoard(int[,] Board)
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(0); j++)
                {
                    foreach (var square in listSquares)
                    {
                        if (square.CoordX == i && square.CoordY == j)
                        {
                            square.Value = Board[i, j];
                        }
                    }
                }
            }
            playField = Board;
            playGround.Invalidate();
        }
    }
}
