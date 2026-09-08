

namespace Dalleni.Application.ExternalServicesAbstractions
{
 public class QuestionSearchDocument
{
    public string id { get; set; }  // lowercase, not "Id"
    
    public string title { get; set; }  // lowercase, not "Title"
    
    public string content { get; set; }  // lowercase, not "Content"
    
    public List<string> tags { get; set; }  // lowercase, not "Tags"
    
    public string categoryName { get; set; }  // lowercase, not "CategoryName"
    
    public DateTime createdAt { get; set; }  // lowercase, not "CreatedAt"
    
    public bool hasAcceptedAnswer { get; set; }  // lowercase, not "HasAcceptedAnswer"
    
    public double score { get; set; }  // lowercase, not "Score"
    
    public int views { get; set; }  // lowercase, not "Views"
    
    public int answersCount { get; set; }  // lowercase, not "AnswersCount"
    
    public int upVotes { get; set; }  // lowercase, not "UpVotes"
    
    public int downVotes { get; set; }  // lowercase, not "DownVotes"
}
    public interface ISearchService
    {
        Task IndexQuestionAsync(QuestionSearchDocument doc);

        Task DeleteQuestionAsync(string id);
        Task<List<QuestionSearchDocument>> HybridSearchAsync(string query,string? category,List<string>? tags,int pageNumber,int pageSize);
    }
}
