 using System.Runtime.Serialization;

namespace BallsAndLines.Model
{
    [DataContract]
    public class Ball 
    {
        public Ball() { }
        public Ball(Color color) => this.Color = color;
         
        [DataMember]
        public Color Color { get; internal set; }
         
        public Ball Clone() => new Ball() { Color = this.Color };

        public override string ToString() => Color.ToString();
         
        #region Override Equals&GetHashCode for UnitTesting serialize/deserialize

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            Ball ball = (Ball)obj;

            if (this.Color != ball.Color)
                return false;

            return true;
        }

        public override int GetHashCode() => Color.GetHashCode();

        #endregion
    }
}