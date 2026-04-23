using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.SavedQuestions.Queries.GetUserSavedQuestion
{
    public record GetUserSavedQuestionsQuery(Guid UserId) : IRequest<Response<IEnumerable<SavedQuestion>>>;
}
