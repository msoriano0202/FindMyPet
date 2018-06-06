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
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Email = owner.Email,
                PhoneNumber1 = owner.PhoneNumber1,
                PhoneNumber2 = owner.PhoneNumber2,
                Address1 = owner.Address1,
                Address2 = owner.Address2
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