using AutoMapper;
using Dalleni.Application.DTOs.Responses.Votes;
using Dalleni.Application.Features.Votes.Queries.GetUserVotedQuestionsQuery;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Votes.Queries.GetUserVotedQuestionsQuery
{
    public class GetUserVotedQuestionsQueryHandler : IRequestHandler<GetUserVotedQuestionsQuery, Response<IEnumerable<VotedQuestionResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<GetUserVotedQuestionsQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetUserVotedQuestionsQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<GetUserVotedQuestionsQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<VotedQuestionResponse>>> Handle(GetUserVotedQuestionsQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.userId,false);
            if (user is null)
            {
                return _responseHandler.BadRequest<IEnumerable<VotedQuestionResponse>>(SystemMessages.USER_NOT_FOUND);
            }
           var userVotes = await _unitOfWork.Votes.GetAllUserVotesAsync(request.userId,TargetVotesIsQuestions:true , cancellationToken);
           if (userVotes is null)
           {
            return _responseHandler.BadRequest<IEnumerable<VotedQuestionResponse>>(SystemMessages.NOT_FOUND);
           } 
           var dtos = _mapper.Map<IEnumerable<VotedQuestionResponse>>(userVotes);

           return _responseHandler.Success(dtos,SystemMessages.DATA_RETRIEVED);
        }
    }
}
