using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Pet
{
    [Route("/createpet", "POST")]
    public class CreatePetRequest : IReturn<Pet>
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
