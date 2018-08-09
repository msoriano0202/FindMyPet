using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace FindMyPet.TableModel
{
    public class PetTableModel : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        public Guid Code { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        public int SexType { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Status: Active / Lost / Found
        /// </summary>
        public int Status { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        [Reference]
        public List<PetImageTableModel> Images { get; set; }
    }
}