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
        Task<int> SetPetImageAsDefaultAsync(int petImageId);
        Task<int> SetPetImageAsDefaultAsync(Guid petImageCode);
        Task<int> DeletePetImageAsync(int petImageId);
        Task<int> DeletePetImageAsync(Guid petImageCode);
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

        public async Task<int> SetPetImageAsDefaultAsync(int petImageId)
        {
            int result;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    var petImage = await dbConnection.SingleAsync<PetImageTableModel>(x => x.Id == petImageId)
                                                     .ConfigureAwait(false);

                    await dbConnection.UpdateOnlyAsync(new PetImageTableModel { IsProfileImage = false }, x => x.IsProfileImage, x => x.PetTableModelId == petImage.PetTableModelId)
                                      .ConfigureAwait(false);

                    result = await dbConnection.UpdateOnlyAsync(new PetImageTableModel { IsProfileImage = true }, x => x.IsProfileImage, x => x.Id == petImageId)
                                               .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return result;
        }

        public async Task<int> SetPetImageAsDefaultAsync(Guid petImageCode)
        {
            int result;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    var petImage = await dbConnection.SingleAsync<PetImageTableModel>(x => x.Code == petImageCode)
                                                     .ConfigureAwait(false);

                    await dbConnection.UpdateOnlyAsync(new PetImageTableModel { IsProfileImage = false }, x => x.IsProfileImage, x => x.PetTableModelId == petImage.PetTableModelId)
                                      .ConfigureAwait(false);

                    result = await dbConnection.UpdateOnlyAsync(new PetImageTableModel { IsProfileImage = true }, x => x.IsProfileImage, x => x.Id == petImage.Id)
                                               .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return result;
        }

        public async Task<int> DeletePetImageAsync(int petImageId)
        {
            return await _petImageBaseDataAccess.DeleteByIdAsync(petImageId)
                                                .ConfigureAwait(false);
        }

        public async Task<int> DeletePetImageAsync(Guid petImageCode)
        {
            int records;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                records = await dbConnection.DeleteAsync<PetImageTableModel>(x => x.Code == petImageCode)
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