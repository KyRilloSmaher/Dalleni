using AutoMapper;
using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Queries.GetOfficialEntityById
{
    internal sealed class GetOfficialEntityByIdQueryHandler: IRequestHandler<GetOfficialEntityByIdQuery,Response<OfficialEntityDto>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOfficialEntityByIdQueryHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<OfficialEntityDto>> Handle(GetOfficialEntityByIdQuery request,CancellationToken cancellationToken)
        {
            var entity =await _unitOfWork.OfficialEntities.GetByIdAsync(request.Id,false);

            if (entity is null)
            {
                return _responseHandler.NotFound<OfficialEntityDto>(SystemMessages.NOT_FOUND);
            }

            var dto = _mapper.Map<OfficialEntityDto>(entity);

            return _responseHandler.Success( dto,SystemMessages.SUCCESS);
        }
    }
}