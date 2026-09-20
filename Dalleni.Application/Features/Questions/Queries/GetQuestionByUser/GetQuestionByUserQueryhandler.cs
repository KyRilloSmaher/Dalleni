using AutoMapper;
using Dalleni.Application.Commans.Extensions;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Questions.Queries.GetByUser
{
    public class GetQuestionsByUserQueryHandler: IRequestHandler<GetQuestionsByUserQuery, Response<IEnumerable<QuestionSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetQuestionsByUserQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<QuestionSummaryDto>>> Handle(GetQuestionsByUserQuery request, CancellationToken cancellationToken)
        {
            var userId = request.userId;
            var user = await _unitOfWork.Users.GetByIdAsync(userId,false);
            if (user is null)
            {
              return _responseHandler.NotFound<IEnumerable<QuestionSummaryDto>>(message:SystemMessages.USER_NOT_FOUND);
            }
            var questions = await _unitOfWork.Questions.GetByUserIdAsync(userId);
            if (questions is null)
            {
                return _responseHandler.NotFound<IEnumerable<QuestionSummaryDto>>(message:SystemMessages.QUESTION_NOT_FOUND);
            }
            var result = _mapper.Map<IEnumerable<QuestionSummaryDto>>(questions);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
}
