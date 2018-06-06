using FindMyPet.DTO.Owner;
using FindMyPet.TableModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MyServiceStack.Mappers
{
    public interface IOwnerMapper
    {
        OwnerTableModel MapCreateRequestToTable(OwnerCreateRequest request);
        Owner MapOwnerTableToOwner(OwnerTableModel ownerTable);
        OwnerSettings MapSettingsTableToSettings(OwnerSettingTableModel settingsTable);
        OwnerTableModel MapUpdateRequestToTable(OwnerUpdateRequest request, OwnerTableModel ownerTable);
        OwnerSettingTableModel MapUpdateRequestToTable(OwnerSettings request, OwnerSettingTableModel settingsTable);
    }

    public class OwnerMapper : IOwnerMapper
    {
        public OwnerTableModel MapCreateRequestToTable(OwnerCreateRequest request)
        {
            return new OwnerTableModel
            {
                MembershipId = request.MembershipId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };
        }

        public Owner MapOwnerTableToOwner(OwnerTableModel ownerTable)
        {
            return new Owner
            {
                Id = ownerTable.Id,
                FirstName = ownerTable.FirstName,
                LastName = ownerTable.LastName,
                Email = ownerTable.Email,
                PhoneNumber1 = ownerTable.PhoneNumber1,
                PhoneNumber2 = ownerTable.PhoneNumber2,
                Address1 = ownerTable.Address1,
                Address2 = ownerTable.Address2,
                ProfileImageUrl = ownerTable.ProfileImageUrl,
                CreatedOn = ownerTable.CreatedOn,
                Settings = MapSettingsTableToSettings(ownerTable.SettingTableModel)
            };
        }

        public OwnerSettings MapSettingsTableToSettings(OwnerSettingTableModel settingsTable)
        {
            return new OwnerSettings
            {
                ShowEmailForAlerts = settingsTable.ShowEmailForAlerts,
                ShowPhoneNumberForAlerts = settingsTable.ShowPhoneNumberForAlerts,
                ShowAddressForAlerts = settingsTable.ShowAddressForAlerts,
                ReceiveAlertsAll = settingsTable.ReceiveAlertsAll,
                ReceiveAlertsInRadio = settingsTable.ReceiveAlertsInRadio,
                ReceiveDistanceRadio = settingsTable.ReceiveDistanceRadio,
                SendDistanceRadio = settingsTable.SendDistanceRadio
            };
        }

        public OwnerTableModel MapUpdateRequestToTable(OwnerUpdateRequest request, OwnerTableModel ownerTable)
        {
            if (request.FirstName != null && !request.FirstName.Equals(ownerTable.FirstName))
                ownerTable.FirstName = request.FirstName;

            if (request.LastName != null && !request.LastName.Equals(ownerTable.LastName))
                ownerTable.LastName = request.LastName;

            if (request.Email != null && !request.Email.Equals(ownerTable.Email))
                ownerTable.Email = request.Email;

            if (request.PhoneNumber1 != null && !request.PhoneNumber1.Equals(ownerTable.PhoneNumber1))
                ownerTable.PhoneNumber1 = request.PhoneNumber1;

            if (request.PhoneNumber2 != null && !request.PhoneNumber2.Equals(ownerTable.PhoneNumber2))
                ownerTable.PhoneNumber2 = request.PhoneNumber2;

            if (request.Address1 != null && !request.Address1.Equals(ownerTable.Address1))
                ownerTable.Address1 = request.Address1;

            if (request.Address2 != null && !request.Address2.Equals(ownerTable.Address2))
                ownerTable.Address2 = request.Address2;

            if (request.ProfileImageUrl != null && !request.ProfileImageUrl.Equals(ownerTable.ProfileImageUrl))
                ownerTable.ProfileImageUrl = request.ProfileImageUrl;

            if (request.Settings != null)
                ownerTable.SettingTableModel = MapUpdateRequestToTable(request.Settings, ownerTable.SettingTableModel);

            return ownerTable;
        }

        public OwnerSettingTableModel MapUpdateRequestToTable(OwnerSettings request, OwnerSettingTableModel settingsTable)
        {
            if(!request.ShowEmailForAlerts.Equals(settingsTable.ShowEmailForAlerts))
                settingsTable.ShowEmailForAlerts = request.ShowEmailForAlerts;

            if (!request.ShowPhoneNumberForAlerts.Equals(settingsTable.ShowPhoneNumberForAlerts))
                settingsTable.ShowPhoneNumberForAlerts = request.ShowPhoneNumberForAlerts;

            if (!request.ShowAddressForAlerts.Equals(settingsTable.ShowAddressForAlerts))
                settingsTable.ShowAddressForAlerts = request.ShowAddressForAlerts;

            if (!request.ReceiveAlertsAll.Equals(settingsTable.ReceiveAlertsAll))
                settingsTable.ReceiveAlertsAll = request.ReceiveAlertsAll;

            if (!request.ReceiveAlertsInRadio.Equals(settingsTable.ReceiveAlertsInRadio))
                settingsTable.ReceiveAlertsInRadio = request.ReceiveAlertsInRadio;

            if (!request.ReceiveDistanceRadio.Equals(settingsTable.ReceiveDistanceRadio))
                settingsTable.ReceiveDistanceRadio = request.ReceiveDistanceRadio;

            if (!request.SendDistanceRadio.Equals(settingsTable.SendDistanceRadio))
                settingsTable.SendDistanceRadio = request.SendDistanceRadio;

            return settingsTable;
        }
    }
}