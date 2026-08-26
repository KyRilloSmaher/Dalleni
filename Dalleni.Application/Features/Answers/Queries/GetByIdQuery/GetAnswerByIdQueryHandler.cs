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
    public class GetAnswerByIdQueryHandler : IRequestHandler<GetAnswerByIdQuery, Response<AnswerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetAnswerByIdQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<AnswerDto>> Handle(GetAnswerByIdQuery request, CancellationToken cancellationToken)
        {
            var answer = await _unitOfWork.Answers.GetByIdAsync(request.Id,false);

            if (answer == null)
            {
                return _responseHandler.NotFound<AnswerDto>(SystemMessages.NOT_FOUND);
            }

            var dto = _mapper.Map<AnswerDto>(answer);

            return _responseHandler.Success(dto, SystemMessages.DATA_RETRIEVED);
        }
    }
}