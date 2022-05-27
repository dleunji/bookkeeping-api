using System;
using System.Collections.Generic;

namespace bookkeeping.Models
{
    public class PagedElement
    {
        public class JoinedElement
        {
            public int ElementId { get; set; }
            public int JournalId { get; set; }
            public int UserId { get; set; }
            public int CategoryId { get; set; }
            public int Amount { get; set; }
            public int Sign { get; set; }
            public DateTime TransactedAt { get; set; }
            public string Summary { get; set; }
        }
        public PagedElement(int totalRowsCount, int totalPages, IEnumerable<JoinedElement> elements)
        {
            TotalRowsCount = totalRowsCount;
            TotalPages = totalPages;
            Elements = elements;
        }

        public int TotalRowsCount { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<JoinedElement> Elements { get; set; }
    }
}