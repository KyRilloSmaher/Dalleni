using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Commands.RegisterQuestionView
{
    public record RegisterQuestionViewCommand(Guid Id) : IRequest<Response<bool>>;
}
