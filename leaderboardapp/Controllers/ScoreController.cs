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
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace leaderboardapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ScoreController : Controller
    {
        private ILeaderboardRepository _leaderboardRepo;

        public ScoreController(ILeaderboardRepository leaderboardRepo)
        {
            _leaderboardRepo = leaderboardRepo;
        }

        // POST api/score
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] ScoreModel model)
        {
            await _leaderboardRepo.PostScoreAsync(model);

            return Json(new { success = true } );
        }

        // GET api/score/retrievescores
        [HttpGet]
        [Route("RetrieveScores")]
        public async Task<JsonResult> RetrieveScores(string centerKey, int offset, int numScores)
        {
            var retrievalDetails = new RetrieveScoresDetails
            {
                CenterKey = centerKey,
                Offset = offset,
                NumScores = numScores
            };

            var leaderboard = await _leaderboardRepo.RetrieveScoresAsync(retrievalDetails);

            return Json(leaderboard);
        }
    }
}