using AutoMapper;
using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Answers.Queries.GetAcceptedAnswersByQuestionId
{
  public class GetAcceptedAnswersByQuestionIdQueryHandler : IRequestHandler<GetAcceptedAnswersByQuestionIdQuery, Response<IEnumerable<AnswerDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetAcceptedAnswersByQuestionIdQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<AnswerDto>>> Handle(GetAcceptedAnswersByQuestionIdQuery request, CancellationToken cancellationToken)
        {
            
            var answers = await _unitOfWork.Answers.GetAcceptedAnswerAsync(request.questionId, false);
            if (answers == null)
            {
                return _responseHandler.NotFound<IEnumerable<AnswerDto>>(SystemMessages.NOT_FOUND);
            }
            var dtos = _mapper.Map<IEnumerable<AnswerDto>>(answers);
            return _responseHandler.Success(dtos, SystemMessages.DATA_RETRIEVED);
            
        }
    }    
}
