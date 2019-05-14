using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leaderboardapp.Models
{
    public class RetrieveScoresDetails
    {
        // If CenterKey is not specified, signifies TOP
        public string CenterKey { get; set; }
        public int Offset { get; set; }
        public int NumScores { get; set; }
    }
}