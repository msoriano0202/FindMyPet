using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.TableModel
{
    public class PetAlertTableModel : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(OwnerTableModel))]
        public int OwnerTableModelId { get; set; }

        public int? PetId { get; set; }

        /// <summary>
        /// Types: Lost / Abandom 
        /// </summary>
        [Required]
        public int AlterType { get; set; }

        public string Comment { get; set; }

        [Required]
        public int XCord { get; set; }

        [Required]
        public int YCord { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
