using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace FindMyPet.TableModel
{
    public class OwnerSharedPetTableModel : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(OwnerTableModel))]
        public int OwnerTableModelId { get; set; }

        [References(typeof(PetTableModel))]
        public int PetTableModelId { get; set; }

        public Guid TokenCode { get; set; }

        [StringLength(50)]
        public string ToOwnerEmail { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        public bool Used { get; set; }

        public DateTimeOffset? UsedOn { get; set; }
    }
}
