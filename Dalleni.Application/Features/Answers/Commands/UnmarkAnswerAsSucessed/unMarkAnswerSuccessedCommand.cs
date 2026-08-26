
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Commands.UnmarkAnswerAsSuccessed
{
    public record UnmarkAnswerAsSuccessedCommand (Guid id , Guid userId): IRequest<Response<bool>>;

}