using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Shared
{
    public class PagedResponse<T>
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public List<T> Result { get; set; }
    }
}
