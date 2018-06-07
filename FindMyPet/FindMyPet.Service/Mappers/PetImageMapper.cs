using FindMyPet.DTO.Pet;
using FindMyPet.TableModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MyServiceStack.Mappers
{
    public interface IPetImageMapper
    {
        PetImageTableModel MapAddRequestToTable(PetImageAddRequest request);
        PetImage MapPetImageTableToPetImage(PetImageTableModel petImagetable);
    }

    public class PetImageMapper : IPetImageMapper
    {
        public PetImageTableModel MapAddRequestToTable(PetImageAddRequest request)
        {
            return new PetImageTableModel
            {
                ImageUrl = request.ImageUrl,
                IsProfileImage = request.IsImageProfile
            };
        }

        public PetImage MapPetImageTableToPetImage(PetImageTableModel petImageTable)
        {
            return new PetImage
            {
                Id = petImageTable.Id,
                ImageUrl = petImageTable.ImageUrl,
                IsProfileImage= petImageTable.IsProfileImage
            };
        }
    }
}