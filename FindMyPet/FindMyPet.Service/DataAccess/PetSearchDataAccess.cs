using FindMyPet.DTO.PetSearch;
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
    public interface IPetSearchDataAccess
    {
        Task<List<PetLost>> GetPetLostByDateAsync(PetSearchByDateRequest request);
    }

    public class PetSearchDataAccess : IPetSearchDataAccess
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PetSearchDataAccess(IDbConnectionFactory dbConnectionFactory)
        {
            if (dbConnectionFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionFactory));

            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<PetLost>> GetPetLostByDateAsync(PetSearchByDateRequest request)
        {
            var result = new List<PetLost>();

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                // pet lost query
                var lostQuery = dbConnection.From<PetAlertTableModel>()
                                            .Join<PetAlertTableModel, PetTableModel>((pa, p) => pa.PetId.HasValue && pa.PetId.Value == p.Id)
                                            .Join<PetAlertTableModel, OwnerTableModel>((pa, o) => pa.OwnerTableModelId == o.Id)
                                            .Join<PetAlertTableModel, PetImageTableModel>((pa, pi) => pa.PetId == pi.PetTableModelId && pi.IsProfileImage)
                                            .Where(pa => pa.AlertType == (int)AlertTypeEnum.Lost && pa.AlertStatus == (int)AlertStatusEnum.Active);

                var lostResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>(lostQuery)
                                                    .ConfigureAwait(false);

                if (lostResults.Any())
                    result = FormatLostResults(lostResults);
            }

            return result;
        }

        public List<PetLost> FormatLostResults(List<Tuple<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>> lostResults)
        {
            var lostPets = new List<PetLost>();

            PetLost petLost;
            foreach (var item in lostResults)
            {
                petLost = new PetLost
                {
                    PetId = item.Item2.Id,
                    PetCode = item.Item2.Code,
                    PetName = item.Item2.Name,
                    PetProfileImageUrl = item.Item4.ImageUrl,
                    //OwnerId = item.Item3.Id,
                    //OwnerCode = item.Item3.Code,
                    //OwnerName = $"{item.Item3.FirstName} {item.Item3.LastName}",
                    //OwnerProfileImageUrl = item.Item3.ProfileImageUrl,
                    Type = item.Item1.AlertType,
                    Latitude = item.Item1.Latitude,
                    Longitude = item.Item1.Longitude,
                    LostDateTime = item.Item1.CreatedOn
                };

                lostPets.Add(petLost);
            }

            return lostPets;
        }
    }
}