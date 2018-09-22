using NUnit.Framework;
using BallsAndLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BallsAndLines.Model;

namespace UnitTestBallsAndLines
{
    [TestFixture]
    public class BoardTest
    {
        static readonly string _savedBoardPath = @"F:\Work_data\DBBest\DBBest_MyProjects\Sources\WPF_Apps\UnitTestBallsAndLines\bin\Debug\SavedTestObj\board.xml";

        readonly int _defBoardSize;
        readonly int _defCountBallForLine;
        readonly int _defDroppingBalls;

        Board _board;

        IList<Cell> _randomCellsWithBalls;

        List<Cell> _horizontalBallsLine;
        List<Cell> _verticalBallsLine;
        List<Cell> _rightDiagBallsLine;
        List<Cell> _leftDiagBallsLine;

        List<Cell> _allLinesCells;

        public BoardTest()
        {
            _defBoardSize = 7;
            _defCountBallForLine = 3;
            _defDroppingBalls = 3;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            GameController.ChangeBoardSize(_defBoardSize);
            GameController.ChangeCountBallsForLineCollected(_defCountBallForLine);
            GameController.ChangeDroppingBallsPerStep(_defDroppingBalls);

            _board = GameController.CreateNewGameBoard(_defBoardSize);
        }

        [TestCase(3)]
        [TestCase(9)]
        [TestCase(25)]
        public void CountCellsOnBoard(int size)
        {
            _board = GameController.CreateNewGameBoard(size);

            int expected = Settings.BoardSize * Settings.BoardSize;
            int actual = _board.Cells.Count * _board.Cells.Count;

            Assert.AreEqual(expected, actual);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        public void AddingBallsToBoardCells(int count)
        {
            _randomCellsWithBalls = GameController.GenerateRandomBalls(count);

            _board.AddBallToCell(_randomCellsWithBalls.ToArray());

            for (int i = 0; i < count; i++)
                Assert.IsTrue(IsBallExist(_randomCellsWithBalls[i]));
        }

        private bool IsBallExist(Cell cell) =>
            _board[cell.Row][cell.Col].Ball != null &&
            _board[cell.Row][cell.Col].Ball.Color == cell.Ball.Color;

        [Test]
        public void RemoveBallsFromBoardCells()
        {
            _randomCellsWithBalls = GameController.CreateOnBoardNewRandomBalls();

            _board.RemoveBallFromCell(_randomCellsWithBalls.ToArray());

            for (int i = 0; i < _defDroppingBalls; i++)
                Assert.IsTrue(!IsBallExist(_randomCellsWithBalls[i]));
        }

        [Test]
        public void PathExist()
        {
            Cell startCellWithBall = new Cell(0, 0, RandomBall.GetRand());
            Cell tergetEmptyCell = new Cell(_defBoardSize - 1, _defBoardSize - 1);

            _board.AddBallToCell(startCellWithBall);

            Assert.IsTrue(_board.IsPathExist(startCellWithBall, tergetEmptyCell));
        }

        [Test]
        public void PathNotExist()
        {
            Cell startCellWithBall = new Cell(0, 0, RandomBall.GetRand());
            Cell tergetEmptyCell = new Cell(_defBoardSize - 1, _defBoardSize - 1);

            Cell[] cellsBlockingPath = GetCellsBlockPath();

            _board.AddBallToCell(startCellWithBall);
            _board.AddBallToCell(cellsBlockingPath);

            Assert.IsFalse(_board.IsPathExist(startCellWithBall, tergetEmptyCell));
        }

        private Cell[] GetCellsBlockPath() => 
        new Cell[3] {
            new Cell(1, 0, RandomBall.GetRand()),
            new Cell(1, 1, RandomBall.GetRand()),
            new Cell(0, 1, RandomBall.GetRand())
        };

        [TestCase(1, ExpectedResult = false)]
        public bool ZeroCollectedLines(int count)
        {
            List<Cell> oneAddedCell = GameController.GenerateRandomBalls(count);

            return GameController.IsCollectedLineExist(oneAddedCell.First().Row, oneAddedCell.First().Col);
        }

        [TestCase(1, ExpectedResult = 0)]
        [TestCase(3, ExpectedResult = 3)] 
        public int CountBallsInSHORTLine(int count)
        { 
            int countBalls = GetCountBallsInLine(count);

            return countBalls;
        }

        private int GetCountBallsInLine(int countBallsGenerate)
        {
            int constBallRow = 0; //doesn't matter horiz line or vertical
            Cell cellWithLastAddBall = null;

            for (int col = 0; col < countBallsGenerate && col < Settings.BoardSize; col++)
            {
                Cell cell = new Cell(constBallRow, col, new Ball(Color.Blue));

                _board.AddBallToCell(cell);

                if (col == countBallsGenerate - 1)
                    cellWithLastAddBall = cell;
            }

            return _board.GetCollectedBallsInLines(cellWithLastAddBall.Row, cellWithLastAddBall.Col).Count;
        }

        [Test]
        public void CountBallsInLONGLine()
        {
            int expectedCount = 5;
            int countBalls = GetCountBallsInLongLine(expectedCount);
            
            Assert.AreEqual(expectedCount, countBalls);
        }

        private int GetCountBallsInLongLine(int countBallsGenerate)
        {
            int constBallRow = 0;
            Cell cellWithLastAddBall = null;

            for (int col = 0; col < countBallsGenerate && col < Settings.BoardSize; col++)
            { 
                if (col == 2)
                    continue;

                Cell cell = new Cell(constBallRow, col, new Ball(Color.Blue));

                _board.AddBallToCell(cell);


                if (col == countBallsGenerate - 1)
                { 
                    cellWithLastAddBall = new Cell(constBallRow, 2, new Ball(Color.Blue)); ;

                    _board.AddBallToCell(cellWithLastAddBall);
                }
            }

            return _board.GetCollectedBallsInLines(cellWithLastAddBall.Row, cellWithLastAddBall.Col).Count;
        }
        
        [Test]
        public void DetectCrossVertHorizLines()
        {
            int expectedCount = 6;
            int countBalls = GetCountBallsInCrossVertHorizLines(expectedCount);

            Assert.AreEqual(expectedCount, countBalls);
        }

        private int GetCountBallsInCrossVertHorizLines(int countBallsGenerate)
        {
            Cell cellWithLastAddBall = null;

            int constCol = 0; 
            int constRow = 0;

            for (int row = 1; row < countBallsGenerate / 2; row++)
            {
                Cell cell = new Cell(row, constCol, new Ball(Color.Blue));

                _board.AddBallToCell(cell);
            }

            for (int col = 1; col < countBallsGenerate / 2; col++)
            {
                Cell cell = new Cell(constRow, col, new Ball(Color.Blue));

                _board.AddBallToCell(cell);
            }

            cellWithLastAddBall = new Cell(0, 0, new Ball(Color.Blue)); ;

            _board.AddBallToCell(cellWithLastAddBall);

            return _board.GetCollectedBallsInLines(cellWithLastAddBall.Row, cellWithLastAddBall.Col).Count;
        }

        [Test]
        public void DetectCrossDiagonalLines()
        {
            int expectedCount = 6;
            int countBalls = GetCountBallsInDiagonalLines(expectedCount);

            Assert.AreEqual(expectedCount, countBalls);
        }

        private int GetCountBallsInDiagonalLines(int countBallsGenerate)
        {
            Cell cellWithLastAddBall = null;

            Cell[] addBallsToCells = new Cell[4] {
                new Cell(0, 0, new Ball(Color.Blue)),
                new Cell(0, 2, new Ball(Color.Blue)),
                new Cell(2, 0, new Ball(Color.Blue)),
                new Cell(2, 2, new Ball(Color.Blue)),
            };

            cellWithLastAddBall = new Cell(1, 1, new Ball(Color.Blue)); ;

            _board.AddBallToCell(cellWithLastAddBall); 
            _board.AddBallToCell(addBallsToCells);
             
            return _board.GetCollectedBallsInLines(cellWithLastAddBall.Row, cellWithLastAddBall.Col).Count;
        }

        [Test]
        public void RemovingLinesFromBoard()
        {
            _horizontalBallsLine = GetHorizontalLine();
            _verticalBallsLine = GetVerticalLineLine();
            _rightDiagBallsLine = GetRightDiagonalLine();
            _leftDiagBallsLine = GetLeftDiagonalLine();

            AddBallsLinesToBoard();

            _board.RemoveCollectedLines(_horizontalBallsLine);
            _board.RemoveCollectedLines(_verticalBallsLine);
            _board.RemoveCollectedLines(_rightDiagBallsLine);
            _board.RemoveCollectedLines(_leftDiagBallsLine);
             
            bool isAnyBallExistOnLinesPosition = CheckLinesCellsOnBallExisting();

            Assert.IsFalse(isAnyBallExistOnLinesPosition);
        }

        private List<Cell> GetHorizontalLine() => 
             new List<Cell>(_defCountBallForLine) {
                new Cell(0, 0, new Ball(Color.Blue)),
                new Cell(0, 1, new Ball(Color.Blue)),
                new Cell(0, 2, new Ball(Color.Blue))
            };

        private List<Cell> GetVerticalLineLine() =>
             new List<Cell>(_defCountBallForLine) {
                new Cell(1, 0, new Ball(Color.Green)),
                new Cell(2, 0, new Ball(Color.Green)),
                new Cell(3, 0, new Ball(Color.Green))
            };

        private List<Cell> GetRightDiagonalLine() =>
             new List<Cell>(_defCountBallForLine) {
                new Cell(1, 1, new Ball(Color.Red)),
                new Cell(2, 2, new Ball(Color.Red)),
                new Cell(3, 3, new Ball(Color.Red))
            };

        private List<Cell> GetLeftDiagonalLine() =>
             new List<Cell>(_defCountBallForLine) {
                new Cell(_defBoardSize - 1, _defBoardSize - 1, new Ball(Color.Yellow)),
                new Cell(_defBoardSize - 2, _defBoardSize - 2, new Ball(Color.Yellow)),
                new Cell(_defBoardSize - 3, _defBoardSize - 3, new Ball(Color.Yellow))
            };

        private void AddBallsLinesToBoard()
        {
            _allLinesCells = new List<Cell>();
            _allLinesCells.AddRange(_horizontalBallsLine);
            _allLinesCells.AddRange(_verticalBallsLine);
            _allLinesCells.AddRange(_rightDiagBallsLine);
            _allLinesCells.AddRange(_leftDiagBallsLine);

            foreach (Cell cell in _allLinesCells) 
                _board[cell.Row][cell.Col].Ball = cell.Ball.Clone(); 
        }

        private bool CheckLinesCellsOnBallExisting()
        { 
            foreach (Cell cell in _allLinesCells)
            {
                if (cell.Ball != null)
                    return false;
            }
            return true;
        }

        [Test]
        public void GameOver()
        {
            int countBoardCells = _defBoardSize * _defBoardSize;

            AddBallsToBoardCells(countBoardCells - (_defDroppingBalls - 1));
             
            Assert.IsTrue(_board.IsGameOver());
        }

        private void AddBallsToBoardCells(int countBalls)
        {
            int counter = 0;
            for (int row = 0; row < _defBoardSize; row++)
            {
                for (int col = 0; col < _defBoardSize; col++)
                {
                    _board[row][col].Ball = RandomBall.GetRand();

                    if (++counter == countBalls)
                        return;
                }
            }
        }

        [TestCase(ExpectedResult = true)]
        public bool SerializeBoard()
        {
            List<Cell> cells = GenerateSomeBalls();

            _board.Save(_savedBoardPath);

            Board savedBoard = Board.Open(_savedBoardPath);

            return _board.Equals(savedBoard);
        }

        private List<Cell> GenerateSomeBalls() => 
            GameController.CreateOnBoardNewRandomBalls();

         
    }
}
