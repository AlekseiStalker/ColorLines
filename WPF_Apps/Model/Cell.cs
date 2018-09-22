using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace BallsAndLines.Model
{
    [DataContract]
    public class Cell  
    {  
        public Cell()
        {
            this.Mark = -1;
        }

        public Cell(int row, int col) : this()
        {
            this.Row = row;
            this.Col = col;
        }

        public Cell(int row, int col, Ball ball) : this(row, col)
        {
            this.Ball = ball;
        }

        [DataMember]
        public int Row { get; internal set; } 
        [DataMember]
        public int Col { get; internal set; }
        [DataMember]
        public Ball Ball { get; internal set; }
         
        public int Mark { get; internal set; } // This varible use in Lee algorithm
         
        internal bool IsFreeForMarking() => (Ball == null && Mark == -1);
         
        public Cell Clone() => new Cell(this.Row, this.Col, this.Ball?.Clone());

        #region Override Equals&GetHashCode for UnitTesting serialize/deserialize

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Cell cell = (Cell)obj;

            if (this.Row != cell.Row || this.Col != cell.Col)
                return false;

            if (this.Ball == null && cell.Ball == null)
                return true;

            if (!this.Ball.Equals(cell.Ball))
                return false;

            return true;
        }

        public override int GetHashCode() => Row ^ Col ^ Ball.GetHashCode();

        #endregion
    }
}