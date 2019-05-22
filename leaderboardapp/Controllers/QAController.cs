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