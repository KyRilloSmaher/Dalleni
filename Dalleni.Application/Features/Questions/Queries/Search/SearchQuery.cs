using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Questions.Queries.Search
{
    public record SearchQuery(string query , PagedRequest pagedRequest): IRequest<Response<PaginatedResult<QuestionSummaryDto>>>;
}
