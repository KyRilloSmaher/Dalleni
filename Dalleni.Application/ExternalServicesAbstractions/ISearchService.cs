using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.ExternalServicesAbstractions
{
    public class QuestionSearchDocument
    {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public List<string> Tags { get; set; } = new();
        public string CategoryName { get; set; } = default!;

        public int UpVotes { get; private set; }

        public int DownVotes { get; private set; }

        public int Views { get; private set; }

        public int AnswersCount { get; private set; }

        public double Score { get; private set; }

        public DateTime CreatedAt { get; set; }
        public bool HasAcceptedAnswer { get; set; }
    }
    public interface ISearchService
    {
        Task IndexQuestionAsync(QuestionSearchDocument doc);

        Task DeleteQuestionAsync(string id);
        Task<List<QuestionSearchDocument>> HybridSearchAsync(string query,string? category,List<string>? tags,int pageNumber,int pageSize);
    }
}
