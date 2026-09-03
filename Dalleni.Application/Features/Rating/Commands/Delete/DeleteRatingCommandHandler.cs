using AutoMapper;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Ratings.Commands.DeleteRating
{
    public class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseHandler _responseHandler;

        public DeleteRatingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        public async Task<Response<bool>> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
        {

                var rating = await _unitOfWork.Ratings.GetByIdAsync(request.rateId);

                if (rating == null)
                        return _responseHandler.NotFound<bool>(SystemMessages.NOT_FOUND);
                if (rating.UserId != request.userId)
                     return _responseHandler.Forbidden<bool>(SystemMessages.ACCESS_DENIED);

                rating.Delete();
                await _unitOfWork.SaveChangesAsync();
                  return _responseHandler.Success(true , SystemMessages.SUCCESS);

            
        }
    }
}
