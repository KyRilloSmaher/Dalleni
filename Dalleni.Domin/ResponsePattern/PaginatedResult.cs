namespace Dalleni.Domin.ResponsePattern
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNext => PageNumber < TotalPages;
        public bool HasPrevious => PageNumber > 1;

        private PaginatedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PaginatedResult()
        {
        }

        public static PaginatedResult<T> Success(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
            => new(items, totalCount, pageNumber, pageSize);
    }
}
