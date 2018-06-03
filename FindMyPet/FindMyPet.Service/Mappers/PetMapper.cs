using FindMyPet.DTO.Pet;
using FindMyPet.TableModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MyServiceStack.Mappers
{
    public interface IPetMapper
    {
        PetTableModel MapCreateRequestToTable(CreatePetRequest request);
        Pet MapPetTableToPet(PetTableModel petTable);
        PetTableModel MapUpdateRequestToTable(UpdatePetRequest request, PetTableModel petTable);
    }

    public class PetMapper : IPetMapper
    {
        public PetTableModel MapCreateRequestToTable(CreatePetRequest request)
        {
            return new PetTableModel
            {
                Name = request.Name,
                DateOfBirth = request.DateOfBirth
            };
        }

        public Pet MapPetTableToPet(PetTableModel petTable)
        {
            return new Pet
            {
                Id = petTable.Id,
                Code = petTable.Code,
                Name = petTable.Name,
                DateOfBirth= petTable.DateOfBirth,
                CreatedOn = petTable.CreatedOn
            };
        }

        public PetTableModel MapUpdateRequestToTable(UpdatePetRequest request, PetTableModel petTable)
        {
            if (request.Name != null && !request.Name.Equals(petTable.Name))
                petTable.Name = request.Name;

            if (request.DateOfBirth != null && !request.DateOfBirth.Equals(petTable.DateOfBirth))
                petTable.DateOfBirth = request.DateOfBirth;

            return petTable;
        }
    }
}