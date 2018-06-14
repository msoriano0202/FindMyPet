using FindMyPet.DTO.Owner;
using FindMyPet.DTO.Pet;
using FindMyPet.DTO.Shared;
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
        Task<Pet> GetPetAsync(PetRequest request);
        Task<Pet> CreatePetAsync(PetCreateRequest request);
        Task<Pet> UpdatePetAsync(PetUpdateRequest request);
        Task<int> DeletePetAsync(PetDeleteRequest request);
        Task<int> SharePetAsync(PetShareRequest request);
        Task<PagedResponse<Pet>> PetsByOwnerPagedAsync(PetsSearchByOwnerRequest request);
        //Task<List<Pet>> PetsByOwnerAsync(PetsByOwnerRequest request);
        //Task<List<Pet>> SearchPetsAsync(SearchPetRequest request);
    }

    public class PetProvider : IPetProvider
    {
        private readonly IPetDataAccess _petDataAccess;
        private readonly IPetMapper _petMapper;
        private readonly IPetAlertDataAccess _petAlertDataAccess;

        public PetProvider(IPetDataAccess petDataAccess, IPetMapper petMapper, IPetAlertDataAccess petAlertDataAccess)
        {
            if (petDataAccess == null)
                throw new ArgumentNullException(nameof(petDataAccess));

            if (petMapper == null)
                throw new ArgumentNullException(nameof(petMapper));

            if (petAlertDataAccess == null)
                throw new ArgumentNullException(nameof(petAlertDataAccess));

            _petDataAccess = petDataAccess;
            _petMapper = petMapper;
            _petAlertDataAccess = petAlertDataAccess;
        }

        public async Task<Pet> GetPetAsync(PetRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!request.Id.HasValue && request.Code == null)
                throw new ArgumentException("Id and Code are NULL");

            PetTableModel table = null;
            if (request.Id.HasValue)
                table = await _petDataAccess.GetPetByIdAsync(request.Id.Value)
                                            .ConfigureAwait(false);
            else if (request.Code.HasValue)
                table = await _petDataAccess.GetPetByCodeAsync(request.Code.Value)
                                            .ConfigureAwait(false);

            var owners = await _petDataAccess.GetOwnersByPetIdAsync(table.Id)
                                             .ConfigureAwait(false);

            var alerts = await _petAlertDataAccess.GetPetAlertsByPetIdAsync(table.Id)
                                                  .ConfigureAwait(false);

            var pet = _petMapper.MapPetTableToPet(table, true);
            pet.Owners = owners;
            pet.Alerts = alerts;

            return pet;
        }

        public async Task<Pet> CreatePetAsync(PetCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!request.OwnerId.HasValue && string.IsNullOrEmpty(request.OwnerMembershipId))
                throw new ArgumentException("OwnerId and OwnerMembershipId are NULL");

            var table = _petMapper.MapCreateRequestToTable(request);

            int newId = 0;
            if(request.OwnerId.HasValue)
                newId = await _petDataAccess.AddPetAsync(request.OwnerId.Value, table)
                                                .ConfigureAwait(false);
            else if(!string.IsNullOrEmpty(request.OwnerMembershipId))
                newId = await _petDataAccess.AddPetAsync(request.OwnerMembershipId, table)
                                            .ConfigureAwait(false);

            var newTable = await _petDataAccess.GetPetByIdAsync(newId)
                                               .ConfigureAwait(false);

            return _petMapper.MapPetTableToPet(newTable, false);
        }

        public async Task<Pet> UpdatePetAsync(PetUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            PetTableModel table = null;

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

            return _petMapper.MapPetTableToPet(updatedTable, false);
        }

        public async Task<int> DeletePetAsync(PetDeleteRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!request.Id.HasValue && !request.Code.HasValue)
                throw new ArgumentException("Id and Code are NULL");

            int records = 0;
            if (request.Id.HasValue)
                records = await _petDataAccess.DeletePetAsync(request.Id.Value)
                                              .ConfigureAwait(false);
            else if (request.Code.HasValue)
                records = await _petDataAccess.DeletePetAsync(request.Code.Value)
                                              .ConfigureAwait(false);

            return records;
        }

        public async Task<int> SharePetAsync(PetShareRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrEmpty(request.PetCode) &&  string.IsNullOrEmpty(request.OwnerMembershipId))
                throw new ArgumentException("PetCode and UserCode are NULL");

            var records = await _petDataAccess.SharePetAsync(request.PetCode, request.OwnerMembershipId)
                                              .ConfigureAwait(false);

            return records;
        }

        public async Task<PagedResponse<Pet>> PetsByOwnerPagedAsync(PetsSearchByOwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var pagedResult = await _petDataAccess.GetPetsByOwnerPagedAsync(request)
                                                  .ConfigureAwait(false);

            return new PagedResponse<Pet>
            {
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = pagedResult.TotalPages,
                Result = pagedResult.Result.ConvertAll(p => _petMapper.MapPetTableToPet(p, false))
            };
        }

        //public async Task<List<Pet>> PetsByOwnerAsync(PetsByOwnerRequest request)
        //{
        //    if (request == null)
        //        throw new ArgumentNullException(nameof(request));

        //    var petsByOwner = await _petDataAccess.GetPetsByOwnerIdAsync(request.OwnerId)
        //                                          .ConfigureAwait(false);

        //    return petsByOwner.ConvertAll(p => _petMapper.MapPetTableToPet(p));
        //}

        //public async Task<List<Pet>> SearchPetsAsync(SearchPetRequest request)
        //{
        //    if (request == null)
        //        throw new ArgumentNullException(nameof(request));

        //    Expression<Func<PetTableModel, bool>> exp = (x) => x.Id > 0;

        //    if (!string.IsNullOrEmpty(request.Name))
        //        exp = exp.And<PetTableModel>(x => x.Name.Contains(request.Name));

        //    var response = await _petDataAccess.SearchPetsAsync(exp);

        //    return response.ConvertAll(pet => _petMapper.MapPetTableToPet(pet));
        //}
    }
}