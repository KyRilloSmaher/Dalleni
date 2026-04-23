using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Questions.Queries.GetRelatedQuestions
{
    public record GetRelatedQuestionsQuery(Guid Id , int Count = 10):IRequest<Response<IEnumerable<QuestionDetailsResponseDto>>>;
}
