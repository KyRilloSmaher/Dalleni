





using AutoMapper;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Rating.Queries.GetUserRatingForService
{
    public class GetUserRatingForServiceQueryHandler : IRequestHandler<GetUserRatingForServiceQuery, Response<RatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseHandler _responseHandler;

        public GetUserRatingForServiceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper , IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        public async Task<Response<RatingDto>> Handle(GetUserRatingForServiceQuery request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Users.ExistsAsync(request.userId))
               return _responseHandler.NotFound<RatingDto>(SystemMessages.USER_NOT_FOUND);
            var rate = await _unitOfWork.Ratings.GetUserRatingForServiceAsync(request.serviceId ,request.userId);
            if (rate is null)
              return _responseHandler.NotFound<RatingDto>(SystemMessages.NOT_FOUND);
            var rateDto = _mapper.Map<RatingDto>(rate);
            return _responseHandler.Success(rateDto , SystemMessages.SUCCESS);
        }
    }
    
}