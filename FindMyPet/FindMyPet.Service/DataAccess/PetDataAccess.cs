using FindMyPet.TableModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IPetDataAccess
    {
        Task<int> AddPetAsync(int ownerId, PetTable petTable);
        Task<int> UpdatePetAsync(PetTable petTable);
        Task<PetTable> GetPetByIdAsync(int petId);
        Task<PetTable> GetPetByCodeAsync(Guid petCode);
        Task<List<PetTable>> GetPetsByOwnerIdAsync(int ownerId);
        Task<List<PetTable>> SearchPetsAsync(Expression<Func<PetTable, bool>> predicate);
    }

    public class PetDataAccess : IPetDataAccess
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IBaseDataAccess<PetTable> _petBaseDataAccess;

        public PetDataAccess(IDbConnectionFactory dbConnectionFactory, IBaseDataAccess<PetTable> petBaseDataAccess)
        {
            if (dbConnectionFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionFactory));

            if (petBaseDataAccess == null)
                throw new ArgumentNullException(nameof(petBaseDataAccess));

            _dbConnectionFactory = dbConnectionFactory;
            _petBaseDataAccess = petBaseDataAccess;
        }

        public async Task<int> AddPetAsync(int ownerId, PetTable petTable)
        {
            petTable.Code = Guid.NewGuid();
            petTable.CreatedOn = DateTime.Now;

            long petId;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    petId = await dbConnection.InsertAsync<PetTable>(petTable, selectIdentity: true)
                                              .ConfigureAwait(false);

                    var ownerPetTable = new OwnerPetTable
                    {
                        OwnerTableId = ownerId,
                        PetTableId = (int)petId,
                        CreatedOn = DateTime.Now
                    };

                    await dbConnection.InsertAsync<OwnerPetTable>(ownerPetTable, selectIdentity: true)
                                      .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return (int)petId;
        }

        public async Task<int> UpdatePetAsync(PetTable petTable)
        {
            return await _petBaseDataAccess.UpdateAsync(petTable);
        }

        public async Task<PetTable> GetPetByIdAsync(int petId)
        {
            return await _petBaseDataAccess.GetByIdAsync(petId);
        }

        public async Task<PetTable> GetPetByCodeAsync(Guid petCode)
        {
            var pet = new PetTable();

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                pet = await dbConnection.SingleAsync<PetTable>(p => p.Code == petCode)
                                        .ConfigureAwait(false);
            }

            return pet;
        }

        public async Task<List<PetTable>> GetPetsByOwnerIdAsync(int ownerId)
        {
            var petsByOwner = new List<PetTable>();
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                var q = dbConnection.From<PetTable>()
                         .Join<PetTable, OwnerPetTable>((p, po) => p.Id == po.PetTableId && po.OwnerTableId == ownerId)
                         .Select().OrderBy(x => x.Name);
                petsByOwner = await dbConnection.SelectAsync<PetTable>(q);
            }

            return petsByOwner;
        }

        public async Task<List<PetTable>> SearchPetsAsync(Expression<Func<PetTable, bool>> predicate)
        {
            return await _petBaseDataAccess.GetListFilteredAsync(predicate);
        }
    }
}