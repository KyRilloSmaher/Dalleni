using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Questions.Queries.GetRelatedQuestions
{
    public class GetRelatedQuestionsQueryHandler : IRequestHandler<GetRelatedQuestionsQuery, Response<IEnumerable<QuestionDetailsResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetRelatedQuestionsQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<QuestionDetailsResponseDto>>> Handle(GetRelatedQuestionsQuery request, CancellationToken cancellationToken)
        {
            var QuestionExists = await _unitOfWork.Questions.ExistsAsync(request.Id);
            if (!QuestionExists) {
                return _responseHandler.NotFound<IEnumerable<QuestionDetailsResponseDto>>(SystemMessages.NOT_FOUND);
            }

            var questions = await _unitOfWork.Questions.GetRelatedQuestionsAsync(request.Id, request.Count);
            if (questions is null || questions.Count() == 0) 
                return _responseHandler.Success<IEnumerable<QuestionDetailsResponseDto>>(null,SystemMessages.NO_DATA_FOUND);
            var dtos = _mapper.Map<IEnumerable<QuestionDetailsResponseDto>>(questions);
            return _responseHandler.Success(dtos,SystemMessages.DATA_RETRIEVED);
        }
    }
}
