using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;

namespace FindMyPet.TableModel
{
    public class OwnerTable : IHasId<int>
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

        [Required]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
