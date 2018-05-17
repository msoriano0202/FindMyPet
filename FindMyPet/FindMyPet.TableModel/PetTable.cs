using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace FindMyPet.TableModel
{
    public class PetTable : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public Guid Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
