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
        OwnerTableModel MapCreateRequestToTable(CreateOwnerRequest request);
        Owner MapOwnerTableToOwner(OwnerTableModel ownerTable);
        OwnerTableModel MapUpdateRequestToTable(UpdateOwnerRequest request, OwnerTableModel ownerTable);
    }

    public class OwnerMapper : IOwnerMapper
    {
        public OwnerTableModel MapCreateRequestToTable(CreateOwnerRequest request)
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
                ProfileImageUrl = ownerTable.ProfileImageUrl,
                CreatedOn = ownerTable.CreatedOn
            };
        }

        public OwnerTableModel MapUpdateRequestToTable(UpdateOwnerRequest request, OwnerTableModel ownerTable)
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