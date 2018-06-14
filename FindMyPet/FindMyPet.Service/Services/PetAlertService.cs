using FindMyPet.DTO.PetAlert;
using FindMyPet.DTO.Shared;
using FindMyPet.MyServiceStack.Providers;
using ServiceStack;
using System;
using System.Threading.Tasks;

namespace FindMyPet.MyServiceStack.Services
{
    public class PetAlertService : Service
    {
        private readonly IPetAlertProvider _petAlertProvider;

        public PetAlertService(IPetAlertProvider petAlertProvider)
        {
            if (petAlertProvider == null)
                throw new ArgumentNullException(nameof(petAlertProvider));

            _petAlertProvider = petAlertProvider;
        }

        public async Task<PetAlert> Post(PetAlertCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petAlertProvider.CreatePetAlertAsync(request)
                                          .ConfigureAwait(false);
        }
    }
}