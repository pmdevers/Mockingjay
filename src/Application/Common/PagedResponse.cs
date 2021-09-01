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
            int currentPage,
            int resultsPerPage,
            long totalResults)
        {
            Items = items;

            TotalPages = ((totalResults - 1) / resultsPerPage) + 1;

            if (currentPage < 1)
            {
                CurrentPage = 1;
            }
            else if (currentPage > TotalPages)
            {
                CurrentPage = TotalPages;
            }
            else
            {
                CurrentPage = currentPage;
            }

            ResultsPerPage = resultsPerPage;
            TotalResults = totalResults;
        }

        public long CurrentPage { get; }
        public int ResultsPerPage { get; }
        public long TotalPages { get; }
        public long TotalResults { get; }

        public IEnumerable<T> Items { get; }

        public bool IsEmpty => Items == null || !Items.Any();
        public bool IsNotEmpty => !IsEmpty;

        public static PagedResponse<T> Empty => new();

        public static PagedResponse<T> Create(
            IEnumerable<T> items,
            int currentPage,
            int resultsPerPage,
            long totalResults)
        {
            return new PagedResponse<T>(items, currentPage, resultsPerPage, totalResults);
        }
    }
}
