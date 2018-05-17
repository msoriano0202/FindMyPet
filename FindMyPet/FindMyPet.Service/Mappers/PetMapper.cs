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
        PetTable MapCreateRequestToTable(CreatePetRequest request);
        Pet MapPetTableToPet(PetTable petTable);
        PetTable MapUpdateRequestToTable(UpdatePetRequest request, PetTable petTable);
    }

    public class PetMapper : IPetMapper
    {
        public PetTable MapCreateRequestToTable(CreatePetRequest request)
        {
            return new PetTable
            {
                Name = request.Name,
                DateOfBirth = request.DateOfBirth
            };
        }

        public Pet MapPetTableToPet(PetTable petTable)
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

        public PetTable MapUpdateRequestToTable(UpdatePetRequest request, PetTable petTable)
        {
            if (request.Name != null && !request.Name.Equals(petTable.Name))
                petTable.Name = request.Name;

            if (request.DateOfBirth != null && !request.DateOfBirth.Equals(petTable.DateOfBirth))
                petTable.DateOfBirth = request.DateOfBirth;

            return petTable;
        }
    }
}