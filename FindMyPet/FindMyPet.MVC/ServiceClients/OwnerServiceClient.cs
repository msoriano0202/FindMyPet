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
        int CreateOwner(string membershipId, string firstName, string lastName, string email);
        Owner GetuserByMembershipId(string membershipId);
        Owner GetOwnerById(int ownerId);
        Owner GetOwnerByEmail(string email);
        Owner UpdateOwner(ProfileViewModel model);
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

        public int CreateOwner(string membershipId, string firstName, string lastName, string email)
        {
            var request = new CreateOwnerRequest
            {
                MembershipId = membershipId,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            var response = _findMyPetClient.JsonClient().Post(request);

            return response.Id;
        }

        public Owner GetuserByMembershipId(string membershipId)
        {
            var request = new SearchOwnerRequest { MembershipId = membershipId };
            var response = _findMyPetClient.JsonClient().Post(request);

            return response.FirstOrDefault();
        }

        public Owner GetOwnerById(int ownerId)
        {
            var request = new OwnerRequest { Id = ownerId };
            var response = _findMyPetClient.JsonClient().Get(request);

            return response;
        }

        public Owner GetOwnerByEmail(string email)
        {
            var request = new SearchOwnerRequest { Email = email };
            var response = _findMyPetClient.JsonClient().Post(request);

            return response.FirstOrDefault();
        }

        public Owner UpdateOwner(ProfileViewModel model)
        {
            var request = new UpdateOwnerRequest
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var response = _findMyPetClient.JsonClient().Post(request);

            return response;
        }
    }
}