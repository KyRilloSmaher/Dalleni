using AutoMapper;
using Dalleni.Application.Commans.Extensions;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Domin.Enums;
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
             Guid currentUserId = request.UserId;

            var tagExists = await _unitOfWork.Tags.ExistsAsync(tagId);
            if (!tagExists)
            {
                return _responseHandler.NotFound<PaginatedResult<QuestionSummaryDto>>(SystemMessages.NOT_FOUND);
            }

            var Questions = await _unitOfWork.Questions.GetByTagIdAsync(tagId,cancellationToken);
           var projected = Questions.Select(q => new QuestionSummaryDto
            {
                Id = q.Id,
                Title = q.Title,
                Content = q.Content,

                CategoryId = q.CategoryId,
                CategoryName = q.Category.Name,

                UserId = q.UserId,
                AuthorName = q.User.UserName,
                AuthorProfileImageUrl = q.User.ProfileImageUrl,
                AuthorReputation = q.User.Reputation,

                UpVotes = q.UpVotes,
                DownVotes = q.DownVotes,
                Views = q.Views,

                AnswersCount = q.Answers.Count(),

                IsClosed = q.IsClosed,
                Score = q.Score,
                CreatedAt = q.CreatedAt,

                Tags = q.QuestionTags
                    .Select(qt => new TagDto
                    {
                        Id = qt.Tag.Id,
                        Name = qt.Tag.Name
                    })
                    .ToList(),

                UpVotedByCurrentUser = q.Votes.Any(v =>v.UserId == currentUserId && v.Type == VoteType.Upvote),

                DownVotedByCurrentUser = q.Votes.Any(v =>v.UserId == currentUserId && v.Type == VoteType.Downvote)
            });
            var result = await projected.ToPaginatedListAsync(PagedRequest.PageNumber, PagedRequest.PageSize);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
}
