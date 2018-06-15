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
        Pet MapPetTableToPet(PetTableModel petTable, bool includeImages);
        PetTableModel MapUpdateRequestToTable(PetUpdateRequest request, PetTableModel petTable);
    }

    public class PetMapper : IPetMapper
    {
        private readonly IPetImageMapper _petImageMapper;

        public PetMapper(IPetImageMapper petImageMapper)
        {
            if (petImageMapper == null)
                throw new ArgumentNullException(nameof(petImageMapper));

            _petImageMapper = petImageMapper;
        }

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

        public Pet MapPetTableToPet(PetTableModel petTable, bool includeImages)
        {
            return new Pet
            {
                Id = petTable.Id,
                Code = petTable.Code,
                Name = petTable.Name,
                Status = petTable.Status,
                Description = petTable.Description,
                DateOfBirth = petTable.DateOfBirth,
                CreatedOn = petTable.CreatedOn,
                ProfileImageUrl = GetProfileImage(petTable.Images),
                Images = (includeImages ? GetPetImages(petTable.Images) : null)
            };
        }

        private List<PetImage> GetPetImages(List<PetImageTableModel> petImages)
        {
            if (petImages != null && petImages.Any())
            {
                petImages = petImages.OrderByDescending(x => x.CreatedOn).ToList();
                return petImages.ConvertAll(x => _petImageMapper.MapPetImageTableToPetImage(x)).ToList();
            }
            else
                return new List<PetImage>();
        }

        private string GetProfileImage(List<PetImageTableModel> petImages)
        {
            string imageUrl = null;

            if (petImages != null && petImages.Any())
            {
                var profileImage = petImages.SingleOrDefault(x => x.IsProfileImage);
                if (profileImage != null)
                    imageUrl = profileImage.ImageUrl;
            }

            return imageUrl;
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