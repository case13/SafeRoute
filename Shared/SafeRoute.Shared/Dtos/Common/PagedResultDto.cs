using System;
using System.Collections.Generic;
using System.Text;

namespace SafeRoute.Shared.Dtos.Common
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
