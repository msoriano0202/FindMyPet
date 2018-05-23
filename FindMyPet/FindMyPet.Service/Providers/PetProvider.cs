using FindMyPet.DTO.Owner;
using FindMyPet.DTO.Pet;
using FindMyPet.MyServiceStack.DataAccess;
using FindMyPet.MyServiceStack.Mappers;
using FindMyPet.TableModel;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FindMyPet.MyServiceStack.Providers
{
    public interface IPetProvider
    {
        Task<Pet> CreatePetAsync(CreatePetRequest request);
        Task<Pet> UpdatePetAsync(UpdatePetRequest request);
        Task<Pet> GetPetAsync(PetRequest request);
        Task<List<Pet>> PetsByOwnerAsync(PetsByOwnerRequest request);
        Task<List<Pet>> SearchPetsAsync(SearchPetRequest request);
    }

    public class PetProvider : IPetProvider
    {
        private readonly IPetDataAccess _petDataAccess;
        private readonly IPetMapper _petMapper;

        public PetProvider(IPetDataAccess petDataAccess, IPetMapper petMapper)
        {
            if (petDataAccess == null)
                throw new ArgumentNullException(nameof(petDataAccess));

            if (petMapper == null)
                throw new ArgumentNullException(nameof(petMapper));

            _petDataAccess = petDataAccess;
            _petMapper = petMapper;
        }

        public async Task<Pet> CreatePetAsync(CreatePetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var table = _petMapper.MapCreateRequestToTable(request);

            var newId = await _petDataAccess.AddPetAsync(request.OwnerId, table)
                                            .ConfigureAwait(false);

            var newTable = await _petDataAccess.GetPetByIdAsync(newId)
                                               .ConfigureAwait(false);

            return _petMapper.MapPetTableToPet(newTable);
        }

        public async Task<Pet> UpdatePetAsync(UpdatePetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            PetTable table = null;

            if(request.Id.HasValue)
                table = await _petDataAccess.GetPetByIdAsync(request.Id.Value)
                                            .ConfigureAwait(false);
            else if(request.Code.HasValue)
                table = await _petDataAccess.GetPetByCodeAsync(request.Code.Value)
                                            .ConfigureAwait(false);

            table = _petMapper.MapUpdateRequestToTable(request, table);
            await _petDataAccess.UpdatePetAsync(table);

            var updatedTable = await _petDataAccess.GetPetByIdAsync(table.Id)
                                                   .ConfigureAwait(false);

            return _petMapper.MapPetTableToPet(updatedTable);
        }

        public async Task<Pet> GetPetAsync(PetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            PetTable table = null;
            if(request.Id.HasValue)
                table = await _petDataAccess.GetPetByIdAsync(request.Id.Value)
                                            .ConfigureAwait(false);
            else if(request.Code.HasValue)
                table = await _petDataAccess.GetPetByCodeAsync(request.Code.Value)
                                            .ConfigureAwait(false);

            return _petMapper.MapPetTableToPet(table);
        }

        public async Task<List<Pet>> PetsByOwnerAsync(PetsByOwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var petsByOwner = await _petDataAccess.GetPetsByOwnerIdAsync(request.OwnerId)
                                                  .ConfigureAwait(false);

            return petsByOwner.ConvertAll(p => _petMapper.MapPetTableToPet(p))
                              .OrderBy(p => p.Name).ToList();
        }

        public async Task<List<Pet>> SearchPetsAsync(SearchPetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            Expression<Func<PetTable, bool>> exp = (x) => x.Id > 0;

            if (!string.IsNullOrEmpty(request.Name))
                exp = exp.And<PetTable>(x => x.Name.Contains(request.Name));

            var response = await _petDataAccess.SearchPetsAsync(exp);

            return response.ConvertAll(pet => _petMapper.MapPetTableToPet(pet));
        }
    }
}