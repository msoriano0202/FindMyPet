using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Shared
{
    public class PagedRequest
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
    }
}
