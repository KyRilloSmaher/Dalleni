using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dalleni.Application.Commans.Extensions;
namespace Dalleni.Application.Features.Categories.Queries.GetQuestionsByCategoryId
{
    public class GetQuestionsByCategoryIdQueryHandler : IRequestHandler<GetQuestionsByCategoryIdQuery, Response<PaginatedResult<QuestionSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetQuestionsByCategoryIdQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedResult<QuestionSummaryDto>>> Handle(GetQuestionsByCategoryIdQuery request, CancellationToken cancellationToken)
        {

            var PagedRequest = request.request;
            if(await _unitOfWork.Categories.ExistsAsync(request.categoryId) == false)
            {
                return _responseHandler.NotFound<PaginatedResult<QuestionSummaryDto>>(SystemMessages.CATEGORY_NOT_FOUND);
            }
            var query = await _unitOfWork.Questions.GetByCategoryIdAsync(request.categoryId);
            var projected = _mapper.ProjectTo<QuestionSummaryDto>(query);
            var result = await projected.ToPaginatedListAsync(PagedRequest.PageNumber , PagedRequest.PageSize);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);

        }
    }
}
