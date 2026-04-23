using AutoMapper;
using Dalleni.Application.Commans.Extensions;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Questions.Queries.GetByTag
{
    public class GetByTagQueryHandler: IRequestHandler<GetByTagQuery, Response<PaginatedResult<QuestionSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetByTagQueryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedResult<QuestionSummaryDto>>> Handle(GetByTagQuery request, CancellationToken cancellationToken)
        {
            var PagedRequest = request.pagedRequest;
            var tagId = request.TagId;

            var tagExists = await _unitOfWork.Tags.ExistsAsync(tagId);
            if (!tagExists)
            {
                return _responseHandler.NotFound<PaginatedResult<QuestionSummaryDto>>(SystemMessages.NOT_FOUND);
            }

            var Questions = await _unitOfWork.Questions.GetByTagIdAsync(tagId,cancellationToken);
            var projected = _mapper.ProjectTo<QuestionSummaryDto>(Questions);
            var result = await projected.ToPaginatedListAsync(PagedRequest.PageNumber, PagedRequest.PageSize);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
}
