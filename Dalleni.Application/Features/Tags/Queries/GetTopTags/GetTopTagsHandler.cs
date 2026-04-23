using AutoMapper;
using Dalleni.Application.Commans.Extensions;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Tags.Queries.GetTopTags
{
    public class GetTopTagsHandler : IRequestHandler<GetTopTagsQuery, Response<PaginatedResult<TagDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetTopTagsHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedResult<TagDto>>> Handle(GetTopTagsQuery request, CancellationToken cancellationToken)
        {
            var PagedRequest = request.Request;
            var tags = await _unitOfWork.Tags.GetAllAsQueryableAsync(false, cancellationToken);
            var projected = _mapper.ProjectTo<TagDto>(tags);
            var result = await projected.ToPaginatedListAsync(PagedRequest.PageNumber, PagedRequest.PageSize);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
}
