using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.PetSearch
{
    public class PetAlertDetails
    {
        public PetDetails PetInfo { get; set; }
        public List<OwnerDetails> OwnersInfo { get; set; }
    }
}
