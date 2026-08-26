using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.SavedQuestions.Commands.AddSavedQuestion
{
    public class AddSavedQuestionCommandHandler : IRequestHandler<AddSavedQuestionCommand, Response<SavedQuestion>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;

        public AddSavedQuestionCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;


        }

        public async Task<Response<SavedQuestion>> Handle(AddSavedQuestionCommand request, CancellationToken cancellationToken)
        {
            if(await _unitOfWork.SavedQuestionsRepository.IsQuestionSavedByUserAsync(request.UserId, request.QuestionId))
            {
                return _responseHandler.BadRequest<SavedQuestion>(SystemMessages.QUESTION_ALREADY_SAVED);
            }   
            if(!await _unitOfWork.Questions.ExistsAsync(request.QuestionId))
            {
                return _responseHandler.NotFound<SavedQuestion>(SystemMessages.NOT_FOUND);
            }
            var savedQuestion = SavedQuestion.Create(request.UserId, request.QuestionId);
            await _unitOfWork.SavedQuestionsRepository.AddAsync(savedQuestion);
            await _unitOfWork.SaveChangesAsync();
            return _responseHandler.Success(savedQuestion, SystemMessages.SAVED_QUESTION_ADDED);
        }
    }
}
