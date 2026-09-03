using AutoMapper;
using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Queries.GetMyOfficialEntities
{
    internal sealed class GetMyOfficialEntitiesQueryHandler: IRequestHandler<GetMyOfficialEntityQuery,Response<OfficialEntityDto>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMyOfficialEntitiesQueryHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<OfficialEntityDto>> Handle(GetMyOfficialEntityQuery request, CancellationToken cancellationToken)
        {

            var entity = await _unitOfWork.OfficialEntityMemberships.GetByUserAsync(request.userId);
            if (entity is null )
            {
                return _responseHandler.NotFound<OfficialEntityDto>(SystemMessages.NOT_FOUND);
            }
           var dto = _mapper.Map<OfficialEntityDto>(entity);
           return _responseHandler.Success<OfficialEntityDto>( dto,SystemMessages.SUCCESS);
        }
    }
}