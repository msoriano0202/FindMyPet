using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace FindMyPet.TableModel
{
    public class OwnerPetTableModel : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        public Guid Code { get; set; }

        [References(typeof(OwnerTableModel))]
        public int OwnerTableModelId { get; set; }

        [References(typeof(PetTableModel))]
        public int PetTableModelId { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        [Required]
        public bool IsFirstOwner { get; set; }
    }
}
