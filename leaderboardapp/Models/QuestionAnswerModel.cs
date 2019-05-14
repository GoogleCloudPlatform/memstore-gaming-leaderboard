using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leaderboardapp.Models
{
    public class QuestionAnswerModel
    {
        public string Question { get; set; }
        public bool IsCorrect { get; set; }
        public bool Selected { get; set; }

        public QuestionAnswerModel()
        {
            IsCorrect = true;
            Selected = false;
        }
    }
}