using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Helper
{
    public class PaginatedResultDto<T>
    {
        public PaginatedResultDto(int pagelndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            Pagelndex = pagelndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int Pagelndex { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }

        public IReadOnlyList<T> Data { get; set; }

    }
}
