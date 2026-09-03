using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Queries.GetOfficialEntityById
{
    public sealed record GetOfficialEntityByIdQuery(Guid Id)
        : IRequest<Response<OfficialEntityDto>>;
}