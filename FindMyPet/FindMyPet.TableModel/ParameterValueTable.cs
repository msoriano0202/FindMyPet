using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace FindMyPet.TableModel
{
    public class ParameterValueTable //: IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(ParameterGroupTable))]
        public int ParameterGroupId { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(20)]
        public string Value { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
}
