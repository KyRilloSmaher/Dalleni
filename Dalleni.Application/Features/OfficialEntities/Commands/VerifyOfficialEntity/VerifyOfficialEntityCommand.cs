using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Commands.VerifyOfficialEntity
{
    public sealed record VerifyOfficialEntityCommand(Guid Id): IRequest<Response<bool>>;
}