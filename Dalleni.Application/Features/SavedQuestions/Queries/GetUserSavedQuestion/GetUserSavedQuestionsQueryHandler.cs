using AutoMapper;
using Dalleni.Application.DTOs.Responses.Categories;
using Dalleni.Application.Features.Categories.Queries;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.SavedQuestions.Queries.GetUserSavedQuestion
{
    public class GetUserSavedQuestionsQueryHandler : IRequestHandler<GetUserSavedQuestionsQuery, Response<IEnumerable<SavedQuestion>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetUserSavedQuestionsQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<SavedQuestion>>> Handle(GetUserSavedQuestionsQuery request, CancellationToken cancellationToken)
        {
            var dtos = await _unitOfWork.SavedQuestionsRepository.GetSavedQuestionsByUserIdAsync(request.UserId,false);
            return _responseHandler.Success(dtos, SystemMessages.DATA_RETRIEVED);
        }
    }
}
