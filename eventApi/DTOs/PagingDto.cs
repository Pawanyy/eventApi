using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eventApi.DTOs
{
    public class PagingDto
    {
        public string type { get; set; } = "latest";
        public int page  { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}