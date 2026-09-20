using AutoMapper;
using Dalleni.Application.Commans.Extensions;
using Dalleni.Application.DTOs.Responses.Notifications;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Notifications.Queries.GetMyNotifications
{
    /// <summary>
    /// Handles retrieving notifications for the authenticated user.
    /// </summary>
    public class GetMyNotificationsQueryHandler: IRequestHandler<GetMyNotificationsQuery,Response<PaginatedResult<NotificationResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseHandler _responseHandler;

        public GetMyNotificationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        public async Task<Response<PaginatedResult<NotificationResponseDto>>> Handle(GetMyNotificationsQuery request,CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var PagedRequest = request.PagedRequest;
            var query = await _unitOfWork.Notifications.GetByRecipientIdAsync(userId, cancellationToken: cancellationToken);
            var projected = _mapper.ProjectTo<NotificationResponseDto>(query);
            var result = await projected.ToPaginatedListAsync(PagedRequest.PageNumber , PagedRequest.PageSize);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
}