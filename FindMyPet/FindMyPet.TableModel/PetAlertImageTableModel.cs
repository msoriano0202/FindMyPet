using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.TableModel
{
    public class PetAlertImageTableModel : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        public Guid Code { get; set; }

        [References(typeof(PetAlertTableModel))]
        public int PetAlertTableModelId { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
