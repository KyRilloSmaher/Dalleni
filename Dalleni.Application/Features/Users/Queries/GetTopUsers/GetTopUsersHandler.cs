using AutoMapper;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Users.Queries.GetTopUsers
{
    public class GetTopUsersHandler : IRequestHandler<GetTopUsersQuery, Response<IEnumerable<UserResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetTopUsersHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<UserResponseDto>>> Handle(GetTopUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetTopUsersByReputationAsync(request.Count, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<UserResponseDto>>(users);
            return _responseHandler.Success(dtos, SystemMessages.DATA_RETRIEVED);
        }
    }
}
