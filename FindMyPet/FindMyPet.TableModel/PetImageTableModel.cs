using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.TableModel
{
    public class PetImageTableModel : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        public Guid Code { get; set; }

        [References(typeof(PetTableModel))]
        public int PetTableModelId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public bool IsProfileImage { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
