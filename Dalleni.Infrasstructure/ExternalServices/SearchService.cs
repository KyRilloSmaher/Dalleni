using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Models;
using Dalleni.Domin.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Dalleni.Infrastructure.ExternalServices
{
    public class SearchService : ISearchService
    {
        private readonly SearchClient _client;
        private readonly AzureSearchSettings _settings;
        private readonly ILogger<SearchService> _logger;

        public SearchService(IOptions<AzureSearchSettings> Settings, ILogger<SearchService> logger)
        {
            _settings = Settings.Value;
            _logger = logger;
            var endpoint = new Uri(_settings.EndPoint);
            var indexName = _settings.IndexName;
            var apiKey = _settings.ApiKey;

            _client = new SearchClient(endpoint, indexName, new AzureKeyCredential(apiKey));
        }

        public async Task IndexQuestionAsync(QuestionSearchDocument doc)
        {
            await _client.UploadDocumentsAsync(new[] { doc });
        }

        public async Task DeleteQuestionAsync(string id)
        {
            await _client.DeleteDocumentsAsync("id", new[] { id });
        }


        public async Task<List<QuestionSearchDocument>> HybridSearchAsync(string query,string? category,List<string>? tags,int pageNumber,int pageSize)
        {
            try
            {
                var options = new SearchOptions
                {
                    QueryType = SearchQueryType.Semantic,
                    Size = pageSize,
                    Skip = (pageNumber - 1) * pageSize,
                    ScoringProfile = "hot-score-profile"
                };

                options.SemanticSearch = new SemanticSearchOptions
                {
                    SemanticConfigurationName = "default"
                };


                options.SearchFields.Add(nameof(QuestionSearchDocument.Title));
                options.SearchFields.Add(nameof(QuestionSearchDocument.Content));
                options.SearchFields.Add(nameof(QuestionSearchDocument.Tags));

                var filters = new List<string>();

                if (!string.IsNullOrEmpty(category))
                    filters.Add($"categoryName eq '{category}'");

                if (tags != null && tags.Any())
                {
                    var tagFilter = string.Join(" or ",
                        tags.Select(t => $"tags/any(x: x eq '{t}')"));
                    filters.Add($"({tagFilter})");
                }

                if (filters.Any())
                    options.Filter = string.Join(" and ", filters);

                options.HighlightFields.Add(nameof(QuestionSearchDocument.Content));

                options.Select.Add(nameof(QuestionSearchDocument.Id));
                options.Select.Add(nameof(QuestionSearchDocument.Title));
                options.Select.Add(nameof(QuestionSearchDocument.Content));
                options.Select.Add(nameof(QuestionSearchDocument.Tags));
                options.Select.Add(nameof(QuestionSearchDocument.CategoryName));
                options.Select.Add(nameof(QuestionSearchDocument.Score));

                var response = await _client.SearchAsync<QuestionSearchDocument>(query, options);

                return response.Value.GetResults()
                    .Select(r => r.Document)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hybrid search failed for query: {Query}", query);
                throw;
            }
        }
    }
}
