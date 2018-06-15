using FindMyPet.DTO.PetAlert;
using FindMyPet.Shared;
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
        Task<int> FoundPet(int petId, string foundComment, bool makeItPublic);
        Task<PetAlertTableModel> GetPetAlertByIdAsync(int petAlertId);
        Task<PetAlertTableModel> GetPetAlertByCodeAsync(Guid petAlertCode);
        Task<List<PetAlert>> GetPetAlertsByPetIdAsync(int petId);
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
            long newId;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction())
                {
                    var pet = await dbConnection.SingleAsync<PetTableModel>(p => p.Id == petAlertTable.PetId)
                                                .ConfigureAwait(false);

                    newId = await dbConnection.InsertAsync<PetAlertTableModel>(petAlertTable, selectIdentity: true)
                                              .ConfigureAwait(false);

                    await dbConnection.UpdateOnlyAsync(new PetTableModel { Status = (int)PetStatusEnum.Lost }, x => x.Status, x => x.Id == pet.Id)
                                      .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return (int)newId;
        }

        public async Task<int> FoundPet(int petId, string foundComment, bool makeItPublic)
        {
            var petAlertId = 0;

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction())
                {
                    var pet = await dbConnection.SingleAsync<PetTableModel>(p => p.Id == petId)
                                                .ConfigureAwait(false);

                    var petAlert = await dbConnection.SingleAsync<PetAlertTableModel>(a => a.PetId.HasValue && a.PetId == petId &&
                                                                                           a.AlertType == (int)AlertTypeEnum.Lost &&
                                                                                           a.AlertStatus == (int)AlertStatusEnum.Active)
                                                     .ConfigureAwait(false);

                    petAlert.CommentFound = foundComment;
                    petAlert.AlertStatus = (int)AlertStatusEnum.Deleted;
                    petAlert.MakeItPublic = makeItPublic;
                    petAlert.SolvedOn = System.DateTime.Now;
                    await dbConnection.UpdateAsync(petAlert).ConfigureAwait(false);

                    await dbConnection.UpdateOnlyAsync(new PetTableModel { Status = (int)PetStatusEnum.Found }, x => x.Status, x => x.Id == pet.Id)
                                      .ConfigureAwait(false);

                    petAlertId = petAlert.Id;

                    trans.Commit();
                }
            }

            return petAlertId;
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

        public async Task<List<PetAlert>> GetPetAlertsByPetIdAsync(int petId)
        {
            var alerts = new List<PetAlert>();

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                var alertsTable = await dbConnection.SelectAsync<PetAlertTableModel>(p => p.PetId.HasValue && p.PetId.Value == petId)
                                                    .ConfigureAwait(false);

                if (alertsTable.Any())
                {
                    alerts = alertsTable.ConvertAll(a => new PetAlert {
                        Id = a.Id,
                        Code = a.Code,
                        Type = a.AlertType,
                        CreatedOn = a.CreatedOn,
                        SolvedOn = a.SolvedOn,
                        Status = a.AlertStatus
                    });
                }
            }

            return alerts;
        }
    }
}