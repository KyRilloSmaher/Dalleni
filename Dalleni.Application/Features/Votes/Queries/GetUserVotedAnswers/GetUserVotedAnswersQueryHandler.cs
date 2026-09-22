using AutoMapper;
using Dalleni.Application.DTOs.Responses.Votes;
using Dalleni.Application.Features.Votes.Queries.GetUserVotedAnswersQuery;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Votes.Queries.GetUserVotedAnswersQuery
{
    public class GetUserVotedAnswersQueryHandler : IRequestHandler<GetUserVotedAnswersQuery, Response<IEnumerable<VotedAnswerResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<GetUserVotedAnswersQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetUserVotedAnswersQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<GetUserVotedAnswersQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<VotedAnswerResponse>>> Handle(GetUserVotedAnswersQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.userId,false);
            if (user is null)
            {
                return _responseHandler.BadRequest<IEnumerable<VotedAnswerResponse>>(SystemMessages.USER_NOT_FOUND);
            }
           var userVotes = await _unitOfWork.Votes.GetAllUserVotesAsync(request.userId,TargetVotesIsQuestions:false , cancellationToken);
           if (userVotes is null)
           {
            return _responseHandler.BadRequest<IEnumerable<VotedAnswerResponse>>(SystemMessages.NOT_FOUND);
           } 
           var dtos = _mapper.Map<IEnumerable<VotedAnswerResponse>>(userVotes);

           return _responseHandler.Success(dtos,SystemMessages.DATA_RETRIEVED);
        }
    }
}
