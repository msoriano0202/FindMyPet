using FindMyPet.DTO.Pet;
using FindMyPet.Shared;
using FindMyPet.TableModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MyServiceStack.Mappers
{
    public interface IPetMapper
    {
        PetTableModel MapCreateRequestToTable(PetCreateRequest request);
        Pet MapPetTableToPet(PetTableModel petTable);
        PetImage MapPetImageTableToPetImage(PetImageTableModel petImageTable);
        PetTableModel MapUpdateRequestToTable(PetUpdateRequest request, PetTableModel petTable);
    }

    public class PetMapper : IPetMapper
    {
        public PetTableModel MapCreateRequestToTable(PetCreateRequest request)
        {
            return new PetTableModel
            {
                Name = request.Name,
                DateOfBirth = request.DateOfBirth,
                Status = (int)PetStatusEnum.Active,
                Description = request.Description
            };
        }

        public Pet MapPetTableToPet(PetTableModel petTable)
        {
            return new Pet
            {
                Id = petTable.Id,
                Code = petTable.Code,
                Name = petTable.Name,
                Status = petTable.Status.ToString(),
                Description = petTable.Description,
                DateOfBirth = petTable.DateOfBirth,
                CreatedOn = petTable.CreatedOn,
                Images = (petTable.Images != null && petTable.Images.Any()) ?
                                    petTable.Images.ConvertAll(x => MapPetImageTableToPetImage(x)).ToList() :
                                    new List<PetImage>()
            };
        }

        public PetImage MapPetImageTableToPetImage(PetImageTableModel petImageTable)
        {
            return new PetImage
            {
                Id = petImageTable.Id,
                ImageUrl = petImageTable.ImageUrl,
                IsProfileImage = petImageTable.IsProfileImage
            };
        }

        public PetTableModel MapUpdateRequestToTable(PetUpdateRequest request, PetTableModel petTable)
        {
            if (request.Name != null && !request.Name.Equals(petTable.Name))
                petTable.Name = request.Name;

            if (request.DateOfBirth != null && !request.DateOfBirth.Equals(petTable.DateOfBirth))
                petTable.DateOfBirth = request.DateOfBirth;

            if (request.Description != null && !request.Description.Equals(petTable.Description))
                petTable.Description = request.Description;

            return petTable;
        }
    }
}