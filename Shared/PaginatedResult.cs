using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(int pageIndex, int pageSize, int totalcount, IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Totalcount = totalcount;
            Data = data;
        }

        public int PageIndex { get; set; }
       public int PageSize { get; set; }

       public int Totalcount { get; set; }

       public IEnumerable<T> Data { get; set; }
    }
}
