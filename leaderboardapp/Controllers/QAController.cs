using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using leaderboardapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace leaderboardapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class QAController : Controller
    {
        private IQuestionAnswerRepository _qaRepo;

        public QAController(IQuestionAnswerRepository qaRepo)
        {
            _qaRepo = qaRepo;
        }

        // GET api/qa
        [HttpGet]
        public async Task<ActionResult<IList<QuestionAnswerModel>>> Get()
        {
            var retVal = await _qaRepo.GetQuestionAnswerListAsync();

            return Ok(retVal);
        }
    }
}