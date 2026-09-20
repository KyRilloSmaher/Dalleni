using AutoMapper;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Notifications.Queries.GetUnreadCount
{
    /// <summary>
    /// Handles retrieving the authenticated user's unread notification count.
    /// </summary>
    public class GetUnreadCountQueryHandler: IRequestHandler<GetUnreadCountQuery, Response<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IResponseHandler _responseHandler;

        public GetUnreadCountQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _responseHandler = responseHandler;
        }

        public async Task<Response<int>> Handle(GetUnreadCountQuery request,CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var count = await _unitOfWork.Notifications.GetUnreadCountAsync(userId,cancellationToken);

            return _responseHandler.Success(count, SystemMessages.DATA_RETRIEVED);
        }
    }
}