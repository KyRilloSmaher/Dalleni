using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Questions.Commands.CreateQuestion
{
    public class CreateQuestionHandler : IRequestHandler<CreateQuestionCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<CreateQuestionHandler> _logger;

        public CreateQuestionHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<CreateQuestionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<Guid>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Get user
                var user = await _unitOfWork.UserManager.FindByIdAsync(request.UserId, true);
                if (user == null)
                    return _responseHandler.NotFound<Guid>(SystemMessages.USER_NOT_FOUND);

                // 2. Create domain entity
                var question = Question.Create(
                    request.dto.Title,
                    request.dto.Content,
                    request.UserId,
                    request.dto.CategoryId
                );

                // 3. Handle tags
                if (request.dto.Tags != null && request.dto.Tags.Any())
                {
                    var normalizedNames = request.dto.Tags.Select(t => t.Trim().ToLowerInvariant()).Distinct().ToList();
                    var existingTags = (await _unitOfWork.Tags.GetByNormalizedNamesAsync(normalizedNames, cancellationToken)).ToList();
                    var existingNames = existingTags.Select(t => t.Name.ToLowerInvariant()).ToList();

                    var newTagNames = normalizedNames.Except(existingNames).ToList();

                    foreach (var tagName in newTagNames)
                    {                                                                             
                        var newTag = Tag.Create(tagName);
                        await _unitOfWork.Tags.AddAsync(newTag);
                        existingTags.Add(newTag);
                    }

                    foreach (var tag in existingTags)
                    {
                        question.AddTag(tag);
                    }
                }                                               

                // 4. Save
                await _unitOfWork.Questions.AddAsync(question);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(question.Id, SystemMessages.RECORD_ADDED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question");
                return _responseHandler.BadRequest<Guid>(ex.Message);
            }
        }
    }
}
