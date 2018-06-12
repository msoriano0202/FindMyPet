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
    public class PetService : Service
    {
        private readonly IPetProvider _petProvider;

        public PetService(IPetProvider petProvider)
        {
            if (petProvider == null)
                throw new ArgumentNullException(nameof(petProvider));

            _petProvider = petProvider;
        }

        public async Task<Pet> Get(PetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petProvider.GetPetAsync(request)
                                     .ConfigureAwait(false);
        }

        public async Task<Pet> Post(PetCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petProvider.CreatePetAsync(request)
                                     .ConfigureAwait(false);
        }

        public async Task<Pet> Put(PetUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petProvider.UpdatePetAsync(request)
                                     .ConfigureAwait(false);
        }

        public async Task<int> Delete(PetDeleteRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petProvider.DeletePetAsync(request)
                                     .ConfigureAwait(false);
        }

        public async Task<PagedResponse<Pet>> Post(PetsSearchByOwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petProvider.PetsByOwnerPagedAsync(request)
                                     .ConfigureAwait(false);
        }

        public async Task<int> Post(PetShareRequest request)
        {
            return await _petProvider.SharePetAsync(request)
                                     .ConfigureAwait(false);
        }

        //public async Task<List<Pet>> Post(SearchPetRequest request)
        //{
        //    if (request == null)
        //        throw new ArgumentNullException(nameof(request));

        //    return await _petProvider.SearchPetsAsync(request)
        //                             .ConfigureAwait(false);
        //}
    }
}