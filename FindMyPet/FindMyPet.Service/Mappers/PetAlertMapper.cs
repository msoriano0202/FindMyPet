using FindMyPet.DTO.PetAlert;
using FindMyPet.Shared;
using FindMyPet.TableModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MyServiceStack.Mappers
{
    public interface IPetAlertMapper
    {
        PetAlertTableModel MapCreateRequestToTable(PetAlertCreateRequest request);
        PetAlert MapPetAlertTableToPetAlert(PetAlertTableModel petAlertTable);
    }

    public class PetAlertMapper : IPetAlertMapper
    {
        public PetAlertTableModel MapCreateRequestToTable(PetAlertCreateRequest request)
        {
            return new PetAlertTableModel
            {
                Code = Guid.NewGuid(),
                OwnerTableModelId = request.OwnerId,
                PetId = request.PetId,
                AlertType = request.Type,
                Comment = request.Comment,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                ImageUrl = request.ImageUrl,
                PositionImageUrl = request.PositionImageUrl,
                AlertStatus = (int)AlertStatusEnum.Active,
                CreatedOn = System.DateTime.Now,
                Approved = false
            };
        }

        public PetAlert MapPetAlertTableToPetAlert(PetAlertTableModel petAlertTable)
        {
            return new PetAlert
            {
                Id = petAlertTable.Id,
                Code = petAlertTable.Code,
                PetId = petAlertTable.PetId,
                Latitude = petAlertTable.Latitude,
                Longitude = petAlertTable.Longitude,
                ImageUrl = petAlertTable.ImageUrl,
                Type = petAlertTable.AlertType,
                Status = petAlertTable.AlertStatus,
                CreatedOn = petAlertTable.CreatedOn,
                SolvedOn = petAlertTable.SolvedOn
            };
        }
    }
}