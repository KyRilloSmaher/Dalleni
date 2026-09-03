

using AutoMapper;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Ratings.Commands.UpdateRating
{
    public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, Response<RatingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseHandler _responseHandler;

        public UpdateRatingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        public async Task<Response<RatingDto>> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
                var requestDto = request.Dto;
                var rating = await _unitOfWork.Ratings.GetByIdAsync( requestDto.RateId);
                if (rating is null) 
                {
                        return _responseHandler.NotFound<RatingDto>( SystemMessages.NOT_FOUND);
                }
                if (rating.UserId != request.userId)
                { 
                    return _responseHandler.Forbidden<RatingDto>( SystemMessages.ACCESS_DENIED); 
                }
                var service = await _unitOfWork.Services .GetByIdAsync( rating.ServiceId, true, cancellationToken); 
                if (service is null)
                 {
                     return _responseHandler.NotFound<RatingDto>( SystemMessages.SERVICE_NOT_FOUND);
                 } 
                 var oldValue = rating.Value;
                 rating.Update( requestDto.Value, requestDto.Comment);
                 service.UpdateRating( oldValue, requestDto.Value);
                 await _unitOfWork.SaveChangesAsync(cancellationToken);
                 var ratingDto = _mapper.Map<RatingDto>(rating); 
                 return _responseHandler.Success( ratingDto, SystemMessages.SUCCESS); }
    }
}
