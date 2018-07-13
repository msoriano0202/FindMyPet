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

        public Guid Code { get; set; }

        [References(typeof(OwnerTableModel))]
        public int? OwnerTableModelId { get; set; }

        public int? PetId { get; set; }

        /// <summary>
        /// Types: Lost / Abandom / Injured / Found / Adoption
        /// </summary>
        [Required]
        public int AlertType { get; set; }

        public string Comment { get; set; }

        public string CommentFound { get; set; }

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }

        public string PositionImageUrl { get; set; }

        /// <summary>
        /// Types: Active / Deleted
        /// </summary>
        [Required]
        public int AlertStatus { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? SolvedOn { get; set; }
        public bool MakeItPublic { get; set; }

        public int Approved { get; set; }

        [Reference]
        public List<PetAlertImageTableModel> Images { get; set; }
    }
}
