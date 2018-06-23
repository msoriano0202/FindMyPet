using FindMyPet.MVC.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.PetSearch
{
    public class PetSuccessStoryPagedListViewModel
    {
        public List<PetSuccessStoryViewModel> Records { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }
}