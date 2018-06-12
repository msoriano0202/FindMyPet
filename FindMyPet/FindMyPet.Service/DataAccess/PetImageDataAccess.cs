using ServiceStack.Data;
using ServiceStack.OrmLite;
using FindMyPet.TableModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IPetImageDataAccess
    {
        Task<int> AddPetImageAsync(int petId, PetImageTableModel petImageTable);
        Task<int> AddPetImageAsync(Guid petCode, PetImageTableModel petImageTable);
        Task<int> DeletePetImageAsync(int petId);
        Task<int> DeletePetImageAsync(Guid petCode);
        Task<PetImageTableModel> GetPetImageByIdAsync(int petImageId);
    }

    public class PetImageDataAccess : IPetImageDataAccess
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IBaseDataAccess<PetImageTableModel> _petImageBaseDataAccess;

        public PetImageDataAccess(IDbConnectionFactory dbConnectionFactory, IBaseDataAccess<PetImageTableModel> petImageBaseDataAccess)
        {
            if (dbConnectionFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionFactory));

            if (petImageBaseDataAccess == null)
                throw new ArgumentNullException(nameof(petImageBaseDataAccess));

            _dbConnectionFactory = dbConnectionFactory;
            _petImageBaseDataAccess = petImageBaseDataAccess;
        }

        public async Task<int> AddPetImageAsync(int petId, PetImageTableModel petImageTable)
        {
            petImageTable.PetTableModelId = petId;
            petImageTable.Code = Guid.NewGuid();
            petImageTable.CreatedOn = DateTime.Now;

            return await _petImageBaseDataAccess.AddAsync(petImageTable)
                                                .ConfigureAwait(false);
        }

        public async Task<int> AddPetImageAsync(Guid petCode, PetImageTableModel petImageTable)
        {
            petImageTable.Code = Guid.NewGuid();
            petImageTable.CreatedOn = DateTime.Now;

            long imagePetId;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    var pet = await dbConnection.SingleAsync<PetTableModel>(x => x.Code == petCode)
                                                .ConfigureAwait(false);

                    petImageTable.PetTableModelId = pet.Id;

                    if (petImageTable.IsProfileImage)
                        await dbConnection.UpdateOnlyAsync(new PetImageTableModel { IsProfileImage = false }, x => x.IsProfileImage, x => x.PetTableModelId == pet.Id)
                                          .ConfigureAwait(false);

                    imagePetId = await dbConnection.InsertAsync<PetImageTableModel>(petImageTable, selectIdentity: true)
                                        .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return (int)imagePetId;
        }

        public async Task<int> DeletePetImageAsync(int petId)
        {
            return await _petImageBaseDataAccess.DeleteByIdAsync(petId)
                                                .ConfigureAwait(false);
        }

        public async Task<int> DeletePetImageAsync(Guid petCode)
        {
            int records;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                records = await dbConnection.DeleteAsync<PetImageTableModel>(x => x.Code == petCode)
                                            .ConfigureAwait(false);
            }

            return records;
        }

        public async Task<PetImageTableModel> GetPetImageByIdAsync(int petImageId)
        {
            return await _petImageBaseDataAccess.GetByIdAsync(petImageId)
                                                .ConfigureAwait(false);
        }
    }
}