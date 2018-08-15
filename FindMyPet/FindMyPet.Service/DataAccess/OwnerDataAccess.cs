using FindMyPet.DTO.Owner;
using FindMyPet.MyServiceStack.Shared;
using FindMyPet.Shared;
using FindMyPet.TableModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IOwnerDataAccess
    {
        Task<int> AddOwnerAsync(OwnerTableModel ownerTable);
        Task<int> UpdateOwnerAsync(OwnerTableModel ownerTable);
        Task<OwnerTableModel> GetOwnerByIdAsync(int ownerId);
        Task<OwnerTableModel> GetOwnerByMembershipIdAsync(string membershipId);
        Task<List<OwnerTableModel>> SearchOwnersAsync(Expression<Func<OwnerTableModel, bool>> predicate);
        Task<List<OwnerAlert>> SearchOwnerAlertsAsync(OwnerAlertSearchRequest request);
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

        public async Task<List<OwnerAlert>> SearchOwnerAlertsAsync(OwnerAlertSearchRequest request)
        {
            var alerts = new List<OwnerAlert>();

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                OwnerTableModel ownerTable = null;
                if(request.Id.HasValue)
                    ownerTable = await dbConnection.SingleAsync<OwnerTableModel>(o => o.Id == request.Id.Value)
                                                   .ConfigureAwait(false);
                else if(!string.IsNullOrEmpty(request.MembershipId))
                    ownerTable = await dbConnection.SingleAsync<OwnerTableModel>(o => o.MembershipId == request.MembershipId)
                                                   .ConfigureAwait(false);
                if (ownerTable != null)
                {
                    var alertsQuery = dbConnection.From<PetAlertTableModel>()
                                                  .Join<PetAlertTableModel, OwnerTableModel>((pa, o) => pa.OwnerTableModelId == o.Id)
                                                  .LeftJoin<PetAlertTableModel, PetTableModel>((pa, p) => pa.PetId.HasValue && pa.PetId.Value == p.Id)
                                                  .LeftJoin<PetAlertTableModel, PetImageTableModel>((pa, pi) => pa.PetId == pi.PetTableModelId && pi.IsProfileImage)
                                                  .Where(pa => pa.OwnerTableModelId == ownerTable.Id && pa.AlertStatus == (int)AlertStatusEnum.Active)
                                                  .OrderByDescending(x => x.CreatedOn);

                    var alertsResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, OwnerTableModel, PetTableModel, PetImageTableModel>(alertsQuery)
                                                          .ConfigureAwait(false);

                    var imagesQuery = dbConnection.From<PetAlertTableModel>()
                                                  .Join<PetAlertTableModel, OwnerTableModel>((pa, o) => pa.OwnerTableModelId == o.Id)
                                                  .Join<PetAlertTableModel, PetAlertImageTableModel>((pa, pai) => pa.Id == pai.PetAlertTableModelId)
                                                  .Where(pa => pa.OwnerTableModelId == ownerTable.Id && pa.AlertStatus == (int)AlertStatusEnum.Active);

                    var imagesResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, OwnerTableModel, PetAlertImageTableModel>(imagesQuery)
                                                          .ConfigureAwait(false);

                    if (alertsResults.Any())
                        alerts = FormatAlertsResults(alertsResults);

                    if (imagesResults.Count > 0)
                    {
                        var groupedImages = imagesResults.GroupBy(x => x.Item1.Code).ToList();

                        foreach (var groupedImage in groupedImages)
                        {
                            var foundAlertImage = alerts.Where(a => a.AlertCode == groupedImage.Key);
                            if (foundAlertImage != null)
                                alerts.Find(r => r.AlertCode == groupedImage.Key).PetProfileImageUrl = groupedImage.FirstOrDefault(x => x.Item2.Id > 0).Item3.ImageUrl;
                        }
                    }
                }
            }

            return alerts;
        }

        private List<OwnerAlert> FormatAlertsResults(List<Tuple<PetAlertTableModel, OwnerTableModel, PetTableModel, PetImageTableModel>> alertsResults)
        {
            var ownerAlerts = new List<OwnerAlert>();

            OwnerAlert ownerAlert;
            foreach (var item in alertsResults)
            {
                ownerAlert = new OwnerAlert
                {
                    AlertCode = item.Item1.Code,
                    Type = item.Item1.AlertType,
                    Latitude = item.Item1.Latitude,
                    Longitude = item.Item1.Longitude,
                    LostDateTime = item.Item1.CreatedOn,
                    LostComment = item.Item1.Comment
                };

                if (item.Item3.Id != 0)
                {
                    ownerAlert.PetId = item.Item3.Id;
                    ownerAlert.PetCode = item.Item3.Code;
                    ownerAlert.PetName = item.Item3.Name;
                    ownerAlert.PetProfileImageUrl = item.Item4.ImageUrl;
                }
                else
                    ownerAlert.PetName = GeneralHelper.GetAnonymousTitle(item.Item1.AlertType);

                ownerAlerts.Add(ownerAlert);
            }

            return ownerAlerts;
        }
    }
}