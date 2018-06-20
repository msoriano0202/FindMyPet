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
        Task<PetLostDetails> GetPetLostDetails(PetLostDetailsRequest request);
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

        private List<PetLost> FormatLostResults(List<Tuple<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>> lostResults)
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

        public async Task<PetLostDetails> GetPetLostDetails(PetLostDetailsRequest request)
        {
            var result = new PetLostDetails
            {
                PetInfo = new PetDetails(),
                OwnersInfo = new List<OwnerDetails>()
            };

            PetTableModel pet = null;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                if (request.PetId.HasValue)
                    pet = await dbConnection.SingleByIdAsync<PetTableModel>(request.PetId.Value)
                                            .ConfigureAwait(false);
                else
                    pet = await dbConnection.SingleAsync<PetTableModel>(p => p.Code == request.PetCode.Value)
                                            .ConfigureAwait(false);

                if (pet != null)
                {
                    var ownersQuery = dbConnection.From<OwnerPetTableModel>()
                                                  .Join<OwnerTableModel>((op, o) => op.OwnerTableModelId == o.Id)
                                                  .Join<OwnerSettingTableModel>((op, os) => op.OwnerTableModelId == os.OwnerTableModelId)
                                                  .Where(op => op.PetTableModelId == pet.Id);

                    var ownersResult = await dbConnection.SelectMultiAsync<OwnerPetTableModel, OwnerTableModel, OwnerSettingTableModel>(ownersQuery)
                                                         .ConfigureAwait(false);

                    var petQuery = dbConnection.From<PetTableModel>()
                                               .Join<PetTableModel, PetAlertTableModel>((p, pa) => p.Id == pa.PetId && pa.AlertType == (int)AlertTypeEnum.Lost && pa.AlertStatus == (int)AlertStatusEnum.Active)
                                               .Where(p => p.Id == pet.Id);

                    var petResult = await dbConnection.SelectMultiAsync<PetTableModel, PetAlertTableModel>(petQuery)
                                                      .ConfigureAwait(false);

                    var petImages = await dbConnection.SelectAsync<PetImageTableModel>(pi => pi.PetTableModelId == pet.Id)
                                                      .ConfigureAwait(false);

                    foreach (var item in petResult)
                    {
                        result.PetInfo.Name = item.Item1.Name;
                        result.PetInfo.ProfileImageUrl = petImages.Where(pi => pi.IsProfileImage).FirstOrDefault()?.ImageUrl;
                        result.PetInfo.DateOfBirth = item.Item1.DateOfBirth;
                        result.PetInfo.Description = item.Item1.Description;
                        result.PetInfo.LostComment = item.Item2.Comment;
                        result.PetInfo.LostDateTime = item.Item2.CreatedOn;
                        result.PetInfo.Images = petImages.Select(pi => pi.ImageUrl).ToList();
                    }

                    OwnerDetails ownerDetails;
                    foreach (var item in ownersResult)
                    {
                        ownerDetails = new OwnerDetails
                        {
                            FullName = $"{item.Item2.FirstName} {item.Item2.LastName}",
                            ProfileImageUrl = item.Item2.ProfileImageUrl,
                            Email = item.Item3.ShowEmailForAlerts ? item.Item2.Email : string.Empty,
                            PhoneNumber1 = item.Item3.ShowPhoneNumberForAlerts ? item.Item2.PhoneNumber1 : string.Empty,
                            PhoneNumber2 = item.Item3.ShowPhoneNumberForAlerts ? item.Item2.PhoneNumber2 : string.Empty,
                            Address1 = item.Item3.ShowAddressForAlerts ? item.Item2.Address1 : string.Empty,
                            Address2 = item.Item3.ShowAddressForAlerts ? item.Item2.Address2 : string.Empty
                        };

                        result.OwnersInfo.Add(ownerDetails);
                    }
                }
            }

            return result;
        }
    }
}