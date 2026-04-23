using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Questions.Queries.GetSimilars
{
    public record SimilarQuestionsQuery(string Question): IRequest<Response<IEnumerable<QuestionDetailsResponseDto>>>;
}
