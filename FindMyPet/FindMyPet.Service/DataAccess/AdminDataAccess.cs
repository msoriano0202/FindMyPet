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
            List<AdminFoundAlert> results = null;
            using (var dbConnection = _dbConnectionFactory.Open())
            {

                var query = dbConnection.From<PetAlertTableModel>()
                                        .Join<PetAlertTableModel, PetTableModel>((pa,p) => pa.PetId == p.Id)
                                        .Join<PetAlertTableModel, OwnerTableModel>((pa, o) => pa.OwnerTableModelId == o.Id)
                                        .LeftJoin<PetAlertTableModel, PetImageTableModel>((pa, pi) => pa.PetId == pi.PetTableModelId && pi.IsProfileImage)
                                        .Where(pa => pa.AlertType == (int)AlertTypeEnum.Lost && 
                                                     pa.AlertStatus == (int)AlertStatusEnum.Deleted && 
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
    }
}