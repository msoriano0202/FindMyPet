using FindMyPet.DTO.Owner;
using FindMyPet.MVC.Models.Profile;
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
        Owner UpdateOwnerImageProfile(int ownerId, string imagePath);
    }

    public class OwnerServiceClient : IOwnerServiceClient
    {
        private readonly IFindMyPetServiceClient _findMyPetClient;

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
            var request = new CreateOwnerRequest
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
            var request = new UpdateOwnerRequest
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var response = _findMyPetClient.JsonClient().Put(request);

            return response;
        }

        public Owner UpdateOwnerImageProfile(int ownerId, string imagePath)
        {
            var request = new UpdateOwnerRequest
            {
                Id = ownerId,
                ProfileImageUrl = imagePath
            };

            var response = _findMyPetClient.JsonClient().Put(request);
            return response;
        }
    }
}