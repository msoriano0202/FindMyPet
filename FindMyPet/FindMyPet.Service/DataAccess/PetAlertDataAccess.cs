using FindMyPet.TableModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IPetAlertDataAccess
    {
        Task<int> AddPetAlertAsync(PetAlertTableModel petAlertTable);
        Task<PetAlertTableModel> GetPetAlertByIdAsync(int petAlertId);
        Task<PetAlertTableModel> GetPetAlertByCodeAsync(Guid petAlertCode);
    }

    public class PetAlertDataAccess : IPetAlertDataAccess
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IBaseDataAccess<PetAlertTableModel> _petAlertBaseDataAccess;

        public PetAlertDataAccess(IDbConnectionFactory dbConnectionFactory, IBaseDataAccess<PetAlertTableModel> petAlertBaseDataAccess)
        {
            if (dbConnectionFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionFactory));

            if (petAlertBaseDataAccess == null)
                throw new ArgumentNullException(nameof(petAlertBaseDataAccess));

            _dbConnectionFactory = dbConnectionFactory;
            _petAlertBaseDataAccess = petAlertBaseDataAccess;
        }

        public async Task<int> AddPetAlertAsync(PetAlertTableModel petAlertTable)
        {
            return await _petAlertBaseDataAccess.AddAsync(petAlertTable)
                                                .ConfigureAwait(false);
        }

        public async Task<PetAlertTableModel> GetPetAlertByIdAsync(int petAlertId)
        {
            return await _petAlertBaseDataAccess.GetByIdAsync(petAlertId)
                                                .ConfigureAwait(false);
        }

        public async Task<PetAlertTableModel> GetPetAlertByCodeAsync(Guid petAlertCode)
        {
            var petAlert = new PetAlertTableModel();

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                petAlert = await dbConnection.SingleAsync<PetAlertTableModel>(p => p.Code == petAlertCode)
                                             .ConfigureAwait(false);

                await dbConnection.LoadReferencesAsync(petAlert);
            }

            return petAlert;
        }
    }
}