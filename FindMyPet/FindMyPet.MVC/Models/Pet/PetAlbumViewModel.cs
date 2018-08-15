using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Pet
{
    public class PetAlbumViewModel
    {
        public bool HasMaxImages { get; set; }
        public List<PetImageViewModel> AlbumImages { get; set; }
    }
}