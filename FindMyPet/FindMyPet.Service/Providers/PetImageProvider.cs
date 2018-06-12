using FindMyPet.DTO.Pet;
using FindMyPet.MyServiceStack.DataAccess;
using FindMyPet.MyServiceStack.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MyServiceStack.Providers
{
    public interface IPetImageProvider
    {
        Task<PetImage> AddPetImageAsync(PetImageAddRequest request);
        Task<int> DeletePetImageAsync(PetImageDeleteRequest request);
    }

    public class PetImageProvider : IPetImageProvider
    {
        private readonly IPetImageDataAccess _petImageDataAccess;
        private readonly IPetImageMapper _petImageMapper;

        public PetImageProvider(IPetImageDataAccess petImageDataAccess, IPetImageMapper petImageMapper)
        {
            if (petImageDataAccess == null)
                throw new ArgumentNullException(nameof(petImageDataAccess));

            if (petImageMapper == null)
                throw new ArgumentNullException(nameof(petImageMapper));

            _petImageDataAccess = petImageDataAccess;
            _petImageMapper = petImageMapper;
        }

        public async Task<PetImage> AddPetImageAsync(PetImageAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!request.PetId.HasValue && !request.PetCode.HasValue)
                throw new ArgumentException("PetId and PetCode are NULL");
            
            var table = _petImageMapper.MapAddRequestToTable(request);

            int newId = 0;
            if (request.PetId.HasValue)
                newId = await _petImageDataAccess.AddPetImageAsync(request.PetId.Value, table)
                                                .ConfigureAwait(false);
            else if (request.PetCode.HasValue)
                newId = await _petImageDataAccess.AddPetImageAsync(request.PetCode.Value, table)
                                            .ConfigureAwait(false);

            var newTable = await _petImageDataAccess.GetPetImageByIdAsync(newId)
                                                    .ConfigureAwait(false);

            return _petImageMapper.MapPetImageTableToPetImage(newTable);
        }

        public async Task<int> DeletePetImageAsync(PetImageDeleteRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!request.Id.HasValue && !request.Code.HasValue)
                throw new ArgumentException("PetId and PetCode are NULL");

            var result = 0;
            if (request.Id.HasValue)
                result = await _petImageDataAccess.DeletePetImageAsync(request.Id.Value)
                                                  .ConfigureAwait(false);
            else if (request.Code.HasValue)
                result = await _petImageDataAccess.DeletePetImageAsync(request.Code.Value)
                                                  .ConfigureAwait(false);

            return result;
        }
    }
}