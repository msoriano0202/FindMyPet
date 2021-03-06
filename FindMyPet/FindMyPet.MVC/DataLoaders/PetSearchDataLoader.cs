﻿using FindMyPet.DTO.PetSearch;
using FindMyPet.MVC.Mappers;
using FindMyPet.MVC.Models.PetSearch;
using FindMyPet.MVC.Models.Shared;
using FindMyPet.MVC.ServiceClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindMyPet.MVC.DataLoaders
{
    public interface IPetSearchDataLoader
    {
        List<PetLost> SearchLostPets(DateTime? from, DateTime? to);
        PetLostDetails GetPetLostDetails(Guid petCode);
        PetAlertDetails GetPetAlertDetails(Guid alertCode);
        int ManageReportedPetAlert(string code, int action);
        PagedResponseViewModel<PetSuccessStoryViewModel> GetPetSuccessStories(int pageSize, int pageNumber);
        PagedResponseViewModel<PetLastAlertDetailViewModel> GetPetLastAlerts(DateTime from, DateTime to, int pageSize, int pageNumber);
    }

    public class PetSearchDataLoader : IPetSearchDataLoader
    {
        private readonly IPetSearchServiceClient _petSearchServiceClient;
        private readonly IPetSearchMapper _petSearchMapper;

        public PetSearchDataLoader(IPetSearchServiceClient petSearchServiceClient, IPetSearchMapper petSearchMapper)
        {
            if (petSearchServiceClient == null)
                throw new ArgumentNullException(nameof(petSearchServiceClient));

            if (petSearchMapper == null)
                throw new ArgumentNullException(nameof(petSearchMapper));

            _petSearchServiceClient = petSearchServiceClient;
            _petSearchMapper = petSearchMapper;
        }

        public List<PetLost> SearchLostPets(DateTime? from, DateTime? to)
        {
            var request = new PetSearchByDateRequest
            {
                From = from, 
                To = to
            };

            return _petSearchServiceClient.SearchLostPets(request);
        }

        public PetLostDetails GetPetLostDetails(Guid petCode)
        {
            var request = new PetLostDetailsRequest { PetCode = petCode };

            return _petSearchServiceClient.GetPetLostDetails(request);
        }

        public PetAlertDetails GetPetAlertDetails(Guid alertCode)
        {
            var request = new PetAlertDetailsRequest { AlertCode = alertCode };

            return _petSearchServiceClient.GetPetAlertDetails(request);
        }

        public int ManageReportedPetAlert(string code, int action)
        {
            var request = new PetAlertReportManageRequest { Code = Guid.Parse(code), Action = action };

            return _petSearchServiceClient.ManageReportedPetAlert(request);
        }

        public PagedResponseViewModel<PetSuccessStoryViewModel> GetPetSuccessStories(int pageSize, int pageNumber)
        {
            var response = _petSearchServiceClient.GetPetSuccessStories(pageSize, pageNumber);

            return new PagedResponseViewModel<PetSuccessStoryViewModel>
            {
                Result = response.Result.ConvertAll(x => _petSearchMapper.PetSuccessStoryToViewModel(x)),
                TotalPages = response.TotalPages,
                TotalRecords = response.TotalRecords
            };
        }

        public PagedResponseViewModel<PetLastAlertDetailViewModel> GetPetLastAlerts(DateTime from, DateTime to, int pageSize, int pageNumber)
        {
            var request = new PetLastAlertsRequest
            {
                From = from, 
                To = to,
                PageSize = pageSize,
                PageNumber = pageNumber
            };

            var response = _petSearchServiceClient.GetPetLastAlerts(request);

            return new PagedResponseViewModel<PetLastAlertDetailViewModel>
            {
                Result = response.Result.ConvertAll(x => _petSearchMapper.PetLastAlertToViewModel(x)),
                TotalPages = response.TotalPages,
                TotalRecords = response.TotalRecords
            };
        }
    }
} 