using System.Collections.Generic;

namespace bookkeeping.Models
{
    public class PagedJournal
    {
        public PagedJournal(int totalRowsCount, int totalPages, IEnumerable<Journal> journals)
        {
            TotalRowsCount = totalRowsCount;
            TotalPages = totalPages;
            Journals = journals;
        }

        public int TotalRowsCount { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<Journal> Journals { get; set; }
    }
}