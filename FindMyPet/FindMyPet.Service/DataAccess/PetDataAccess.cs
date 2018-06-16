using FindMyPet.TableModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FindMyPet.DTO.Shared;
using FindMyPet.DTO.Pet;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IPetDataAccess
    {
        Task<int> AddPetAsync(int ownerId, PetTableModel petTable);
        Task<int> AddPetAsync(string ownerMembershipId, PetTableModel petTable);
        Task<int> UpdatePetAsync(PetTableModel petTable);
        Task<int> DeletePetAsync(int id);
        Task<int> DeletePetAsync(Guid code);
        Task<string> CreateSharePetAsync(PetShareCreateRequest request);
        Task<int> ConfirmSharePetAsync(PetShareConfirmRequest request);
        Task<PetTableModel> GetPetByIdAsync(int petId);
        Task<PetTableModel> GetPetByCodeAsync(Guid petCode);
        Task<List<PetOwner>> GetOwnersByPetIdAsync(int petId);
        Task<List<PetTableModel>> GetPetsByOwnerIdAsync(int ownerId);
        Task<PagedResponse<PetTableModel>> GetPetsByOwnerPagedAsync(PetsSearchByOwnerRequest request);
        Task<List<PetTableModel>> SearchPetsAsync(Expression<Func<PetTableModel, bool>> predicate);
    }

    public class PetDataAccess : IPetDataAccess
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IBaseDataAccess<PetTableModel> _petBaseDataAccess;

        public PetDataAccess(IDbConnectionFactory dbConnectionFactory, IBaseDataAccess<PetTableModel> petBaseDataAccess)
        {
            if (dbConnectionFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionFactory));

            if (petBaseDataAccess == null)
                throw new ArgumentNullException(nameof(petBaseDataAccess));

            _dbConnectionFactory = dbConnectionFactory;
            _petBaseDataAccess = petBaseDataAccess;
        }

        private OwnerPetTableModel GetNewOwnerPetTableModel(int ownerId, int petId, bool firstOwner)
        {
            return new OwnerPetTableModel
            {
                Code = Guid.NewGuid(),
                OwnerTableModelId = ownerId,
                PetTableModelId = petId,
                IsFirstOwner = firstOwner,
                CreatedOn = DateTime.Now
            };
        }

        public async Task<int> AddPetAsync(int ownerId, PetTableModel petTable)
        {
            petTable.Code = Guid.NewGuid();
            petTable.CreatedOn = DateTime.Now;

            long petId;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    petId = await dbConnection.InsertAsync<PetTableModel>(petTable, selectIdentity: true)
                                              .ConfigureAwait(false);

                    var ownerPetTable = GetNewOwnerPetTableModel(ownerId, (int)petId, true);
                    //var ownerPetTable = new OwnerPetTableModel
                    //{
                    //    Code = Guid.NewGuid(),
                    //    OwnerTableModelId = ownerId,
                    //    PetTableModelId = (int)petId,
                    //    IsFirstOwner = true,
                    //    CreatedOn = DateTime.Now
                    //};

                    await dbConnection.InsertAsync<OwnerPetTableModel>(ownerPetTable, selectIdentity: true)
                                      .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return (int)petId;
        }

        public async Task<int> AddPetAsync(string ownerMembershipId, PetTableModel petTable)
        {
            petTable.Code = Guid.NewGuid();
            petTable.CreatedOn = DateTime.Now;

            long petId;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    var owner = await dbConnection.SingleAsync<OwnerTableModel>(x => x.MembershipId == ownerMembershipId)
                                                  .ConfigureAwait(false);

                    petId = await dbConnection.InsertAsync<PetTableModel>(petTable, selectIdentity: true)
                                              .ConfigureAwait(false);

                    var ownerPetTable = GetNewOwnerPetTableModel(owner.Id, (int)petId, true);
                    //var ownerPetTable = new OwnerPetTableModel
                    //{
                    //    Code = Guid.NewGuid(),
                    //    OwnerTableModelId = owner.Id,
                    //    PetTableModelId = (int)petId,
                    //    IsFirstOwner = true,
                    //    CreatedOn = DateTime.Now
                    //};

                    await dbConnection.InsertAsync<OwnerPetTableModel>(ownerPetTable, selectIdentity: true)
                                      .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return (int)petId;
        }

        public async Task<int> UpdatePetAsync(PetTableModel petTable)
        {
            return await _petBaseDataAccess.UpdateAsync(petTable)
                                           .ConfigureAwait(false);
        }

        public async Task<int> DeletePetAsync(int id)
        {
            return await _petBaseDataAccess.DeleteByIdAsync(id)
                                           .ConfigureAwait(false);
        }

        public async Task<int> DeletePetAsync(Guid code)
        {
            var records = 0;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    var pet = await dbConnection.SingleAsync<PetTableModel>(x => x.Code == code)
                                                .ConfigureAwait(false);

                    await dbConnection.DeleteAsync<OwnerPetTableModel>(x => x.PetTableModelId == pet.Id)
                                      .ConfigureAwait(false);

                    records = await dbConnection.DeleteAsync<PetTableModel>(p => p.Code == code)
                                    .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return records;
        }

        public async Task<string> CreateSharePetAsync(PetShareCreateRequest request)
        {
            long records = 0;
            PetTableModel pet;
            var token = Guid.NewGuid();

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    if (request.PetId.HasValue)
                        pet = await dbConnection.SingleByIdAsync<PetTableModel>(request.PetId.Value)
                                                .ConfigureAwait(false);
                    else
                        pet = await dbConnection.SingleAsync<PetTableModel>(x => x.Code == request.PetCode)
                                                .ConfigureAwait(false);

                    var newtable = new OwnerSharedPetTableModel
                    {
                        OwnerTableModelId = request.OwnerId,
                        PetTableModelId = pet.Id,
                        TokenCode = token,
                        ToOwnerEmail = request.ToOwnerEmail,
                        Used = false,
                        CreatedOn = DateTime.Now
                    };

                    records = await dbConnection.InsertAsync<OwnerSharedPetTableModel>(newtable, selectIdentity: true)
                                                .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return token.ToString();
        }

        public async Task<int> ConfirmSharePetAsync(PetShareConfirmRequest request)
        {
            long records = 0;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    var requestRecord = await dbConnection.SingleAsync<OwnerSharedPetTableModel>(x => x.TokenCode == request.Token && !x.Used)
                                                          .ConfigureAwait(false);

                    if (requestRecord != null)
                    {
                        var pet = await dbConnection.SingleAsync<PetTableModel>(x => x.Id == requestRecord.PetTableModelId)
                                                .ConfigureAwait(false);

                        var owner = await dbConnection.SingleAsync<OwnerTableModel>(x => x.Email == requestRecord.ToOwnerEmail)
                                                      .ConfigureAwait(false);

                        var ownerPetTable = GetNewOwnerPetTableModel(owner.Id, pet.Id, false);
                        //var ownerPetTable = new OwnerPetTableModel
                        //{
                        //    Code = Guid.NewGuid(),
                        //    OwnerTableModelId = owner.Id,
                        //    PetTableModelId = pet.Id,
                        //    IsFirstOwner = false,
                        //    CreatedOn = DateTime.Now
                        //};

                        records = await dbConnection.InsertAsync<OwnerPetTableModel>(ownerPetTable, selectIdentity: true)
                                                    .ConfigureAwait(false);

                        await dbConnection.UpdateOnlyAsync(new OwnerSharedPetTableModel { Used = true }, x => x.Used, x => x.Id == requestRecord.Id)
                                          .ConfigureAwait(false);
                        await dbConnection.UpdateOnlyAsync(new OwnerSharedPetTableModel { UsedOn = System.DateTime.Now }, x => x.UsedOn, x => x.Id == requestRecord.Id)
                                          .ConfigureAwait(false);

                        trans.Commit();
                    }
                }
            }

            return (int)records;
        }

        public async Task<PetTableModel> GetPetByIdAsync(int petId)
        {
            return await _petBaseDataAccess.GetByIdAsync(petId)
                                           .ConfigureAwait(false);
        }

        public async Task<PetTableModel> GetPetByCodeAsync(Guid petCode)
        {
            var pet = new PetTableModel();

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                pet = await dbConnection.SingleAsync<PetTableModel>(p => p.Code == petCode)
                                        .ConfigureAwait(false);

                await dbConnection.LoadReferencesAsync(pet);
            }

            return pet;
        }

        public async Task<List<PetOwner>> GetOwnersByPetIdAsync(int petId)
        {
            var petOwners = new List<PetOwner>();

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                var q = dbConnection.From<OwnerTableModel>()
                                    .Join<OwnerTableModel, OwnerPetTableModel>((o, op) => o.Id == op.OwnerTableModelId && op.PetTableModelId == petId);

                var results = await dbConnection.SelectMultiAsync<OwnerTableModel, OwnerPetTableModel>(q)
                                                .ConfigureAwait(false);

                if (results.Any())
                {
                    foreach (var item in results)
                    {
                        petOwners.Add(new PetOwner {
                            FullName = $"{item.Item1.FirstName} {item.Item1.LastName}",
                            ProfileImageUrl = item.Item1.ProfileImageUrl,
                            RegisteredDate = item.Item2.CreatedOn
                        });
                    }
                }
            }

            return petOwners;
        }

        public async Task<List<PetTableModel>> GetPetsByOwnerIdAsync(int ownerId)
        {
            var petsByOwner = new List<PetTableModel>();
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                var q = dbConnection.From<PetTableModel>()
                                    .Join<PetTableModel, OwnerPetTableModel>((p, po) => p.Id == po.PetTableModelId && po.OwnerTableModelId == ownerId)
                                    .Select().OrderBy(x => x.Name);

                petsByOwner = await dbConnection.SelectAsync<PetTableModel>(q)
                                                .ConfigureAwait(false);
            }

            return petsByOwner;
        }

        public async Task<PagedResponse<PetTableModel>> GetPetsByOwnerPagedAsync(PetsSearchByOwnerRequest request)
        {
            var response = new PagedResponse<PetTableModel>();
            var records = new List<PetTableModel>();
            int totalRecords = 0;
            int totalPages = 0;

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                var q = dbConnection.From<PetTableModel>()
                                    .Join<PetTableModel, OwnerPetTableModel>((p, po) => p.Id == po.PetTableModelId && po.OwnerTableModelId == request.OwnerId)
                                    .Select().OrderBy(x => x.Name);

                //Micky: Place this logic into a helper to be available to all clases
                totalRecords = await dbConnection.SqlScalarAsync<int>(q.ToCountStatement());
                if (
                    (request.PageSize.HasValue && request.PageNumber.HasValue) && 
                    totalRecords > request.PageSize
                   )
                {
                    totalPages = (int)((totalRecords + (request.PageSize - 1)) / request.PageSize);

                    if (request.PageNumber <= 1)
                    {
                        request.PageNumber = 1;
                        q = q.Take(request.PageSize);
                    }
                    else
                    {
                        if (request.PageNumber > totalPages)
                            request.PageNumber = totalPages;

                        q = q.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
                    }

                    records = await dbConnection.SelectAsync<PetTableModel>(q)
                                                .ConfigureAwait(false);
                }
                else
                {
                    totalPages = 1;
                    records = await dbConnection.SelectAsync<PetTableModel>(q)
                                                .ConfigureAwait(false);
                }

                if (records.Any())
                    foreach (var item in records)
                    {
                        await dbConnection.LoadReferencesAsync(item);
                    }
            }

            response.TotalRecords = totalRecords;
            response.TotalPages = totalPages;
            response.Result = records;

            return response;
        }

        public async Task<List<PetTableModel>> SearchPetsAsync(Expression<Func<PetTableModel, bool>> predicate)
        {
            return await _petBaseDataAccess.GetListFilteredAsync(predicate)
                                           .ConfigureAwait(false);
        }
    }
}