using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace FindMyPet.TableModel
{
    public class OwnerTableModel : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        public Guid Code { get; set; }

        [Required]
        [StringLength(128)]
        public string MembershipId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(15)]
        public string PhoneNumber1 { get; set; }

        [StringLength(15)]
        public string PhoneNumber2 { get; set; }

        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        public string ProfileImageUrl { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        [Reference]
        public OwnerSettingTableModel SettingTableModel { get; set; }

        [Reference]
        public List<PetAlertTableModel> PetAlerts { get; set; }
    }
}
