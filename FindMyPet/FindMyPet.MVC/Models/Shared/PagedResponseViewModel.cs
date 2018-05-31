using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Shared
{
    public class PagedResponseViewModel<T>
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public List<T> Result { get; set; }
    }
}