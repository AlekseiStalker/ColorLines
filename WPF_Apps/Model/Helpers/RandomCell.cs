
using System; 

namespace BallsAndLines.Model
{
    public static class RandomCell
    { 
        static Random _random = new Random();

        public static Cell GetRand() =>
            new Cell(_random.Next(0, Settings.BoardSize), _random.Next(0, Settings.BoardSize)); 
    }
}
