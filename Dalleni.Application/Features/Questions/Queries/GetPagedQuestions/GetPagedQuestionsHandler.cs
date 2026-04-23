using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dalleni.Application.Commans.Extensions;
namespace Dalleni.Application.Features.Questions.Queries.GetPagedQuestions
{
    public class GetPagedQuestionsHandler : IRequestHandler<GetPagedQuestionsQuery, Response<PaginatedResult<QuestionSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetPagedQuestionsHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedResult<QuestionSummaryDto>>> Handle(GetPagedQuestionsQuery request, CancellationToken cancellationToken)
        {

            var PagedRequest = request.request;
            var query = await _unitOfWork.Questions.GetHotQuestionsAsync(cancellationToken);
            var projected = _mapper.ProjectTo<QuestionSummaryDto>(query);
            var result = await projected.ToPaginatedListAsync(PagedRequest.PageNumber , PagedRequest.PageSize);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);

        }
    }
}
