using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Admin
{
    public class AdminReportedAlert
    {
        public int Id { get; set; }
        public Guid Code { get; set; }
        public string OwnerFullName { get; set; }
        public string OwnerProfileImageUrl { get; set; }
        public string PetName { get; set; }
        public string PetProfileImageUrl { get; set; }
        public string Comment { get; set; }
        public List<string> Images { get; set; }
        public DateTimeOffset CreateOn{ get; set; }
    }
}
