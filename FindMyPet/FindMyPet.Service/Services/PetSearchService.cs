using FindMyPet.DTO.PetAlert;
using FindMyPet.DTO.PetSearch;
using FindMyPet.DTO.Shared;
using FindMyPet.MyServiceStack.Providers;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMyPet.MyServiceStack.Services
{
    public class PetSearchService : Service
    {
        private readonly IPetSearchProvider _petSearchProvider;

        public PetSearchService(IPetSearchProvider petSearchProvider)
        {
            if (petSearchProvider == null)
                throw new ArgumentNullException(nameof(petSearchProvider));

            _petSearchProvider = petSearchProvider;
        }

        public async Task<List<PetLost>> Post(PetSearchByDateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petSearchProvider.GetPetLostByDateAsync(request)
                                           .ConfigureAwait(false);
        }

        public async Task<PetLostDetails> Post(PetLostDetailsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petSearchProvider.GetPetLostDetails(request)
                                           .ConfigureAwait(false);
        }

        public async Task<PagedResponse<PetSuccessStory>> Get(PetSuccessStoryRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petSearchProvider.GetPetSuccessStoriesAsync(request)
                                           .ConfigureAwait(false);
        }
    }
}