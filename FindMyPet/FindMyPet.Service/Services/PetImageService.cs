using FindMyPet.DTO.Pet;
using FindMyPet.DTO.Shared;
using FindMyPet.MyServiceStack.Providers;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MyServiceStack.Services
{
    public class PetImageService : Service
    {
        private readonly IPetImageProvider _petImageProvider;

        public PetImageService(IPetImageProvider petImageProvider)
        {
            if (petImageProvider == null)
                throw new ArgumentNullException(nameof(petImageProvider));

            _petImageProvider = petImageProvider;
        }

        public async Task<PetImage> Post(PetImageAddRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petImageProvider.AddPetImageAsync(request)
                                          .ConfigureAwait(false);
        }
    }
}