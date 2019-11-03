using System.Collections.Generic;

namespace LunchBreak.Helpers
{
    public class PaginationData<T>
    {
        public int PageNumber { get; set; }
        public int NumberOfItems { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
    }
}
