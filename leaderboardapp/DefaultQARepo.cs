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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace leaderboardapp
{
    public class DefaultQARepo : IQuestionAnswerRepository
    {
        const int MAX_OPERAND_VAL = 12;
        const int NUM_QUESTIONS = 6;
        Random _rand = new Random();
        string[] OPERATORS = { "+", "-", "*" };

        public Task<IList<QuestionAnswerModel>> GetQuestionAnswerListAsync()
        {
            return GetQuestionAnswerListInternalAsync();
        }

        private Task<IList<QuestionAnswerModel>> GetQuestionAnswerListInternalAsync()
        {
            List<QuestionAnswerModel> qaList = new List<QuestionAnswerModel>();

            for(int i = 0; i < NUM_QUESTIONS; i++)
            {
                qaList.Add(GetNextQA());
            }

            return Task.FromResult<IList<QuestionAnswerModel>>(qaList);
        }

        private int GetOperand()
        {
            return _rand.Next(1, MAX_OPERAND_VAL);
        }

        private bool CoinToss()
        {
            const int MAX_VAL = 10000;

            var val = _rand.Next(MAX_VAL);

            return (val > (MAX_VAL / 2));
        }

        private string GetOperator()
        {
            var index = _rand.Next(OPERATORS.Length);
            return OPERATORS[index];
        }

        private QuestionAnswerModel GetNextQA()
        {
            QuestionAnswerModel qa = new QuestionAnswerModel();

            var lho = GetOperand();
            var rho = GetOperand();

            var op = GetOperator();

            switch(op)
            {
                case "+":
                    {
                        var result = lho + rho;
                        if((result = AdjustResult(result)) != (lho + rho))
                        {
                            qa.IsCorrect = false;
                        }
                        qa.Question = string.Format("{0}+{1}={2}", lho, rho, result);
                    }
                    break;
                case "-":
                    {
                        var result = lho - rho;
                        if ((result = AdjustResult(result)) != (lho - rho))
                        {
                            qa.IsCorrect = false;
                        }
                        qa.Question = string.Format("{0}-{1}={2}", lho, rho, result);
                    }
                    break;
                case "*":
                    {
                        var result = lho * rho;
                        if ((result = AdjustResult(result)) != (lho * rho))
                        {
                            qa.IsCorrect = false;
                        }
                        qa.Question = string.Format("{0}*{1}={2}", lho, rho, result);
                    }
                    break;
            }

            return qa;
        }

        private int AdjustResult(int result)
        {
            const int MAX_ADJUSTMENT = 6;

            if (!CoinToss())
            {
                var adjust = _rand.Next(MAX_ADJUSTMENT);
                if (CoinToss())
                {
                    adjust = -adjust;
                }
                result += adjust;
            }

            return result;
        }
    }
}