using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.SavedQuestions.Commands.DeleteSavedQuestion
{
    public class DeleteSavedQuestionCommandHandler:IRequestHandler<DeleteSavedQuestionCommand,Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;

        public DeleteSavedQuestionCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }


        public async Task<Response<bool>> Handle(DeleteSavedQuestionCommand request, CancellationToken cancellationToken)
        {
            var savedQuestion = await _unitOfWork.SavedQuestionsRepository.GetByIdAsync(request.Id,true);
            if (savedQuestion == null)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.FAILED);
            }
            _unitOfWork.SavedQuestionsRepository.Remove(savedQuestion);
            await _unitOfWork.SaveChangesAsync();
            return _responseHandler.Success<bool>(true , SystemMessages.SUCCESS);
        }
    }
}
