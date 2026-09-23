using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Dalleni.Application.Commans.Extensions;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Domin.Enums;
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

       public async Task<Response<PaginatedResult<QuestionSummaryDto>>> Handle(GetPagedQuestionsQuery request,CancellationToken cancellationToken)
        {
            Guid currentUserId = request.UserId;

            var query = _unitOfWork.Questions.GetHotQuestionsAsync();
            var projected = query.Select(q => new QuestionSummaryDto
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

            var result = await projected.ToPaginatedListAsync(request.request.PageNumber,request.request.PageSize);

            return _responseHandler.Success(result,SystemMessages.DATA_RETRIEVED);
        }
    }
}
