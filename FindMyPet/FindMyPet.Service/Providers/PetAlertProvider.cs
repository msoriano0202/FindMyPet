using FindMyPet.DTO.PetAlert;
using FindMyPet.MyServiceStack.DataAccess;
using FindMyPet.MyServiceStack.Mappers;
using FindMyPet.TableModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MyServiceStack.Providers
{
    public interface IPetAlertProvider
    {
        Task<PetAlert> CreatePetAlertAsync(PetAlertCreateRequest request);
        Task<PetAlert> FoundPetAsync(PetAlertFoundRequest request);
    }

    public class PetAlertProvider : IPetAlertProvider
    {
        private readonly IPetAlertDataAccess _petAlertDataAccess;
        private readonly IPetAlertMapper _petAlertMapper;
        private readonly IPetDataAccess _petDataAccess;

        public PetAlertProvider(IPetAlertDataAccess petAlertDataAccess, 
                                IPetAlertMapper petAlertMapper,
                                IPetDataAccess petDataAccess)
        {
            if (petAlertDataAccess == null)
                throw new ArgumentNullException(nameof(petAlertDataAccess));

            if (petAlertMapper == null)
                throw new ArgumentNullException(nameof(petAlertMapper));

            if (petDataAccess == null)
                throw new ArgumentNullException(nameof(petDataAccess));

            _petAlertDataAccess = petAlertDataAccess;
            _petAlertMapper = petAlertMapper;
            _petDataAccess = petDataAccess;
        }

        public async Task<PetAlert> CreatePetAlertAsync(PetAlertCreateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
          
            var table = _petAlertMapper.MapCreateRequestToTable(request);

            int newId = 0;
            if (request.PetId.HasValue || request.PetCode.HasValue)
            {
                PetTableModel pet = null;
                if (request.PetId.HasValue)
                    pet = await _petDataAccess.GetPetByIdAsync(request.PetId.Value)
                                              .ConfigureAwait(false);
                else if (request.PetCode.HasValue)
                    pet = await _petDataAccess.GetPetByCodeAsync(request.PetCode.Value)
                                              .ConfigureAwait(false);

                table.PetId = pet.Id;

                newId = await _petAlertDataAccess.AddPetAlertAsync(table)
                                                 .ConfigureAwait(false);
            }
            else
            {
                //else if (request.PetCode.HasValue)
                //    newId = await _petAlertDataAccess.AddPetAlertAsync(request.PetCode.Value, table)
                //  
            }

            var newTable = await _petAlertDataAccess.GetPetAlertByIdAsync(newId)
                                                    .ConfigureAwait(false);

            return _petAlertMapper.MapPetAlertTableToPetAlert(newTable);
        }

        public async Task<PetAlert> FoundPetAsync(PetAlertFoundRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!request.PetId.HasValue && !request.PetCode.HasValue)
                throw new ArgumentException("Id and Code are NULL");

            PetTableModel pet = null;
            if (request.PetId.HasValue)
                pet = await _petDataAccess.GetPetByIdAsync(request.PetId.Value)
                                          .ConfigureAwait(false);
            else if (request.PetCode.HasValue)
                pet = await _petDataAccess.GetPetByCodeAsync(request.PetCode.Value)
                                          .ConfigureAwait(false);

            var petAlertId = await _petAlertDataAccess.FoundPet(pet.Id, request.Comment, request.MakeItPublic)
                                                      .ConfigureAwait(false);

            var petAlert = await _petAlertDataAccess.GetPetAlertByIdAsync(petAlertId)
                                                    .ConfigureAwait(false);

            return _petAlertMapper.MapPetAlertTableToPetAlert(petAlert);
        }
    }
}