
using AutoMapper;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Rating.Queries.GetRatingById
{
    public class GetRatingByIdQueryHandler : IRequestHandler<GetRatingByIdQuery, Response<RatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseHandler _responseHandler;

        public GetRatingByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper , IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        public async Task<Response<RatingDto>> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
        {
            var rating = await _unitOfWork.Ratings.GetByIdAsync(request.Id);
            if (rating is null)
            {
                return _responseHandler.NotFound<RatingDto>(SystemMessages.NOT_FOUND);
            }

            var ratingDto = _mapper.Map<RatingDto>(rating);
            return _responseHandler.Success(ratingDto, SystemMessages.SUCCESS);
        }
    }
    
}