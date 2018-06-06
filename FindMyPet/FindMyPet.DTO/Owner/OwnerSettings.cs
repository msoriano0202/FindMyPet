using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DTO.Owner
{
    public class OwnerSettings
    {
        public bool ShowEmailForAlerts { get; set; }
        public bool ShowPhoneNumberForAlerts { get; set; }
        public bool ShowAddressForAlerts { get; set; }

        public bool ReceiveAlertsAll { get; set; }
        public bool ReceiveAlertsInRadio { get; set; }
        public int? ReceiveDistanceRadio { get; set; }

        public int SendDistanceRadio { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
