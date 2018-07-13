using FindMyPet.DTO.Owner;
using FindMyPet.DTO.PetAlert;
using FindMyPet.MVC.Models.Account;
using FindMyPet.MVC.Models.Home;
using FindMyPet.MVC.Models.Pet;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.ServiceClients
{
    public interface IOwnerServiceClient
    {
        Owner GetOwnerById(int ownerId);
        Owner GetOwnerByMembershipId(string membershipId);
        Owner CreateOwner(string membershipId, string firstName, string lastName, string email);
        Owner UpdateOwner(ProfileViewModel model);
        Owner UpdateOwner(SettingsViewModel model);
        Owner UpdateOwnerImageProfile(int ownerId, string imagePath);
        PetAlert AddPetAlert(PetAlertViewModel model);
        PetAlert AddPetPublicAlert(PetPublicAlertViewModel model, List<string> urlImages);
        PetAlert FoundPet(PetAlertViewModel model);
    }

    public class OwnerServiceClient : IOwnerServiceClient
    {
        private readonly IFindMyPetServiceClient _findMyPetClient;

        public OwnerServiceClient(): this(new FindMyPetServiceClient())
        { 
        }

        public OwnerServiceClient(IFindMyPetServiceClient findMyPetClient)
        {
            if (findMyPetClient == null)
                throw new ArgumentNullException(nameof(findMyPetClient));

            _findMyPetClient = findMyPetClient;
        }

        public Owner GetOwnerById(int ownerId)
        {
            var request = new OwnerRequest { Id = ownerId };
            var response = _findMyPetClient.JsonClient().Get(request);

            return response;
        }

        public Owner GetOwnerByMembershipId(string membershipId)
        {
            var request = new OwnerRequest { MembershipId = membershipId };
            var response = _findMyPetClient.JsonClient().Get(request);

            return response;
        }

        public Owner CreateOwner(string membershipId, string firstName, string lastName, string email)
        {
            var request = new OwnerCreateRequest
            {
                MembershipId = membershipId,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            var response = _findMyPetClient.JsonClient().Post(request);

            return response;
        }
        
        public Owner UpdateOwner(ProfileViewModel model)
        {
            var request = new OwnerUpdateRequest
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber1 = model.PhoneNumber1,
                PhoneNumber2 = model.PhoneNumber2,
                Address1 = model.Address1,
                Address2 = model.Address2
            };
            var response = _findMyPetClient.JsonClient().Put(request);

            return response;
        }

        public Owner UpdateOwner(SettingsViewModel model)
        {
            var request = new OwnerUpdateRequest
            {
                Id = model.OwnerId,
                Settings = new OwnerSettings
                {
                    ShowEmailForAlerts = model.ShowEmailForAlerts,
                    ShowPhoneNumberForAlerts = model.ShowPhoneNumberForAlerts,
                    ShowAddressForAlerts = model.ShowAddressForAlerts,
                    ReceiveAlertsAll = model.ReceiveAlertsAll,
                    ReceiveAlertsInRadio = model.ReceiveAlertsInRadio,
                    ReceiveDistanceRadio = model.ReceiveDistanceRadio,
                    SendDistanceRadio = model.SendDistanceRadio
                }
            };
            var response = _findMyPetClient.JsonClient().Put(request);

            return response;
        }

        public Owner UpdateOwnerImageProfile(int ownerId, string imagePath)
        {
            var request = new OwnerUpdateRequest
            {
                Id = ownerId,
                ProfileImageUrl = imagePath
            };

            var response = _findMyPetClient.JsonClient().Put(request);
            return response;
        }

        public PetAlert AddPetAlert(PetAlertViewModel model)
        {
            var request = new PetAlertCreateRequest
            {
                OwnerId = model.OwnerId,
                PetCode =  Guid.Parse(model.PetCode),
                Latitude = model.Latitude.Value,
                Longitude = model.Longitude.Value,
                Comment = model.Commets,
                PositionImageUrl = model.StaticMapUrl,
                Type = model.Type
            };

            var response = _findMyPetClient.JsonClient().Post(request);
            return response;
        }

        public PetAlert AddPetPublicAlert(PetPublicAlertViewModel model, List<string> urlImages)
        {
            var request = new PetAlertCreateRequest
            {
                OwnerId = model.OwnerId,
                Latitude = model.Latitude.Value,
                Longitude = model.Longitude.Value,
                Comment = model.Commets,
                PositionImageUrl = model.StaticMapUrl,
                Type = model.SelectedAlertTypeId,
                UrlImages = urlImages
            };

            var response = _findMyPetClient.JsonClient().Post(request);
            return response;
        }

        public PetAlert FoundPet(PetAlertViewModel model)
        {
            var request = new PetAlertFoundRequest
            {
                OwnerId = model.OwnerId,
                PetCode = Guid.Parse(model.PetCode),
                Comment = model.Commets,
                MakeItPublic = model.MakeItPublic
            };

            var response = _findMyPetClient.JsonClient().Put(request);
            return response;
        }
    }
}