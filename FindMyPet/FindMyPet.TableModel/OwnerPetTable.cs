using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace FindMyPet.TableModel
{
    public class OwnerPetTable : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(OwnerTable))]
        public int OwnerTableId { get; set; }

        [References(typeof(PetTable))]
        public int PetTableId { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
