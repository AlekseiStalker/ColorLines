using System; 

namespace BallsAndLines.Model
{
    public static class RandomBall
    {
        static Random random = new Random();

        public static Ball GetRand() => 
            new Ball() { Color = (Color)random.Next(0, Enum.GetValues(typeof(Color)).Length) }; 
    }
}
