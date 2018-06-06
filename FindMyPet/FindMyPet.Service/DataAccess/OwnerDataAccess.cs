using FindMyPet.TableModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IOwnerDataAccess
    {
        Task<int> AddOwnerAsync(OwnerTableModel ownerTable);
        Task<int> UpdateOwnerAsync(OwnerTableModel ownerTable);
        Task<OwnerTableModel> GetOwnerByIdAsync(int ownerId);
        Task<OwnerTableModel> GetOwnerByMembershipIdAsync(string membershipId);
        Task<List<OwnerTableModel>> SearchOwnersAsync(Expression<Func<OwnerTableModel, bool>> predicate);
    }

    public class OwnerDataAccess : IOwnerDataAccess
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IBaseDataAccess<OwnerTableModel> _ownerBaseDataAccess;
        private const int DefaultReceiveDistanceRadio = 1000;
        private const int DefaultSendDistanceRadio = 1000;

        public OwnerDataAccess(IDbConnectionFactory dbConnectionFactory, IBaseDataAccess<OwnerTableModel> ownerBaseDataAccess)
        {
            if (dbConnectionFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionFactory));

            if (ownerBaseDataAccess == null)
                throw new ArgumentNullException(nameof(ownerBaseDataAccess));

            _dbConnectionFactory = dbConnectionFactory;
            _ownerBaseDataAccess = ownerBaseDataAccess;
        }

        public async Task<int> AddOwnerAsync(OwnerTableModel ownerTable)
        {
            long newId;
            var now = DateTime.Now;

            ownerTable.Code = Guid.NewGuid();
            ownerTable.CreatedOn = now;

            var ownerSettingTableModel = new OwnerSettingTableModel
            {
                ShowEmailForAlerts = true,
                ShowPhoneNumberForAlerts = false,
                ShowAddressForAlerts = false,
                ReceiveAlertsAll = false,
                ReceiveAlertsInRadio = true,
                ReceiveDistanceRadio = DefaultReceiveDistanceRadio,
                SendDistanceRadio = DefaultSendDistanceRadio,
                CreatedOn = now
            };

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    newId = await dbConnection.InsertAsync<OwnerTableModel>(ownerTable, selectIdentity: true)
                                              .ConfigureAwait(false);

                    ownerSettingTableModel.OwnerTableModelId = (int)newId;
                    await dbConnection.InsertAsync<OwnerSettingTableModel>(ownerSettingTableModel)
                                      .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return (int)newId;
        }

        public async Task<int> UpdateOwnerAsync(OwnerTableModel ownerTable)
        {
            var rowsAffected = 0;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction(IsolationLevel.ReadCommitted))
                {
                    rowsAffected = await dbConnection.UpdateAsync<OwnerTableModel>(ownerTable).ConfigureAwait(false);
                    await dbConnection.UpdateAsync<OwnerSettingTableModel>(ownerTable.SettingTableModel).ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return rowsAffected;
        }

        public async Task<OwnerTableModel> GetOwnerByIdAsync(int ownerId)
        {
            return await _ownerBaseDataAccess.GetByIdAsync(ownerId);
        }

        public async Task<OwnerTableModel> GetOwnerByMembershipIdAsync(string membershipId)
        {
            OwnerTableModel ownerTableModel;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                ownerTableModel = await dbConnection.SingleAsync<OwnerTableModel>(x => x.MembershipId == membershipId)
                                                    .ConfigureAwait(false);

                await dbConnection.LoadReferencesAsync(ownerTableModel);
            }

            return ownerTableModel;
        }

        public async Task<List<OwnerTableModel>> SearchOwnersAsync(Expression<Func<OwnerTableModel, bool>> predicate)
        {
            return await _ownerBaseDataAccess.GetListFilteredAsync(predicate);
        }
    }
}