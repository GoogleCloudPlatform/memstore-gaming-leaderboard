using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leaderboardapp.Models
{
    public class LeaderboardItemModel
    {
        public long Rank { get; set; }
        public string PlayerName { get; set; }
        public double Score { get; set; }
    }
}