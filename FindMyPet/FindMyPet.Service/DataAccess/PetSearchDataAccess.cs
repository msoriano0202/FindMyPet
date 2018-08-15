using FindMyPet.DTO.Pet;
using FindMyPet.DTO.PetSearch;
using FindMyPet.DTO.Shared;
using FindMyPet.MyServiceStack.Shared;
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
        Task<PagedResponse<PetLostAlert>> GetPetLostAlertsAsync(PetLastAlertsRequest request);
        Task<PetLostDetails> GetPetLostDetails(PetLostDetailsRequest request);
        Task<PetAlertDetails> GetPetAlertDetailsAsync(PetAlertDetailsRequest request);
        Task<int> ManageReportedPetAlertAsync(PetAlertReportManageRequest request);
        Task<PagedResponse<PetSuccessStory>> GetPetSuccessStoriesAsync(PetSuccessStoryRequest request);
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
                var lostQuery = dbConnection.From<PetAlertTableModel>()
                                            .LeftJoin<PetAlertTableModel, PetTableModel>((pa, p) => pa.PetId.HasValue && pa.PetId.Value == p.Id)
                                            .LeftJoin<PetAlertTableModel, OwnerTableModel>((pa, o) => pa.OwnerTableModelId == o.Id)
                                            .LeftJoin<PetAlertTableModel, PetImageTableModel>((pa, pi) => pa.PetId == pi.PetTableModelId && pi.IsProfileImage)
                                            .Where(pa => pa.AlertStatus == (int)AlertStatusEnum.Active);

                if (request.From.HasValue && request.To.HasValue)
                    lostQuery = lostQuery.And(pa => pa.CreatedOn >= request.From.Value && pa.CreatedOn <= request.To.Value);

                var lostResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>(lostQuery)
                                                    .ConfigureAwait(false);

                // --- PetAlertImages ---
                var imagesQuery = dbConnection.From<PetAlertTableModel>()
                                              .Join<PetAlertTableModel, PetAlertImageTableModel>((pa, pai) => pa.Id == pai.PetAlertTableModelId)
                                              .Where(pa => pa.AlertStatus == (int)AlertStatusEnum.Active);

                if (request.From.HasValue && request.To.HasValue)
                    imagesQuery = imagesQuery.And(pa => pa.CreatedOn >= request.From.Value && pa.CreatedOn <= request.To.Value);

                var imagesResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetAlertImageTableModel>(imagesQuery)
                                                      .ConfigureAwait(false);

                if (lostResults.Any())
                    result = FormatLostResults(lostResults);

                if (imagesResults.Count > 0)
                {
                    var groupedImages = imagesResults.GroupBy(x => x.Item1.Code).ToList();

                    foreach (var groupedImage in groupedImages)
                    {
                        var foundAlertImage = result.Where(a => a.AlertCode == groupedImage.Key);
                        if (foundAlertImage != null)
                            result.Find(r => r.AlertCode == groupedImage.Key).PetProfileImageUrl = groupedImage.FirstOrDefault(x => x.Item2.Id > 0).Item2.ImageUrl;
                    }
                }
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
                    AlertCode = item.Item1.Code,
                    Type = item.Item1.AlertType,
                    Latitude = item.Item1.Latitude,
                    Longitude = item.Item1.Longitude,
                    LostDateTime = item.Item1.CreatedOn
                };

                if (item.Item2.Id != 0)
                {
                    petLost.PetId = item.Item2.Id;
                    petLost.PetCode = item.Item2.Code;
                    petLost.PetName = item.Item2.Name;
                    petLost.PetProfileImageUrl = item.Item4.ImageUrl;
                }
                else
                    petLost.PetName = GeneralHelper.GetAnonymousTitle(item.Item1.AlertType);

                lostPets.Add(petLost);
            }

            return lostPets;
        }

        public async Task<PagedResponse<PetLostAlert>> GetPetLostAlertsAsync(PetLastAlertsRequest request)
        {
            var response = new PagedResponse<PetLostAlert>();
            List<PetAlertTableModel> petAlerts;
            var records = new List<Tuple<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>>();
            var imagesResults = new List<Tuple<PetAlertTableModel, PetAlertImageTableModel>>();
            int totalRecords = 0;
            int totalPages = 0;

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                var q = dbConnection.From<PetAlertTableModel>()
                                    .Where(pa => pa.AlertStatus == (int)AlertStatusEnum.Active &&
                                                 (pa.CreatedOn >= request.From && pa.CreatedOn <= request.To))
                                    .OrderByDescending(pa => pa.CreatedOn);

                totalRecords = await dbConnection.SqlScalarAsync<int>(q.ToCountStatement(), q.Params);
                if (
                    (request.PageSize.HasValue && request.PageNumber.HasValue) &&
                    totalRecords > request.PageSize
                   )
                {
                    totalPages = (int)((totalRecords + (request.PageSize - 1)) / request.PageSize);

                    if (request.PageNumber <= 1)
                    {
                        request.PageNumber = 1;
                        q = q.Take(request.PageSize);
                    }
                    else
                    {
                        if (request.PageNumber > totalPages)
                            request.PageNumber = totalPages;

                        q = q.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
                    }

                    petAlerts = await dbConnection.SelectAsync<PetAlertTableModel>(q).ConfigureAwait(false);
                }
                else
                {
                    totalPages = 1;
                    petAlerts = await dbConnection.SelectAsync<PetAlertTableModel>(q).ConfigureAwait(false);
                }


                var petAlertsIds = petAlerts.Select(x => x.Id).ToList();

                var query = dbConnection.From<PetAlertTableModel>()
                                        .LeftJoin<PetAlertTableModel, PetTableModel>((pa, p) => pa.PetId == p.Id)
                                        .LeftJoin<PetAlertTableModel, OwnerTableModel>((pa, o) => pa.OwnerTableModelId == o.Id)
                                        .LeftJoin<PetAlertTableModel, PetImageTableModel>((pa, pi) => pa.PetId == pi.PetTableModelId && pi.IsProfileImage)
                                        .Where(pa => Sql.In(pa.Id, petAlertsIds))
                                        .OrderByDescending(pa => pa.CreatedOn);

                records = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>(query)
                                            .ConfigureAwait(false);

                // --- PetAlertImages ---
                var imagesQuery = dbConnection.From<PetAlertTableModel>()
                                              .Join<PetAlertTableModel, PetAlertImageTableModel>((pa, pai) => pa.Id == pai.PetAlertTableModelId)
                                              .Where(pa => Sql.In(pa.Id, petAlertsIds));

                imagesResults = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetAlertImageTableModel>(imagesQuery)
                                                  .ConfigureAwait(false);
            }

            response.TotalRecords = totalRecords;
            response.TotalPages = totalPages;
            response.Result = FormatAlertResults(records);

            if (imagesResults.Count > 0)
            {
                var groupedImages = imagesResults.GroupBy(x => x.Item1.Code).ToList();

                foreach (var groupedImage in groupedImages)
                {
                    var foundAlertImage = response.Result.Where(a => a.AlertCode == groupedImage.Key);
                    if (foundAlertImage != null)
                        response.Result.Find(r => r.AlertCode == groupedImage.Key).PetProfileImageUrl = groupedImage.FirstOrDefault(x => x.Item2.Id > 0).Item2.ImageUrl;
                }
            }

            return response;
        }

        private List<PetLostAlert> FormatAlertResults(List<Tuple<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>> lostResults)
        {
            var alerts = new List<PetLostAlert>();

            PetLostAlert alert;
            foreach (var item in lostResults)
            {
                alert = new PetLostAlert
                {
                    AlertCode = item.Item1.Code,
                    Type = item.Item1.AlertType,
                    Latitude = item.Item1.Latitude,
                    Longitude = item.Item1.Longitude,
                    LostDateTime = item.Item1.CreatedOn,
                    LostComment = item.Item1.Comment
                };

                if (item.Item2.Id != 0)
                {
                    alert.PetId = item.Item2.Id;
                    alert.PetCode = item.Item2.Code;
                    alert.PetName = item.Item2.Name;
                    alert.Description = item.Item2.Description;
                    alert.PetProfileImageUrl = item.Item4.ImageUrl;
                }
                else
                    alert.PetName = GeneralHelper.GetAnonymousTitle(item.Item1.AlertType);

                alerts.Add(alert);
            }

            return alerts;
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
                        result.PetInfo.PositionImageUrl = item.Item2.PositionImageUrl;
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

        public async Task<PetAlertDetails> GetPetAlertDetailsAsync(PetAlertDetailsRequest request)
        {
            PetAlertDetails petAlertDetails = null;
            PetAlertTableModel petAlertTable = null;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                petAlertTable = await dbConnection.SingleAsync<PetAlertTableModel>(pa => pa.Code == request.AlertCode && 
                                                                                         pa.AlertStatus == (int)AlertStatusEnum.Active)
                                                  .ConfigureAwait(false);

                if (petAlertTable != null)
                {
                    petAlertDetails = new PetAlertDetails();
                    petAlertDetails.PetInfo = new PetDetails();
                    petAlertDetails.OwnersInfo = new List<OwnerDetails>();
                    OwnerDetails ownerDetails;

                    // --- Pet Details ---
                    if (petAlertTable.PetId.HasValue && petAlertTable.PetId > 0)
                    {
                        var petQuery = dbConnection.From<PetTableModel>()
                                                   .Join<PetTableModel, PetAlertTableModel>((p, pa) => p.Id == pa.PetId &&
                                                                                                       pa.Code == petAlertTable.Code);

                        var petResult = await dbConnection.SelectMultiAsync<PetTableModel, PetAlertTableModel>(petQuery)
                                                          .ConfigureAwait(false);

                        var petImages = await dbConnection.SelectAsync<PetImageTableModel>(pi => pi.PetTableModelId == petAlertTable.PetId.Value)
                                                          .ConfigureAwait(false);

                        foreach (var item in petResult)
                        {
                            petAlertDetails.PetInfo.Name = item.Item1.Name;
                            petAlertDetails.PetInfo.ProfileImageUrl = petImages.Where(pi => pi.IsProfileImage).FirstOrDefault()?.ImageUrl;
                            petAlertDetails.PetInfo.DateOfBirth = item.Item1.DateOfBirth;
                            petAlertDetails.PetInfo.Description = item.Item1.Description;
                            petAlertDetails.PetInfo.LostComment = item.Item2.Comment;
                            petAlertDetails.PetInfo.LostDateTime = item.Item2.CreatedOn;
                            petAlertDetails.PetInfo.PositionImageUrl = item.Item2.PositionImageUrl;
                            petAlertDetails.PetInfo.Images = petImages.Select(pi => pi.ImageUrl).ToList();
                        }
                    }
                    else
                    {
                        var petImages = await dbConnection.SelectAsync<PetAlertImageTableModel>(pi => pi.PetAlertTableModelId == petAlertTable.Id)
                                                          .ConfigureAwait(false);

                        petAlertDetails.PetInfo.Name = GeneralHelper.GetAnonymousTitle(petAlertTable.AlertType);
                        petAlertDetails.PetInfo.LostComment = petAlertTable.Comment;
                        petAlertDetails.PetInfo.LostDateTime = petAlertTable.CreatedOn;
                        petAlertDetails.PetInfo.PositionImageUrl = petAlertTable.PositionImageUrl;
                        petAlertDetails.PetInfo.Images = petImages.Select(x => x.ImageUrl).ToList();
                    }

                    // --- Owner Details ---
                    if (petAlertTable.OwnerTableModelId.HasValue && petAlertTable.OwnerTableModelId > 0)
                    {
                        // --- Owner's Pet --
                        if (petAlertTable.PetId.HasValue && petAlertTable.PetId.Value > 0)
                        {
                            var ownersQuery = dbConnection.From<OwnerPetTableModel>()
                                                          .Join<OwnerPetTableModel, OwnerTableModel>((op, o) => op.OwnerTableModelId == o.Id)
                                                          .Join<OwnerPetTableModel, OwnerSettingTableModel>((op, os) => op.OwnerTableModelId == os.OwnerTableModelId)
                                                          .Where(op => op.PetTableModelId == petAlertTable.PetId.Value);

                            var ownersResult = await dbConnection.SelectMultiAsync<OwnerPetTableModel, OwnerTableModel, OwnerSettingTableModel>(ownersQuery)
                                                                 .ConfigureAwait(false);

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

                                petAlertDetails.OwnersInfo.Add(ownerDetails);
                            }
                        }
                        else // --- Other Pet ---
                        {
                            var ownersQuery = dbConnection.From<OwnerTableModel>()
                                                          .Join<OwnerTableModel, OwnerSettingTableModel>((o, os) => o.Id == os.OwnerTableModelId)
                                                          .Where(o => o.Id == petAlertTable.OwnerTableModelId);

                            var ownersResult = await dbConnection.SelectMultiAsync<OwnerTableModel, OwnerSettingTableModel>(ownersQuery)
                                                                 .ConfigureAwait(false);

                            foreach (var item in ownersResult)
                            {
                                ownerDetails = new OwnerDetails
                                {
                                    FullName = $"{item.Item1.FirstName} {item.Item1.LastName}",
                                    ProfileImageUrl = item.Item1.ProfileImageUrl,
                                    Email = item.Item2.ShowEmailForAlerts ? item.Item1.Email : string.Empty,
                                    PhoneNumber1 = item.Item2.ShowPhoneNumberForAlerts ? item.Item1.PhoneNumber1 : string.Empty,
                                    PhoneNumber2 = item.Item2.ShowPhoneNumberForAlerts ? item.Item1.PhoneNumber2 : string.Empty,
                                    Address1 = item.Item2.ShowAddressForAlerts ? item.Item1.Address1 : string.Empty,
                                    Address2 = item.Item2.ShowAddressForAlerts ? item.Item1.Address2 : string.Empty
                                };

                                petAlertDetails.OwnersInfo.Add(ownerDetails);
                            }
                        }
                    }                   
                }
            }

            return petAlertDetails;
        }

        public async Task<int> ManageReportedPetAlertAsync(PetAlertReportManageRequest request)
        {
            int records;
            
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                using (var trans = dbConnection.OpenTransaction())
                {
                    var petAlert = await dbConnection.SingleAsync<PetAlertTableModel>(p => p.Code == request.Code)
                                                 .ConfigureAwait(false);

                    var action = request.Action;

                    records = await dbConnection.UpdateOnlyAsync(new PetAlertTableModel { AlertStatus = action }, x => x.AlertStatus, x => x.Id == petAlert.Id)
                                      .ConfigureAwait(false);

                    trans.Commit();
                }
            }

            return records;
        }

        public async Task<PagedResponse<PetSuccessStory>> GetPetSuccessStoriesAsync(PetSuccessStoryRequest request)
        {
            var response = new PagedResponse<PetSuccessStory>();
            List<PetAlertTableModel> petAlerts;
            var records = new List<Tuple<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>>();
            int totalRecords = 0;
            int totalPages = 0;

            using (var dbConnection = _dbConnectionFactory.Open())
            {
                var q = dbConnection.From<PetAlertTableModel>()
                                    .Where(pa => pa.AlertType == (int)AlertTypeEnum.Lost &&
                                                 pa.AlertStatus == (int)AlertStatusEnum.Closed &&
                                                 pa.MakeItPublic &&
                                                 pa.Approved == (int)ApproveStatusEnum.Approved)
                                    .OrderByDescending(pa => pa.SolvedOn);

                totalRecords = await dbConnection.SqlScalarAsync<int>(q.ToCountStatement(), q.Params);
                if (
                    (request.PageSize.HasValue && request.PageNumber.HasValue) &&
                    totalRecords > request.PageSize
                   )
                {
                    totalPages = (int)((totalRecords + (request.PageSize - 1)) / request.PageSize);

                    if (request.PageNumber <= 1)
                    {
                        request.PageNumber = 1;
                        q = q.Take(request.PageSize);
                    }
                    else
                    {
                        if (request.PageNumber > totalPages)
                            request.PageNumber = totalPages;

                        q = q.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
                    }

                    petAlerts = await dbConnection.SelectAsync<PetAlertTableModel>(q).ConfigureAwait(false);
                }
                else
                {
                    totalPages = 1;
                    petAlerts = await dbConnection.SelectAsync<PetAlertTableModel>(q).ConfigureAwait(false);
                }


                var petAlertsIds = petAlerts.Select(x => x.Id).ToList();

                var query = dbConnection.From<PetAlertTableModel>()
                                        .Join<PetAlertTableModel, PetTableModel>((pa, p) => pa.PetId == p.Id)
                                        .Join<PetAlertTableModel, OwnerTableModel>((pa, o) => pa.OwnerTableModelId == o.Id)
                                        .LeftJoin<PetAlertTableModel, PetImageTableModel>((pa, pi) => pa.PetId == pi.PetTableModelId && pi.IsProfileImage)
                                        .Where(pa => Sql.In(pa.Id, petAlertsIds))
                                        .OrderByDescending(pa => pa.SolvedOn);

                records = await dbConnection.SelectMultiAsync<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>(query)
                                            .ConfigureAwait(false);
            }

            response.TotalRecords = totalRecords;
            response.TotalPages = totalPages;
            response.Result = FormatPetSuccessStories(records);

            return response;
        }

        private List<PetSuccessStory> FormatPetSuccessStories(List<Tuple<PetAlertTableModel, PetTableModel, OwnerTableModel, PetImageTableModel>> records)
        {
            var result = new List<PetSuccessStory>();

            PetSuccessStory petSuccessStory;
            foreach (var item in records)
            {
                petSuccessStory = new PetSuccessStory
                {
                    OwnerFullName = $"{item.Item3.FirstName} {item.Item3.LastName}",
                    OwnerProfileImageUrl = item.Item3.ProfileImageUrl,
                    PetName = item.Item2.Name,
                    PetProfileImageUrl = item.Item4.ImageUrl,
                    FoundComment = item.Item1.CommentFound,
                    LostDateTime = item.Item1.CreatedOn,
                    FoundDateTime = item.Item1.SolvedOn.Value
                };

                result.Add(petSuccessStory);
            }

            return result;
        }
    }
}