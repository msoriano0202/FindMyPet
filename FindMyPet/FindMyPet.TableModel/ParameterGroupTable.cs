using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System.Collections.Generic;

namespace FindMyPet.TableModel
{
    public class ParameterGroupTable //: IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Reference]
        public List<ParameterValueTable> ParameterValues { get; set; }
    }
}
