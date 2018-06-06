using FindMyPet.DTO.Owner;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.Profile;
using FindMyPet.MVC.ServiceClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.DataLoaders
{
    public interface IOwnerDataLoader
    {
        Owner RegisterOwner(string membershipId, string firstName, string lastName, string email);
        Owner GetOwnerById(int ownerId);
        Owner GetOwnerByMembershipId(string membershipId);
        Owner UpdateOwner(ProfileViewModel model);
        Owner UpdateSettingsOwner(SettingsViewModel model);
        Owner UpdateOwnerImageProfile(int ownerId, string imagePath);
    }

    public class OwnerDataLoader : IOwnerDataLoader
    {
        private readonly IOwnerServiceClient _ownerServiceClient;

        public OwnerDataLoader() : this(new OwnerServiceClient(new FindMyPetServiceClient()))
        {
        }

        public OwnerDataLoader(IOwnerServiceClient ownerServiceClient)
        {
            if (ownerServiceClient == null)
                throw new ArgumentNullException(nameof(ownerServiceClient));

            _ownerServiceClient = ownerServiceClient;
        }

        public Owner RegisterOwner(string membershipId, string firstName, string lastName, string email)
        {
            return _ownerServiceClient.CreateOwner(membershipId, firstName, lastName, email);
        }

        public Owner GetOwnerById(int ownerId)
        {
            return _ownerServiceClient.GetOwnerById(ownerId);
        }

        public Owner GetOwnerByMembershipId(string membershipId)
        {
            return _ownerServiceClient.GetOwnerByMembershipId(membershipId);
        }

        public Owner UpdateOwner(ProfileViewModel model)
        {
            return _ownerServiceClient.UpdateOwner(model);
        }

        public Owner UpdateSettingsOwner(SettingsViewModel model)
        {
            return _ownerServiceClient.UpdateOwner(model);
        }

        public Owner UpdateOwnerImageProfile(int ownerId, string imagePath)
        {
            return _ownerServiceClient.UpdateOwnerImageProfile(ownerId, imagePath);
        }
    }
}