using AutoMapper;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Users.Queries.SearchUserByName
{

    public class SearchUsersByNameQueryHandler : IRequestHandler<SearchUsersByNameQuery, Response<List<UserResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchUsersByNameQueryHandler> _logger;

        public SearchUsersByNameQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper, ILogger<SearchUsersByNameQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<List<UserResponseDto>>> Handle(SearchUsersByNameQuery request, CancellationToken cancellationToken)
        {
            var searchterm = request.keyword?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(searchterm))
            {              
                _logger.LogWarning("Search term is null or empty");
                return _responseHandler.BadRequest<List<UserResponseDto>>(SystemMessages.INVALID_INPUT);
            }

            var users = await _unitOfWork.Users.SearchAsync(searchterm);
            if (users is null || users.Count()==0)
            {
                _logger.LogInformation("No users found matching the search term: {SearchTerm}", searchterm);
                return _responseHandler.NotFound<List<UserResponseDto>>(SystemMessages.NOT_FOUND);
            }
            var userDto = _mapper.Map<List<UserResponseDto>>(users);
            return _responseHandler.Success(userDto, SystemMessages.SUCCESS);
        }
    }
}
