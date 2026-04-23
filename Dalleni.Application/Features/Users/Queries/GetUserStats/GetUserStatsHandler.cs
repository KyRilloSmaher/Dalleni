using AutoMapper;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Users.Queries.GetUserStats
{
    public class GetUserStatsHandler : IRequestHandler<GetUserStatsQuery, Response<UserResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetUserStatsHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<UserResponseDto>> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, false, cancellationToken);
            if (user == null)
                return _responseHandler.NotFound<UserResponseDto>(SystemMessages.USER_NOT_FOUND);

            var dto = _mapper.Map<UserResponseDto>(user);
            return _responseHandler.Success(dto, SystemMessages.DATA_RETRIEVED);
        }
    }
}
