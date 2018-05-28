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
        OwnerTable MapCreateRequestToTable(CreateOwnerRequest request);
        Owner MapOwnerTableToOwner(OwnerTable ownerTable);
        OwnerTable MapUpdateRequestToTable(UpdateOwnerRequest request, OwnerTable ownerTable);
    }

    public class OwnerMapper : IOwnerMapper
    {
        public OwnerTable MapCreateRequestToTable(CreateOwnerRequest request)
        {
            return new OwnerTable
            {
                MembershipId = request.MembershipId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };
        }

        public Owner MapOwnerTableToOwner(OwnerTable ownerTable)
        {
            return new Owner
            {
                Id = ownerTable.Id,
                FirstName = ownerTable.FirstName,
                LastName = ownerTable.LastName,
                Email = ownerTable.Email,
                ProfileImageUrl = ownerTable.ProfileImageUrl,
                CreatedOn = ownerTable.CreatedOn
            };
        }

        public OwnerTable MapUpdateRequestToTable(UpdateOwnerRequest request, OwnerTable ownerTable)
        {
            if (request.FirstName != null && !request.FirstName.Equals(ownerTable.FirstName))
                ownerTable.FirstName = request.FirstName;

            if (request.LastName != null && !request.LastName.Equals(ownerTable.LastName))
                ownerTable.LastName = request.LastName;

            if (request.Email != null && !request.Email.Equals(ownerTable.Email))
                ownerTable.Email = request.Email;

            if (request.ProfileImageUrl != null && !request.ProfileImageUrl.Equals(ownerTable.ProfileImageUrl))
                ownerTable.ProfileImageUrl = request.ProfileImageUrl;

            return ownerTable;
        }
    }
}