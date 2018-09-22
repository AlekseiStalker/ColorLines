
using System;
using System.Collections.Generic; 
using System.IO; 
using System.Runtime.Serialization; 
using System.Xml;

//List<List<Cell>>
using Cells2DList = System.Collections.Generic.List<System.Collections.Generic.List<BallsAndLines.Model.Cell>>;

namespace BallsAndLines.Model
{   
    [DataContract]
    public class Board
    {
        public static readonly int MaxSize;
        public static readonly int MinSize;
         
        private Cells2DList _cells;

        private FindLinesAlgorithm _linesFinder;
        private FindPathAlgorithm _pathFinder;

        static Board()
        {
            MaxSize = 20;
            MinSize = 5;
        }
         
        public Board()
        {
            InitCollectionCells();
        }

        [DataMember]
        public Cells2DList Cells
        {
            get => _cells;
            set => _cells = value;
        } 
         
        public event EventHandler<ActionCellEventArgs> CellChanged; 

        private void InitCollectionCells()
        { 
            _cells = new Cells2DList(Settings.BoardSize);

            for (int indxRow = 0; indxRow < Settings.BoardSize; indxRow++)
            {
                _cells.Add(new List<Cell>());

                for (int indxCol = 0; indxCol < Settings.BoardSize; indxCol++)
                {
                    _cells[indxRow].Add(new Cell(indxRow, indxCol));
                }
            }
        }

        internal void AddBallToCell(params Cell[] ballsInCells)
        {
            foreach (Cell cell in ballsInCells)
            {
                _cells[cell.Row][cell.Col].Ball = cell.Ball.Clone();

                CellChanged?.Invoke(this, new ActionCellEventArgs(cell.Row, cell.Col, cell.Ball.Clone()));
            }
        }

        internal void RemoveBallFromCell(params Cell[] ballsInCells)
        {
            foreach (Cell cell in ballsInCells)
            {
                _cells[cell.Row][cell.Col].Ball = null;

                CellChanged?.Invoke(this, new ActionCellEventArgs(cell.Row, cell.Col));
            }
        }
         
        public bool IsPathExist(Cell startCell, Cell targetCell)
        { 
            _pathFinder = new FindPathAlgorithm(startCell, targetCell, _cells);

            return _pathFinder.CanFindPath(); 
        } 
         
        public IList<Cell> GetBallPath() => 
            _pathFinder.GetLastFoundPath();  

        internal void ClearObsoletePathData() => 
            _pathFinder.ClearAllMarks();
         
        public IList<Cell> GetCollectedBallsInLines(int lastBallAddRow, int lastBallAddCol)
        { 
            if (_cells[lastBallAddRow][lastBallAddCol].Ball == null)  //triggered when two or more random balls exists in new collected line
                return new List<Cell>();

            _linesFinder = new FindLinesAlgorithm(findFromRow: lastBallAddRow, 
                                                  findFromCol: lastBallAddCol, 
                                                  originalBoardCells: _cells);

            Cells2DList possibleLines = _linesFinder.GetPossibleLines();
              
            return _linesFinder.FindCollectedLines(possibleLines);
        } 
            
        internal void RemoveCollectedLines(IList<Cell> line)
        { 
            for (int i = 0; i < line.Count; i++)
            {
                _cells[line[i].Row][line[i].Col].Ball = null;

                CellChanged?.Invoke(this, new ActionCellEventArgs(line[i].Row, line[i].Col, null));
            } 
        }
         
        public bool IsGameOver()
        { 
            int countFreeCells = 0;

            for (int row = 0; row < Settings.BoardSize; row++)
            {
                for (int col = 0; col < Settings.BoardSize; col++)
                {
                    if (_cells[row][col].Ball == null)
                    {
                        countFreeCells++;
                    }
                }
            }

            return countFreeCells < Settings.DropBallsPerStep;
        } 

        public List<Cell> this[int indxRow]
        {
            get => _cells[indxRow]; 
        }

        public Cell this[int row, int col]
        {
            get => _cells[row][col]; 
        } 

        public static Board Open(string filePath)
        {
            Board board = null;

            if (!File.Exists(filePath))
                return null;

            try
            { 
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(file, new XmlDictionaryReaderQuotas()))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Board));
                        var objGameBoard = serializer.ReadObject(reader, true);
                        board = (Board)objGameBoard;
                    }
                }
            }
            catch
            {
                throw new BallsAndLinesException("Error reading file");
            }

            return board;
        }

        public bool Save(string filePath)
        {
            try
            {
                using (FileStream writer = new FileStream(filePath, FileMode.Create))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Board));
                    serializer.WriteObject(writer, this);
                } 
            }
            catch 
            {
                throw new BallsAndLinesException("Error reading file");
            }

            return true;
        }

        #region Override Equals&GetHashCode for UnitTesting serialize/deserialize

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Board board = (Board)obj;

            if (this.Cells.Count != board.Cells.Count)
                return false;

            for (int row = 0; row < Cells.Count; row++)
            {
                for (int col = 0; col < Cells.Count; col++)
                {
                    if (!this[row][col].Equals(board[row][col]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;

            for (int i = 0; i < _cells.Count; i++)
            {
                for (int j = 0; j < _cells.Count; j++)
                {
                    hash ^= this[i][j].GetHashCode();
                }
            }

            return hash;
        }

        #endregion
    }
} 
