using BallsAndLines.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBallsAndLines
{
    [TestFixture]
    public class ScoreTest
    {
        static readonly string _savedScorePath = @"F:\Work_data\DBBest\DBBest_MyProjects\Sources\WPF_Apps\UnitTestBallsAndLines\bin\Debug\SavedTestObj\score.xml";

        static Random _rand;

        readonly int _boardSize;

        Score _score;

        public ScoreTest()
        {
            _rand = new Random();

            _boardSize = Settings.BoardSize;

            _score = new Score();
        }


        [TestCase(1, ExpectedResult = 1)]
        [TestCase(15, ExpectedResult = 15)]
        [TestCase(-1, ExpectedResult = 15)]
        public int ChangedCurrentScore(int newCurScore)
        {
            _score.CurrentScore = newCurScore;

            return _score.CurrentScore;
        }

        [TestCase(1, ExpectedResult = 1)]
        [TestCase(9, ExpectedResult = 9)]
        [TestCase(8, ExpectedResult = 9)]
        public int ChangedMaxScore(int newScore)
        {
            _score.CheckOnMaxScore(newScore);

            return _score.GetMaxScore();
        }

        [TestCase(ExpectedResult = true)]
        public bool SerializeScore()
        {
            SetRandomScoreAndBestScoreList();

            _score.Save(_savedScorePath);

            Score savedScore = Score.Open(_savedScorePath);

            return _score.Equals(savedScore);
        }

        private void SetRandomScoreAndBestScoreList()
        {
            _score.CurrentScore = _rand.Next();

            _score.BestScoreList[GetRandomBoardSize()] = _rand.Next();
        }

        private int GetRandomBoardSize() => _rand.Next(Board.MinSize, Board.MaxSize);

        [TestCase(ExpectedResult = true)]
        public bool SaveOnlyBestScore()
        {
            int previousCurScore = GetExistCurScore();

            SetRandomScoreAndBestScoreList();

            _score.SaveOnlyNewRecord(_savedScorePath);

            Score savedScore = Score.Open(_savedScorePath);

            return previousCurScore == savedScore.CurrentScore;
        }

        private int GetExistCurScore() =>
            Score.Open(GameController.GameStorePath + "score.xml").CurrentScore;

        private void SerializeRandomScoreObject(out int score)
        {
            SetRandomScoreAndBestScoreList();

            _score.Save(_savedScorePath);

            score = _score.CurrentScore;
        }
    }
}
