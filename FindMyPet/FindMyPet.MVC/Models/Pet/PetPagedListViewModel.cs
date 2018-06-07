using FindMyPet.MVC.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Pet
{
    public class PetPagedListViewModel
    {
        public List<PetProfileViewModel> Records { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }
}