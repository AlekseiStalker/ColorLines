using NUnit.Framework; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BallsAndLines.Model;

namespace UnitTestBallsAndLines
{
    [TestFixture]
    class GameSettingTest
    {
        readonly int _defBoardSize;

        public GameSettingTest()
        {
            _defBoardSize = 12;
        }

        [SetUp]
        public void BeforeEach()
        {
            GameController.ChangeBoardSize(_defBoardSize);
        }

        [TestCase(3, ExpectedResult = 5)]
        [TestCase(9, ExpectedResult = 9)]
        [TestCase(12, ExpectedResult = 12)]
        [TestCase(21, ExpectedResult = 20)]
        [TestCase(-1, ExpectedResult = 5)]
        public int ChangedBoardSize(int newSize)
        {
            GameController.ChangeBoardSize(newSize);

            return Settings.BoardSize;
        }

        [TestCase(3, ExpectedResult = 3)]
        [TestCase(12, ExpectedResult = 12)]
        [TestCase(13, ExpectedResult = 12)]
        [TestCase(-1, ExpectedResult = 12)] 
        public int ChangedCountDroppingBalls(int newCount)
        {
            GameController.ChangeDroppingBallsPerStep(newCount);

            return Settings.DropBallsPerStep;
        }

        [TestCase(3, ExpectedResult = 3)]
        [TestCase(12, ExpectedResult = 12)]
        [TestCase(13, ExpectedResult = 12)]
        [TestCase(-1, ExpectedResult = 12)] 
        public int ChangedNumBallsInCollectedLine(int numBalls)
        {
            GameController.ChangeCountBallsForLineCollected(numBalls);

            return Settings.NumBallsInLine;
        }
    }
}
