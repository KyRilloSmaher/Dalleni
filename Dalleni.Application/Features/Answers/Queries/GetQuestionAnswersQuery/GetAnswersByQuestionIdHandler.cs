using AutoMapper;
using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dalleni.Application.Features.Answers.Queries
{
    public class GetAnswersByQuestionIdHandler : IRequestHandler<GetAnswersByQuestionIdQuery, Response<IEnumerable<AnswerDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetAnswersByQuestionIdHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<AnswerDto>>> Handle(GetAnswersByQuestionIdQuery request, CancellationToken cancellationToken)
        {
            if(!await _unitOfWork.Questions.ExistsAsync(request.QuestionId, cancellationToken))
            {
                return _responseHandler.NotFound<IEnumerable<AnswerDto>>(SystemMessages.QUESTION_NOT_FOUND);
            }
            var answers = await _unitOfWork.Answers.GetByQuestionIdAsync(request.QuestionId, false, cancellationToken);


            if (answers == null)
                {
                 return _responseHandler.NotFound<IEnumerable<AnswerDto>>(SystemMessages.NOT_FOUND);
                }
            var dtos = _mapper.Map<IEnumerable<AnswerDto>>(answers);
            
            return _responseHandler.Success(dtos, SystemMessages.DATA_RETRIEVED);
        }
    }
}
