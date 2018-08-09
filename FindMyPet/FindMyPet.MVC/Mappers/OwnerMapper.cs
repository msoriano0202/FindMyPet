using FindMyPet.DTO.Owner;
using FindMyPet.MVC.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.Mappers
{
    public interface IOwnerMapper
    {
        ProfileViewModel OwnerToProfileViewModel(Owner owner);
        SettingsViewModel OwnerToSettingsViewModel(Owner owner);
    }

    public class OwnerMapper : IOwnerMapper
    {
        public ProfileViewModel OwnerToProfileViewModel(Owner owner)
        {
            return new ProfileViewModel()
            {
                FirstName = HttpUtility.HtmlDecode(owner.FirstName),
                LastName = HttpUtility.HtmlDecode(owner.LastName),
                Email = owner.Email,
                PhoneNumber1 = HttpUtility.HtmlDecode(owner.PhoneNumber1),
                PhoneNumber2 = HttpUtility.HtmlDecode(owner.PhoneNumber2),
                Address1 = HttpUtility.HtmlDecode(owner.Address1),
                Address2 = HttpUtility.HtmlDecode(owner.Address2)
            };
        }

        public SettingsViewModel OwnerToSettingsViewModel(Owner owner)
        {
            return new SettingsViewModel()
            {
                ShowEmailForAlerts = owner.Settings.ShowEmailForAlerts,
                ShowPhoneNumberForAlerts = owner.Settings.ShowPhoneNumberForAlerts,
                ShowAddressForAlerts = owner.Settings.ShowAddressForAlerts,
                ReceiveAlertsAll = owner.Settings.ReceiveAlertsAll,
                ReceiveAlertsInRadio = owner.Settings.ReceiveAlertsInRadio,
                ReceiveDistanceRadio = owner.Settings.ReceiveDistanceRadio,
                SendDistanceRadio = owner.Settings.SendDistanceRadio
            };
        }
    }
}