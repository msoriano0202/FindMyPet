using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FindMyPet.TableModel
{
    public class OwnerSettingTableModel : IHasId<int>
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(OwnerTableModel))]
        public int OwnerTableModelId { get; set; }

        [Required]
        public bool ShowEmailForAlerts { get; set; }

        [Required]
        public bool ShowPhoneNumberForAlerts { get; set; }

        [Required]
        public bool ShowAddressForAlerts { get; set; }

        [Required]
        public bool ReceiveAlertsAll { get; set; }

        [Required]
        public bool ReceiveAlertsInRadio { get; set; }

        public int? ReceiveDistanceRadio { get; set; }

        [Required]
        public int SendDistanceRadio { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }
    }
}
