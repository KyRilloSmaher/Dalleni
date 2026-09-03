using AutoMapper;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Ratings.Commands.CreateRating
{
    public sealed class CreateRatingCommandHandler: IRequestHandler<CreateRatingCommand, Response<RatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseHandler _responseHandler;

        public CreateRatingCommandHandler(IUnitOfWork unitOfWork,IMapper mapper,IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        public async Task<Response<RatingDto>> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            var service = await _unitOfWork.Services.GetByIdAsync(request.Dto.ServiceId, true, cancellationToken);

            if (service is null)
            {
                return _responseHandler.NotFound<RatingDto>(SystemMessages.SERVICE_NOT_FOUND);
            }

            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

            if (user is null)
            {
                return _responseHandler.NotFound<RatingDto>(SystemMessages.USER_NOT_FOUND);
            }

            var existingRating = await _unitOfWork.Ratings.GetUserRatingForServiceAsync(request.Dto.ServiceId, request.UserId);

            if (existingRating is not null)
            {
                return _responseHandler.Conflict<RatingDto>(SystemMessages.AlREADY_RATED);
            }

            var rating = _mapper.Map<Dalleni.Domin.Models.Rating>(request);

            await _unitOfWork.Ratings.AddAsync(rating,cancellationToken);

            service.AddRating(rating.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var ratingDto = _mapper.Map<RatingDto>(rating);

            return _responseHandler.Success(ratingDto,SystemMessages.SUCCESS);
        }
    }
}