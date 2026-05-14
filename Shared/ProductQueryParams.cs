using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductQueryParams
    {
        public int? brandid { get; set; }
        public int? typeid { get; set; }
        public string? search { get; set; }
        public ProductSorting Sort { get; set; }

        // make validation for pageindex and pagesize
        private int _pageindex = 1;
        public int PageIndex {

            get => _pageindex;
            set {
                _pageindex = (value <= 0) ? 1 : value;

            }
        }

        private const int maxpagesize = 10;
        private const int defaultpagesize = 5;
        private int _pagesize = defaultpagesize;

        public int PageSize {

            get => _pagesize;

            set {

                if (value <= 0)
                    _pagesize = defaultpagesize;

                else if (value >= maxpagesize)
                    _pagesize = maxpagesize;
                else 
                    _pagesize = value;

            }
        
        }
    }
}
