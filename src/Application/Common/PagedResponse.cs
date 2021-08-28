using System.Collections.Generic;
using System.Linq;

namespace Mockingjay.Common
{
    public class PagedResponse<T>
    {
        protected PagedResponse()
        {
            Items = Enumerable.Empty<T>();
        }

        protected PagedResponse(
            IEnumerable<T> items,
            long currentPage,
            int resultsPerPage,
            long totalPages,
            long totalResults)
        {
            Items = items;
            CurrentPage = currentPage > totalPages ? totalPages : currentPage;
            ResultsPerPage = resultsPerPage;
            TotalPages = totalPages;
            TotalResults = totalResults;
        }

        public long CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public long TotalPages { get; set; }
        public long TotalResults { get; set; }
        public IEnumerable<T> Items { get; set; }
        public bool IsEmpty => Items == null || !Items.Any();
        public bool IsNotEmpty => !IsEmpty;
        public static PagedResponse<T> Empty => new ();
        public static PagedResponse<T> Create(
            IEnumerable<T> items,
            int currentPage,
            int resultsPerPage,
            int totalPages,
            long totalResults)
        {
            return new PagedResponse<T>(items, currentPage, resultsPerPage, totalPages, totalResults);
        }
    }
}
