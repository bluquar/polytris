using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Polytris
{
    public partial class Polytris : Form
    {
        #region memberData
        public enum GameState
        {
            Menu,
            Paused,
            InGame,
            Loading
        }

        private GameState _state = GameState.Menu;

        private Point _canvasClickDown;
        private Point _canvasClickUp;
        private Point _canvasMove;
        private List<Point> _unhandledClicks = new List<Point>();

        private Random rand = new Random();

        private Bitmap _canvasBmp;
        private Bitmap _nextPieceBoxBmp;

        private int _maximumY;

        private Color[][] _gameGrid;

        private string _selectionString = "";

        //
        //
        //
        private List<Point[]> _arsenal = new List<Point[]>();
        private Color[] _arsenalColors;
        //
        //
        //

        private Font _menuFont = new Font("Courier New", 14);

        private Brush _whiteBrush = new SolidBrush(Color.White);

        private int _n;

        private int _nextPieceIndex;
        private int _currentPieceIndex;
        private Point[] _currentPiece;
        private int _currentPieceGridX;
        private int _currentPieceGridY;
        private int _countDownToNextCurrentPieceDrop;
        private int _timeBetweenCountdowns;
        private int _numOfRows;
        private int _numOfCols;

        private Pen _whitePen = new Pen(Color.Ivory, 3);

        #endregion //member data

        public Polytris()
        {
            InitializeComponent();
            InitializeCanvas();
            InitializeMenuObjects();

            InitializePolys();

            this.KeyDown += new KeyEventHandler(GotKeyDown);
            this.KeyUp += new KeyEventHandler(GotKeyUp);
            GameTick.Enabled = true;

        }

        private void InitializeCanvas()
        {
            _canvasBmp = new Bitmap(Canvas.MaximumSize.Width, Canvas.MaximumSize.Height);
            Canvas.Image = _canvasBmp;

            _nextPieceBoxBmp = new Bitmap(NextPieceBox.Width, NextPieceBox.Height);
            NextPieceBox.Image = _nextPieceBoxBmp;
        }

        private void InitializePolys()
        {
            listBox1.Items.Add("[1] Methis");
            listBox1.Items.Add("[2] Ethis");
            listBox1.Items.Add("[3] Butris");
            listBox1.Items.Add("[4] Tetris");
            listBox1.Items.Add("[5] Pentris");
            listBox1.Items.Add("[6] Hexis");
            listBox1.Items.Add("[7] Heptis");
            listBox1.Items.Add("[8] Octis");
            listBox1.Items.Add("[9] Nonis");
        }

        private void InitializeMenuObjects()
        {

        }

        private void InitializeNewGame(int n)
        {
            int boxWidth = 20;
            _numOfRows = Canvas.Height / boxWidth - 3;
            _numOfCols = n * 3;

            _gameGrid = new Color[_numOfCols][];
            for (int i = 0; i < _gameGrid.Length; i++)
            {
                _gameGrid[i] = new Color[_numOfRows];
            }

            _nextPieceIndex = rand.Next(_arsenal.Count - 1);
            _currentPieceIndex = rand.Next(_arsenal.Count - 1);
            _currentPiece = _arsenal[_currentPieceIndex];
            _currentPieceGridX = (int)((_numOfCols / 2) - (_n / 2));
            _currentPieceGridY = 0;
            _timeBetweenCountdowns = 10;
            _countDownToNextCurrentPieceDrop = 0;
        }

        private void GameTimer_Tick(object sender, EventArgs e) //every time the timer ticks this function is called
        {
            switch (_state)
            {
                case GameState.InGame:
                    DoGameAI();
                    DoGameRendering();
                    break;
                case GameState.Loading:
                    break;
                case GameState.Menu:

                    RenderMenu();
                    break;
                case GameState.Paused:

                    break;
                default:
                    break;
            }
        }

        private void RenderMenu()
        {
            using (Graphics gr = Graphics.FromImage(Canvas.Image))
            {
                gr.Clear(Color.Black);
                gr.DrawString("Welcome to Polytris!", _menuFont, _whiteBrush, 0, 0);
                /*int height = (Canvas.Height - 150) / (_menuMax - _menuMin + 1);
                int width = (Canvas.Width - 20);
                for (int i = 0; i <= _menuMax - _menuMin; i++)
                {
                    gr.DrawRectangle(_whitePen, 10, 50 + (i * height), width, height - 8);
                }*/

                if (_arsenal.Count > 0)
                {
                    int startX = 10;
                    int startY = 50;
                    int boxWidth = 10;
                    
                    for (int i = 0; i < _arsenal.Count; i++)
                    {
                        for (int j = 0; j < _arsenal[i].Length; j++)
                        {
                            gr.FillRectangle(new SolidBrush(_arsenalColors[i]), startX + boxWidth * _arsenal[i][j].X, startY + boxWidth * _arsenal[i][j].Y - ((_maximumY * vScrollBar1.Value) / 91), boxWidth - 2, boxWidth - 2);
                        }
                        startX += (_n + 1) * boxWidth;
                        if (startX + (_n + 1) * boxWidth > Canvas.Width)
                        {
                            startX = 10;
                            startY += (_n + 1) * boxWidth;
                        }
                    }

                    _maximumY = startY + (_n + 1) * ( 2 * boxWidth) - Canvas.Height;
                }
            }
            Canvas.Invalidate();
        }

        private void DoGameAI()
        {
            Canvas.Focus();

            _countDownToNextCurrentPieceDrop--;
            if (_countDownToNextCurrentPieceDrop <= 0)
            {
                _currentPieceGridY++;
                _countDownToNextCurrentPieceDrop = _timeBetweenCountdowns;

                int maxY = 0;
                for (int i = 0; i < _currentPiece.Length; i++)
                {
                    if (_currentPiece[i].Y > maxY)
                        maxY = _currentPiece[i].Y;
                }
                bool collision = false;
                for (int x = 0; x < _gameGrid.Length; x++)
                {
                    for (int y = 0; y < _gameGrid[x].Length; y++)
                    {
                        for (int i = 0; i < _currentPiece.Length; i++)
                        {
                            if ((_gameGrid[x][y].A != 0) && _currentPieceGridX + _currentPiece[i].X == x && _currentPieceGridY + _currentPiece[i].Y == y)
                            {
                                collision = true;
                            }
                        }
                    }
                }
                if (_currentPieceGridY + maxY >= _numOfRows || collision)
                {
                    for (int i = 0; i < _currentPiece.Length; i++)
                    {
                        _gameGrid[_currentPieceGridX + _currentPiece[i].X][_currentPieceGridY + _currentPiece[i].Y - 1] = _arsenalColors[_currentPieceIndex];
                    }

                    _currentPiece = _arsenal[_nextPieceIndex];
                    _currentPieceIndex = _nextPieceIndex;
                    _nextPieceIndex = rand.Next(_arsenal.Count);
                    _currentPieceGridX = (int)((_numOfCols / 2) - (_n / 2));
                    _currentPieceGridY = 0;
                    _countDownToNextCurrentPieceDrop = 0;
                }
            }
        }
        private void DoGameRendering()
        {
            using (Graphics gr = Graphics.FromImage(NextPieceBox.Image))
            {
                gr.Clear(Color.Transparent);
                int boxwidth = 14;
                for (int i = 0; i < _arsenal[_nextPieceIndex].Length; i++)
                {
                    gr.FillRectangle(new SolidBrush(_arsenalColors[_nextPieceIndex]), _arsenal[_nextPieceIndex][i].X * boxwidth,
                                                    _arsenal[_nextPieceIndex][i].Y * boxwidth, boxwidth - 2, boxwidth - 2);

                }
            }
            NextPieceBox.Invalidate();

            using (Graphics g = Graphics.FromImage(Canvas.Image))
            {
                g.Clear(Color.Black);
                int height = Canvas.Height - 40;
                int boxWidth = (height / _numOfRows);
                int width = Canvas.Width - 20;
                int boxHeight = (width / _numOfCols);
                int boxLength = Math.Min(boxWidth, boxHeight);

                for (int x = 0; x < _gameGrid.Length; x++)
                {
                    for (int y = 0; y < _gameGrid[x].Length; y++)
                    {
                        if (_gameGrid[x][y] != null)
                            g.FillRectangle(new SolidBrush(_gameGrid[x][y]), (Canvas.Width / 2) + ((x - (_numOfCols / 2)) * boxLength), (Canvas.Height / 2) + ((y - (_numOfRows / 2)) * boxLength), boxLength - 1, boxLength - 1);
                    }
                }
                g.DrawRectangle(_whitePen, (Canvas.Width / 2) - ((_numOfCols / 2) * boxLength), (Canvas.Height / 2) - ((_numOfRows / 2) * boxLength), _numOfCols * boxLength, _numOfRows * boxLength);

                for (int i = 0; i < _currentPiece.Length; i++)
                {
                    g.FillRectangle(new SolidBrush(_arsenalColors[_currentPieceIndex]), (Canvas.Width / 2) + ((_currentPiece[i].X + _currentPieceGridX - (_numOfCols / 2)) * boxLength) + 1,
                                                                                        (Canvas.Height / 2) + ((_currentPiece[i].Y + _currentPieceGridY - (_numOfRows / 2)) * boxLength) + 1 - (_countDownToNextCurrentPieceDrop * boxLength) / (_timeBetweenCountdowns),
                                                                                        boxLength -2, boxLength - 2);
                }

            }
            Canvas.Invalidate();
        }

        private Color[] GetColors(int n)
        {
            Color[] colors = new Color[n];

            Random rnd = new Random(4245);

            for (int i = 0; i < n; i++)
            {
                colors[i] = Color.FromArgb(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255));
            }

            return colors;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region input
        private void GotKeyDown(object o, KeyEventArgs e)
        {
            e.Handled = ProcessKeyDown(e);
        }
        private void GotKeyUp(object o, KeyEventArgs e)
        {
            e.Handled = ProcessKeyUp(e);
        }
        public bool ProcessKeyDown(KeyEventArgs e)
        {
            bool handled = true;

            switch (e.KeyCode)
            {
                case System.Windows.Forms.Keys.Right:
                    bool canMoveRight = true;
                    int maxX = 0;
                    for (int i = 0; i < _currentPiece.Length; i++)
                    {
                        if (_currentPiece[i].X > maxX)
                            maxX = _currentPiece[i].X;
                    }
                    if (_currentPieceGridX + maxX > _numOfCols)
                    {
                        canMoveRight = false;
                    }
                    if (canMoveRight)
                    _currentPieceGridX++;
                    break;
                case System.Windows.Forms.Keys.Left:
                    _currentPieceGridX--;
                    break;
                case System.Windows.Forms.Keys.Up:

                    break;
                case System.Windows.Forms.Keys.Down:
                    break;
                default:
                    handled = true;
                    break;
            }
            return handled;
        }
        public bool ProcessKeyUp(KeyEventArgs e)
        {
            bool handled = true;

            switch (e.KeyCode)
            {
                default:
                    handled = true;
                    break;
            }
            return handled;
        }
        void Canvas_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _canvasClickDown.X = e.X;
                _canvasClickDown.Y = e.Y;
                _canvasMove.X = e.X;
                _canvasMove.Y = e.Y;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {

            }
        }
        void Canvas_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _canvasClickUp.X = e.X;
                _canvasClickUp.Y = e.Y;
                _unhandledClicks.Add(_canvasClickUp);

            }
        }
        void Canvas_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _canvasMove.X = e.X;
            _canvasMove.Y = e.Y;
        }
        #endregion //input

        private static string GetNomenclature(int n)
        {
            string s = "";

            int numOfTens = 0;
            while (n >= 10)
            {
                n -= 10;
                numOfTens++;
            }

            if (numOfTens > 0 && numOfTens < 11)
            {
                switch (n)
                {
                    case 0:
                        break;
                    case 1:
                        s += "Un";
                        break;
                    case 2:
                        s += "Do";
                        break;
                    case 3:
                        s += "Tri";
                        break;
                    case 4:
                        s += "Tetra";
                        break;
                    case 5:
                        s += "Penta";
                        break;
                    case 6:
                        s += "Hexa";
                        break;
                    case 7:
                        s += "Hepta";
                        break;
                    case 8:
                        s += "Octo";
                        break;
                    case 9:
                        s += "Nona";
                        break;
                    default:
                        break;
                }
            }
            else if (numOfTens == 0)
            {
                switch (n)
                {
                    case 0:
                        break;
                    case 1:
                        s += "Meth";
                        break;
                    case 2:
                        s += "Eth";
                        break;
                    case 3:
                        s += "Butr";
                        break;
                    case 4:
                        s += "Tetr";
                        break;
                    case 5:
                        s += "Pent";
                        break;
                    case 6:
                        s += "Hex";
                        break;
                    case 7:
                        s += "Hept";
                        break;
                    case 8:
                        s += "Oct";
                        break;
                    case 9:
                        s += "Non";
                        break;
                    default:
                        break;
                }
            }

            switch (numOfTens)
            {
                case 1:
                    if (n == 0)
                        s += "Dec";
                    else
                        s += "dec";
                    break;
                case 2:
                    if (n == 0)
                        s += "Elicos";
                    else
                        s += "elicos";
                    break;
                case 3:
                    if (n == 0)
                        s += "Triacont";
                    else
                        s += "triacont";
                    break;
                case 4:
                    if (n == 0)
                        s += "Tetracont";
                    else
                        s += "tetracont";
                    break;
                case 5:
                    if (n == 0)
                        s += "Pentacont";
                    else
                        s += "pentacont";
                    break;
                case 6:
                    if (n == 0)
                        s += "Hexacont";
                    else
                        s += "hexacont";
                    break;
                case 7:
                    if (n == 0)
                        s += "Heptacont";
                    else
                        s += "heptacont";
                    break;
                case 8:
                    if (n == 0)
                        s += "Octocont";
                    else
                        s += "octocont";
                    break;
                case 9:
                    if (n == 0)
                        s += "Nonacont";
                    else
                        s += "nonacont";
                    break;
                default:
                    break;
            }
            if (numOfTens < 11)
                s += "is";
            else
                s += "Behemoth";

            return s;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int x = -3;
            int.TryParse(textBox1.Text, out x);
            if (x != 0)
            {
                listBox1.Items.Add("[" + x + "] " + GetNomenclature(x));
            }
            else
            {
                textBox1.Text = "";
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                _selectionString = (string)listBox1.Items[listBox1.SelectedIndex];
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                listBox1.Items.Remove(listBox1.Items[listBox1.SelectedIndex]);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            _state = GameState.Loading;

            int n = GetNumberDesired();
            GenerateArsenal(n);
            
            
        }
        private int GetNumberDesired()
        {
            int n = 4;

            if (_selectionString != "")
            {
                int i = _selectionString.IndexOf(']');


                string s = _selectionString.Substring(1, i - 1);

                int.TryParse(s, out n);
            }

            return n;
        }
        private void GenerateArsenal(int n)
        {
            Point[] seed = new Point[1];
            Point initialPoint = new Point(0, 0);
            seed[0] = initialPoint;

            _n = n;

            _arsenal.Add(seed);

            for (int k = 2; k <= n; k++)
            {
                List<Point[]> piecesToAdd = new List<Point[]>();
                foreach (Point[] piece in _arsenal)
                {
                    List<Point> pointsToTryAdding = new List<Point>();
                    for (int j = 0; j < k - 1; j++)
                    {
                        for (int dir = 0; dir < 4; dir++)
                        {
                            Point pointToAdd = piece[j];
                            switch (dir)
                            {
                                case 0:
                                    pointToAdd.X++;
                                    break;
                                case 1:
                                    pointToAdd.X--;
                                    break;
                                case 2:
                                    pointToAdd.Y++;
                                    break;
                                case 3:
                                    pointToAdd.Y--;
                                    break;
                                default:
                                    break;
                            }
                            bool alreadyTrying = false;
                            foreach (Point p in pointsToTryAdding)
                            {
                                if (p.X == pointToAdd.X && p.Y == pointToAdd.Y)
                                {
                                    alreadyTrying = true;
                                    break;
                                }
                            }
                            if (!alreadyTrying)
                            {
                                pointsToTryAdding.Add(pointToAdd);
                            }
                        }
                    }
                    List<Point> pointsToAdd = new List<Point>();
                    foreach (Point pta in pointsToTryAdding)
                    {
                        bool alreadyInPiece = false;
                        foreach (Point pip in piece)
                        {
                            if (pip.X == pta.X && pip.Y == pta.Y)
                            {
                                alreadyInPiece = true;
                                break;
                            }
                        }
                        if (!alreadyInPiece)
                            pointsToAdd.Add(pta);
                    }
                    foreach (Point pta in pointsToAdd)
                    {
                        Point[] newPiece = new Point[k];
                        for (int l = 0; l < k - 1; l++)
                        {
                            newPiece[l] = piece[l];
                        }
                        newPiece[k - 1] = pta;
                        piecesToAdd.Add(newPiece);
                    }
                }
                //trim identicals from piecesToAdd
                _arsenal = Trim(piecesToAdd);

            }

            
            _arsenalColors = GetColors(_arsenal.Count);
            _state = GameState.Menu;
        }
        private List<Point[]> Trim(List<Point[]> bulk)
        {
            List<Point[]> trimmed = new List<Point[]>();
            List<Point[]> standardBulk = new List<Point[]>();

            foreach (Point[] piece in bulk)
            {
                standardBulk.Add(StandardizeTranslation(piece));
            }

            for (int i = 0; i < bulk.Count; i++)
            {
                bool include = true;
                List<Point[]> rotations = GetRotations(standardBulk[i]);

                for (int j = i + 1; j < bulk.Count; j++)
                {
                    foreach (Point[] rot in rotations)
                    {
                        if (Identical(rot, bulk[j]))
                        {
                            include = false;
                            break;
                        }
                    }
                }
                if (include)
                {
                    trimmed.Add(bulk[i]);
                }
            }

            return trimmed;
        }
        private bool Identical(Point[] test, Point[] control)
        {
            bool identical = true;

            if (test.Length != control.Length)
                return false;

            for (int i = 0; i < test.Length; i++)
            {
                bool inControl = false;
                for (int j = 0; j < control.Length; j++)
                {
                    if (test[i].X == control[j].X && test[i].Y == control[j].Y)
                    {
                        inControl = true;
                        break;
                    }
                }
                if (!inControl)
                {
                    identical = false;
                    break;
                }
            }
            if (identical)
            {
                for (int i = 0; i < control.Length; i++)
                {
                    bool inTest = false;
                    for (int j = 0; j < test.Length; j++)
                    {
                        if (control[i].X == test[j].X && control[i].Y == test[j].Y)
                        {
                            inTest = true;
                            break;
                        }
                    }
                    if (!inTest)
                    {
                        identical = false;
                        break;
                    }
                }
            }
            return identical;
        }
        private List<Point[]> GetRotations(Point[] initial)
        {
            List<Point[]> rotations = new List<Point[]>();
            rotations.Add(initial);
            Point[] clock = new Point[initial.Length];
            Point[] counter = new Point[initial.Length];
            Point[] reflect = new Point[initial.Length];
            for (int i = 0; i < initial.Length; i++)
            {
                clock[i] = new Point(initial[i].Y, -1 * initial[i].X);
                counter[i] = new Point(-1 * initial[i].Y, initial[i].X);
                reflect[i] = new Point(-1 * initial[i].X, -1 * initial[i].Y);
            }
            rotations.Add(StandardizeTranslation(clock));
            rotations.Add(StandardizeTranslation(counter));
            rotations.Add(StandardizeTranslation(reflect));

            return rotations;
        }
        private Point[] StandardizeTranslation(Point[] piece)
        {
            int minX = 1;
            int minY = 1;
            for (int i = 0; i < piece.Length; i++)
            {
                if (piece[i].X < minX)
                    minX = piece[i].X;
                if (piece[i].Y < minY)
                    minY = piece[i].Y;
            }
            for (int i = 0; i < piece.Length; i++)
            {
                piece[i].X -= minX;
                piece[i].Y -= minY;
            }
            return piece;
        }
        private void PlayButton_Click(object sender, EventArgs e)
        {
            InitializeNewGame(_n);
            _state = GameState.InGame;
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
