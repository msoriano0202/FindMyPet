using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Owner
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTimeOffset CreatedOn { get; set; }

        public OwnerSettings Settings { get; set; }
    }
}
