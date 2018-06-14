using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindMyPet.DTO.Pet;

namespace FindMyPet.DTO.PetAlert
{
    public class PetAlert
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public int? PetId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string ImageUrl { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset SolvedOn { get; set; }
    }
}
