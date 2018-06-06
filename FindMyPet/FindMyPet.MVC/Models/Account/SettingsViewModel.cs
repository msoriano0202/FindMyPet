using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Models.Account
{
    public class SettingsViewModel
    {
        public int OwnerId { get; set; }
        public bool ShowEmailForAlerts { get; set; }
        public bool ShowPhoneNumberForAlerts { get; set; }
        public bool ShowAddressForAlerts { get; set; }

        public bool ReceiveAlertsAll { get; set; }
        public bool ReceiveAlertsInRadio { get; set; }
        public int? ReceiveDistanceRadio { get; set; }

        public int SendDistanceRadio { get; set; }
    }
}