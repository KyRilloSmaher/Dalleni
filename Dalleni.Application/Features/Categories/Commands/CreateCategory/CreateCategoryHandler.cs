using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<CreateCategoryHandler> _logger;

        public CreateCategoryHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<CreateCategoryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Simple duplication check could be added here if there's a unique constraint or method in repo
                var category = Category.Create(request.Name);
                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(category.Id, SystemMessages.RECORD_ADDED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category {CategoryName}", request.Name);
                return _responseHandler.BadRequest<Guid>(ex.Message);
            }
        }
    }
}
