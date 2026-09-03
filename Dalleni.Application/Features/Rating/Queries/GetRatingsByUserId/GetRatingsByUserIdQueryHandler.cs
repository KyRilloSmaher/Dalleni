


using AutoMapper;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Rating.Queries.GetRatingByUserId
{
    public class GetRatingsByUserIdQueryHandler : IRequestHandler<GetRatingsByUserIdQuery, Response<IEnumerable<RatingDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseHandler _responseHandler;

        public GetRatingsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper , IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        public async Task<Response<IEnumerable<RatingDto>>> Handle(GetRatingsByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Users.ExistsAsync(request.UserId))
            {
                var ratings = await _unitOfWork.Ratings.GetUserRatings(request.UserId);
                
                if (ratings is null || ! ratings.Any())
                {
                    return _responseHandler.NotFound<IEnumerable<RatingDto>>(SystemMessages.NOT_FOUND);
                }

                var dtos = _mapper.Map<IEnumerable<RatingDto>>(ratings);
                return _responseHandler.Success(dtos , SystemMessages.SUCCESS);
            }
            return _responseHandler.NotFound<IEnumerable<RatingDto>>(SystemMessages.USER_NOT_FOUND);
        }
    }
    
}