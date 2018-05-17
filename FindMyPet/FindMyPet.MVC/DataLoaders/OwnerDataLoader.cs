using FindMyPet.DTO.Owner;
using FindMyPet.MVC.ServiceClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.DataLoaders
{
    public interface IOwnerDataLoader
    {
        int RegisterOwner(string membershipId, string firstName, string lastName, string email);
        Owner GetuserByMembershipId(string membershipId);
        Owner GetOwnerById(int ownerId);
        Owner GetOwnerByEmail(string email);
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

        public int RegisterOwner(string membershipId, string firstName, string lastName, string email)
        {
            return _ownerServiceClient.CreateOwner(membershipId, firstName, lastName, email);
        }

        public Owner GetuserByMembershipId(string membershipId)
        {
            return _ownerServiceClient.GetuserByMembershipId(membershipId);
        }

        public Owner GetOwnerById(int ownerId)
        {
            return _ownerServiceClient.GetOwnerById(ownerId);
        }

        public Owner GetOwnerByEmail(string email)
        {
            return _ownerServiceClient.GetOwnerByEmail(email);
        }
    }
}