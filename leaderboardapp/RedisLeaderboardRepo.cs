// Copyright 2019 Google LLC

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at

//     https://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using leaderboardapp.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace leaderboardapp
{
    public class RedisLeaderboardRepo : ILeaderboardRepository
    {
        private const string REDIS_HOST_ENV = "REDIS_HOST";
        private const string LEADERBOARD_KEY = "leaderboard";
        private const string DEFAULT_REDIS_HOST = "localhost";

        // this is thread-safe
        private ConnectionMultiplexer _redis;

        public RedisLeaderboardRepo()
        {
            _redis = ConnectionMultiplexer.Connect(GetRedisHost());
        }

        public async Task<IList<LeaderboardItemModel>> RetrieveScoresAsync(RetrieveScoresDetails retrievalDetails)
        {
            IDatabase db = _redis.GetDatabase();
            List<LeaderboardItemModel> leaderboard = new List<LeaderboardItemModel>();

            long offset = retrievalDetails.Offset;

            // If centered, get rank of specified user first
            if (!string.IsNullOrWhiteSpace(retrievalDetails.CenterKey))
            {
                // SortedSetRankAsync corresponds to ZREVRANK
                var rank = await db.SortedSetRankAsync(LEADERBOARD_KEY, retrievalDetails.CenterKey, Order.Descending);

                // If specified user is not present, return empty leaderboard
                if (!rank.HasValue)
                {
                    return leaderboard;
                }

                // Use rank to calculate offset
                offset = Math.Max(0, rank.Value + retrievalDetails.Offset);
            }

            // SortedSetRangeByScoreWithScoresAsync corresponds to ZREVRANGEBYSCORE [WITHSCORES]
            var scores = await db.SortedSetRangeByScoreWithScoresAsync(LEADERBOARD_KEY,
                skip: offset,
                take: retrievalDetails.NumScores,
                order: Order.Descending);

            var startingRank = offset;
            for (int i = 0; i < scores.Length; i++)
            {
                var lbItem = new LeaderboardItemModel
                {
                    Rank = startingRank++,
                    PlayerName = scores[i].Element.ToString(),
                    Score = scores[i].Score
                };
                leaderboard.Add(lbItem);
            }

            return leaderboard;
        }

        public async Task<bool> PostScoreAsync(ScoreModel score)
        {
            IDatabase db = _redis.GetDatabase();

            // SortedSetAddAsync corresponds to ZADD
            return await db.SortedSetAddAsync(LEADERBOARD_KEY, score.PlayerName, score.Score);
        }

        private string GetRedisHost()
        {
            try
            {
                var redisHost = Environment.GetEnvironmentVariable(REDIS_HOST_ENV);
                return string.IsNullOrEmpty(redisHost) ? DEFAULT_REDIS_HOST : redisHost;
            }
            catch(Exception)
            {
                return DEFAULT_REDIS_HOST;
            }
        }
    }
}