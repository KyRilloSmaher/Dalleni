using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.Features.Questions.Queries.GetRelatedQuestions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Questions.Queries.GetSimilars
{
    public class SimilarQuestionsQueryhandler : IRequestHandler<SimilarQuestionsQuery, Response<IEnumerable<QuestionDetailsResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public SimilarQuestionsQueryhandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<QuestionDetailsResponseDto>>> Handle(SimilarQuestionsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Question))
            {
                return _responseHandler.BadRequest<IEnumerable<QuestionDetailsResponseDto>>(SystemMessages.BAD_REQUEST);
            }

            var questions = await _unitOfWork.Questions.SearchAsync(request.Question);
            if (questions is null || questions.Count() == 0)
                return _responseHandler.Success<IEnumerable<QuestionDetailsResponseDto>>(null, SystemMessages.NO_DATA_FOUND);
            var result = await questions.OrderByDescending(q => q.CreatedAt).Take(5).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<QuestionDetailsResponseDto>>(result);
            return _responseHandler.Success(dtos, SystemMessages.DATA_RETRIEVED);
        }
    }
}
