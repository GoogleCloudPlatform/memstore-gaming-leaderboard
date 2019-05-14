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