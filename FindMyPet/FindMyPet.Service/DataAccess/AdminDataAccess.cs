using FindMyPet.DTO.Admin;
using FindMyPet.TableModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FindMyPet.Shared;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IAdminDataAccess
    {
        Task<List<AdminFoundAlert>> GetAdminFoundAlertsAsync();
        Task<int> ManageAdminFoundAlertsAsync(AdminManageFoundAlertRequest request);
        Task<AdminDashboardDetails> GetAdminDashboardAsync();
        Task<List<AdminReportedAlert>> GetAdminReportedAlertsAsync();
        Task<int> ManageReportedAlertsAsync(AdminManageReportedAlertRequest request);
    }

    public class AdminDataAccess : IAdminDataAccess
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public AdminDataAccess(IDbConnectionFactory dbConnectionFactory)
        {
            if (dbConnectionFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionFactory));

            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<AdminFoundAlert>> GetAdminFoundAlertsAsync()
        {
            var results = new List<AdminFoundAlert>();
            using (var dbConnection = _dbConnectionFactory.Open())
            {

                var query = dbConnection.From<PetAlertTableModel>()
                                        .Join<PetAlertTableModel, PetTableModel>((pa,p) => pa.PetId == p.Id)
                                        .Join<PetAlertTableModel, OwnerTableModel>((pa, o) => pa.OwnerTableModelId == o.Id)
                                        .LeftJoin<PetAlertTableModel, PetImageTableModel>((pa, pi) => pa.PetId == pi.PetTableModelId && pi.IsProfileImage)
                                        .Where(pa => pa.AlertType == (int)AlertTypeEnum.Lost && 
                                                     pa.AlertStatus == (int)AlertStatusEnum.Closed && 
                                                     pa.MakeItPublic && 
                                                     pa.Approved == (int)ApproveStatusEnum.Pending)
                                         .OrderBy(pa => pa.SolvedOn);

                var queryResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>(query)
                                                     .ConfigureAwait(false);

                if (queryResults.Any())
                    results = FormatQueryResults(queryResults);
            }

            return results;
        }

        private List<AdminFoundAlert> FormatQueryResults(List<Tuple<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>> queryResults)
        {
            var foundAlerts = new List<AdminFoundAlert>();

            AdminFoundAlert adminFoundAlert;
            foreach (var item in queryResults)
            {
                adminFoundAlert = new AdminFoundAlert
                {
                    Id = item.Item1.Id,
                    Code = item.Item1.Code,
                    OwnerFullName = $"{item.Item3.FirstName} {item.Item3.LastName}",
                    OwnerProfileImageUrl = item.Item3.ProfileImageUrl,
                    PetName = item.Item2.Name,
                    PetProfileImageUrl = item.Item4.ImageUrl,
                    FoundComment = item.Item1.CommentFound,
                    FoundDateTime = item.Item1.SolvedOn.Value
                };

                foundAlerts.Add(adminFoundAlert);
            }

            return foundAlerts;
        }

        public async Task<int> ManageAdminFoundAlertsAsync(AdminManageFoundAlertRequest request)
        {
            int records;
            PetAlertTableModel petAlert = null;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction())
                {
                    if(request.Id.HasValue)
                        petAlert = await dbConnection.SingleByIdAsync<PetAlertTableModel>(request.Id.Value)
                                                     .ConfigureAwait(false);
                    else if(request.Code.HasValue)
                        petAlert = await dbConnection.SingleAsync<PetAlertTableModel>(p => p.Code == request.Code.Value)
                                                     .ConfigureAwait(false);

                    var action = request.Action;

                    records = await dbConnection.UpdateOnlyAsync(new PetAlertTableModel { Approved = action }, x => x.Approved, x => x.Id == petAlert.Id)
                                      .ConfigureAwait(false);

                    trans.Commit();
                }
            }
            
            return records;
        }

        public async Task<AdminDashboardDetails> GetAdminDashboardAsync()
        {
            var result = new AdminDashboardDetails();

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                var registeredAccountsQuery = dbConnection.From<OwnerTableModel>();
                var registeredAccountsCounter = await dbConnection.SqlScalarAsync<int>(registeredAccountsQuery.ToCountStatement());

                var registeredPetsQuery = dbConnection.From<PetTableModel>();
                var registeredPetsCounter = await dbConnection.SqlScalarAsync<int>(registeredPetsQuery.ToCountStatement());

                var lostPetsQuery = dbConnection.From<PetAlertTableModel>()
                                                .Where(a => a.PetId.HasValue && 
                                                            a.PetId > 0 &&
                                                            a.AlertType == (int)AlertTypeEnum.Lost &&
                                                            a.AlertStatus == (int)AlertStatusEnum.Active);
                var lostPetsCounter = await dbConnection.SqlScalarAsync<int>(lostPetsQuery.ToCountStatement(), lostPetsQuery.Params);

                var foundPetsQuery = dbConnection.From<PetAlertTableModel>()
                                               .Where(a => a.PetId.HasValue &&
                                                           a.PetId > 0 &&
                                                           a.AlertType == (int)AlertTypeEnum.Lost &&
                                                           a.AlertStatus == (int)AlertStatusEnum.Closed);
                var foundPetsCounter = await dbConnection.SqlScalarAsync<int>(foundPetsQuery.ToCountStatement(), foundPetsQuery.Params);

                var commentsToApproveQuery = dbConnection.From<PetAlertTableModel>()
                                                         .Where(a => a.PetId.HasValue && //micky ??
                                                                     a.PetId > 0 && // micky ??
                                                                     a.AlertType == (int)AlertTypeEnum.Lost &&
                                                                     a.AlertStatus == (int)AlertStatusEnum.Closed &&
                                                                     a.MakeItPublic == true &&
                                                                     a.Approved == (int)ApproveStatusEnum.Pending);
                var commentsToApproveCounter = await dbConnection.SqlScalarAsync<int>(commentsToApproveQuery.ToCountStatement(), commentsToApproveQuery.Params);

                var successStoriesQuery = dbConnection.From<PetAlertTableModel>()
                                                      .Where(a => a.PetId.HasValue && //micky ??
                                                                  a.PetId > 0 && // micky ??
                                                                  a.AlertType == (int)AlertTypeEnum.Lost &&
                                                                  a.AlertStatus == (int)AlertStatusEnum.Closed &&
                                                                  a.MakeItPublic == true &&
                                                                  a.Approved == (int)ApproveStatusEnum.Approved);
                var successStoriesCounter = await dbConnection.SqlScalarAsync<int>(successStoriesQuery.ToCountStatement(), successStoriesQuery.Params);

                result.RegisteredAccounts = registeredAccountsCounter;
                result.RegisteredPets = registeredPetsCounter;
                result.LostPets = lostPetsCounter;
                result.FoundPets = foundPetsCounter;
                result.CommentsToApprove = commentsToApproveCounter;
                result.SuccessStories = successStoriesCounter;
            }

            return result;
        }

        public async Task<List<AdminReportedAlert>> GetAdminReportedAlertsAsync()
        {
            var results = new List<AdminReportedAlert>();
            using (var dbConnection = _dbConnectionFactory.Open())
            {

                var query = dbConnection.From<PetAlertTableModel>()
                                        .LeftJoin<PetAlertTableModel, PetTableModel>((pa, p) => pa.PetId == p.Id)
                                        .LeftJoin<PetAlertTableModel, OwnerTableModel>((pa, o) => pa.OwnerTableModelId == o.Id)
                                        .LeftJoin<PetAlertTableModel, PetImageTableModel>((pa, pi) => pa.PetId == pi.PetTableModelId && pi.IsProfileImage)
                                        .Where(pa => pa.AlertStatus == (int)AlertStatusEnum.Reported)
                                        .OrderBy(pa => pa.CreatedOn);

                var queryResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>(query)
                                                     .ConfigureAwait(false);

                if (queryResults.Any())
                    results = FormatReportedQueryResults(queryResults);


                // --- PetAlertImages ---
                var imagesQuery = dbConnection.From<PetAlertTableModel>()
                                              .Join<PetAlertTableModel, PetAlertImageTableModel>((pa, pai) => pa.Id == pai.PetAlertTableModelId)
                                              .Where(pa => pa.AlertStatus == (int)AlertStatusEnum.Reported);

                var imagesResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetAlertImageTableModel>(imagesQuery)
                                                      .ConfigureAwait(false);

                if (imagesResults.Count > 0)
                {
                    var groupedImages = imagesResults.GroupBy(x => x.Item1.Code).ToList();

                    foreach (var groupedImage in groupedImages)
                    {
                        var foundAlertImage = results.Where(a => a.Code == groupedImage.Key);
                        if (foundAlertImage != null)
                            results.Find(r => r.Code == groupedImage.Key).PetProfileImageUrl = groupedImage.FirstOrDefault(x => x.Item2.Id > 0).Item2.ImageUrl;
                    }
                }
            }

            foreach (var item in results)
            {
                item.Images = await GetReportedAlertImages(item.Id).ConfigureAwait(false);
            }

            return results;
        }

        private async Task<List<string>> GetReportedAlertImages(int petAlertId)
        {
            var images = new List<string>();
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                var query = dbConnection.From<PetAlertTableModel>()
                                        .LeftJoin<PetAlertTableModel, PetImageTableModel>((pa, pi) => pa.PetId.HasValue && pa.PetId == pi.PetTableModelId && !pi.IsProfileImage)
                                        .LeftJoin<PetAlertTableModel, PetAlertImageTableModel>((pa, pai) => pa.Id == pai.PetAlertTableModelId)
                                        .Where(pa => pa.Id == petAlertId);

                var queryResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetImageTableModel, PetAlertImageTableModel>(query)
                                                     .ConfigureAwait(false);

                foreach (var item in queryResults)
                {
                    if (item.Item2.Id > 0)
                        images.Add(item.Item2.ImageUrl);

                    if (item.Item3.Id > 0)
                        images.Add(item.Item3.ImageUrl);
                }
            }

            return images;
        }

        private List<AdminReportedAlert> FormatReportedQueryResults(List<Tuple<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>> queryResults)
        {
            var reportedAlerts = new List<AdminReportedAlert>();

            AdminReportedAlert reportedAlert;
            foreach (var item in queryResults)
            {
                reportedAlert = new AdminReportedAlert
                {
                    Id = item.Item1.Id,
                    Code = item.Item1.Code,
                    Comment = item.Item1.Comment,
                    CreateOn = item.Item1.CreatedOn
                };

                if (item.Item2.Id != 0)
                {
                    reportedAlert.OwnerFullName = $"{item.Item3.FirstName} {item.Item3.LastName}";
                    reportedAlert.OwnerProfileImageUrl = item.Item3.ProfileImageUrl;
                    reportedAlert.PetName = item.Item2.Name;
                    reportedAlert.PetProfileImageUrl = item.Item4.ImageUrl;
                }
                else
                {
                    reportedAlert.OwnerFullName = "Anónimo";
                    reportedAlert.PetName = "Anónimo";
                }

                reportedAlerts.Add(reportedAlert);
            }

            return reportedAlerts;
        }

        public async Task<int> ManageReportedAlertsAsync(AdminManageReportedAlertRequest request)
        {
            int records;
            PetAlertTableModel petAlert = null;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction())
                {
                    if (request.Id.HasValue)
                        petAlert = await dbConnection.SingleByIdAsync<PetAlertTableModel>(request.Id.Value)
                                                     .ConfigureAwait(false);
                    else if (request.Code.HasValue)
                        petAlert = await dbConnection.SingleAsync<PetAlertTableModel>(p => p.Code == request.Code.Value)
                                                     .ConfigureAwait(false);

                    var action = request.Action;

                    records = await dbConnection.UpdateOnlyAsync(new PetAlertTableModel { AlertStatus = action }, x => x.AlertStatus, x => x.Id == petAlert.Id)
                                                .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return records;
        }
    }
}