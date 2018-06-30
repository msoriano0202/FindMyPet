using FindMyPet.DTO.PetSearch;
using FindMyPet.DTO.Shared;
using FindMyPet.MyServiceStack.DataAccess;
using FindMyPet.MyServiceStack.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MyServiceStack.Providers
{
    public interface IPetSearchProvider
    {
        Task<List<PetLost>> GetPetLostByDateAsync(PetSearchByDateRequest request);
        Task<PagedResponse<PetLostAlert>> GetPetLostAlertsAsync(PetLastAlertsRequest request);
        Task<PetLostDetails> GetPetLostDetails(PetLostDetailsRequest request);
        Task<PagedResponse<PetSuccessStory>> GetPetSuccessStoriesAsync(PetSuccessStoryRequest request);
    }

    public class PetSearchProvider : IPetSearchProvider
    {
        private readonly IPetSearchDataAccess _petSearchDataAccess;
        private readonly IPetSearchMapper _petSearchMapper;

        public PetSearchProvider(IPetSearchDataAccess petSearchDataAccess, IPetSearchMapper petSearchMapper)
        {
            if (petSearchDataAccess == null)
                throw new ArgumentNullException(nameof(petSearchDataAccess));

            if (petSearchMapper == null)
                throw new ArgumentNullException(nameof(petSearchMapper));

            _petSearchDataAccess = petSearchDataAccess;
            _petSearchMapper = petSearchMapper;
        }

        public async Task<List<PetLost>> GetPetLostByDateAsync(PetSearchByDateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petSearchDataAccess.GetPetLostByDateAsync(request)
                                             .ConfigureAwait(false);
        }

        public async Task<PagedResponse<PetLostAlert>> GetPetLostAlertsAsync(PetLastAlertsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petSearchDataAccess.GetPetLostAlertsAsync(request)
                                             .ConfigureAwait(false);
        }

        public async Task<PetLostDetails> GetPetLostDetails(PetLostDetailsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!request.PetId.HasValue && !request.PetCode.HasValue)
                throw new ArgumentException("Id and Code are NULL");

            return await _petSearchDataAccess.GetPetLostDetails(request)
                                             .ConfigureAwait(false);
        }

        public async Task<PagedResponse<PetSuccessStory>> GetPetSuccessStoriesAsync(PetSuccessStoryRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return await _petSearchDataAccess.GetPetSuccessStoriesAsync(request)
                                             .ConfigureAwait(false);
        }
    }
}