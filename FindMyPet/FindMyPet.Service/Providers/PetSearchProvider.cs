using FindMyPet.DTO.PetSearch;
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
    }
}