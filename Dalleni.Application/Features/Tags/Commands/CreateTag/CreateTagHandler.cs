using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Tags.Commands.CreateTag
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<CreateTagHandler> _logger;

        public CreateTagHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<CreateTagHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<Guid>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var normalized = request.Name.Trim().ToLowerInvariant();
                if (await _unitOfWork.Tags.ExistsByNormalizedNameAsync(normalized, cancellationToken))
                    return _responseHandler.BadRequest<Guid>(SystemMessages.DUPLICATE_RECORD);

                var tag = Tag.Create(request.Name);
                await _unitOfWork.Tags.AddAsync(tag);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(tag.Id, SystemMessages.RECORD_ADDED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tag {TagName}", request.Name);
                return _responseHandler.BadRequest<Guid>(ex.Message);
            }
        }
    }
}
