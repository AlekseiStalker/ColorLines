using BallsAndLines.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallsAndLines.ViewModels
{
    class ScoreViewModel : BaseViewModel
    {
        Score _score;

        public ScoreViewModel()
        {
            _score = new Score();
        }

        public ScoreViewModel(Score score)
        {
            _score = score;  
        }

        public Score Score => _score;
         
        public int CurrentScore
        {
            get => _score.CurrentScore;
            set
            {
                _score.CurrentScore = value;
                RaisePropertyChanged();

                this.MaxScore = value;
            }
        }

        public int MaxScore
        {
            get => _score.GetMaxScore(); 

            private set
            {
                bool IsNewRecordReached = _score.CheckOnMaxScore(value);

                if (IsNewRecordReached)
                { 
                    _score.SaveOnlyNewRecord(GameController.GameStorePath + "score.xml");

                    RaisePropertyChanged();
                }
            }
        }

        public void OnUpdateGameScore(int addPoints) => CurrentScore += addPoints;
         
        public bool SerializeScore(string fileName) => _score.Save(GameController.GameStorePath + fileName); 
    }
}
