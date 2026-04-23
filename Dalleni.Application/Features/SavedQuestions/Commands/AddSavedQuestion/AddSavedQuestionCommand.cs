using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.SavedQuestions.Commands.AddSavedQuestion
{
   public record AddSavedQuestionCommand(Guid UserId, Guid QuestionId) : IRequest<Response<SavedQuestion>>;
}
