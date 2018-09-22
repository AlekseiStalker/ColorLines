using System;
using System.Collections.Generic; 

namespace BallsAndLines.Model
{
    public class ActionCellEventArgs : EventArgs
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public Ball ActionBall { get; private set; }

        public IList<Cell> ActionCells { get; private set; }
         
        public ActionCellEventArgs()
        {
            this.ActionBall = null; 
        }

        public ActionCellEventArgs(int row, int col) 
        {
            this.Row = row;
            this.Col = col;
        }

        public ActionCellEventArgs(Ball ball)
        {
            this.ActionBall = ball;
        }

        public ActionCellEventArgs(int row, int col, Ball ball)
        {
            this.Row = row;
            this.Col = col;
            this.ActionBall = ball;
        }

        public ActionCellEventArgs(IList<Cell> cells)
        {
            ActionCells = cells;
        }
    }
}
