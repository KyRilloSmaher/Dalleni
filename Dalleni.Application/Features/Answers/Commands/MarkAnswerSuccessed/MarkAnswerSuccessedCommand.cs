
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Commands.MarkAnswerSuccessed
{
    public record MarkAnswerSuccessedCommand (Guid id , Guid userId): IRequest<Response<bool>>;

}