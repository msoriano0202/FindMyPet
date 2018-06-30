using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.PetSearch
{
    public class PetLostAlert : PetLost
    {
        public string Description { get; set; }
        public string LostComment { get; set; }
        //public string LocationImageUrl { get; set; }
    }
}
