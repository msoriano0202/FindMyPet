using FindMyPet.DTO.Owner;
using FindMyPet.MyServiceStack.DataAccess;
using FindMyPet.MyServiceStack.Mappers;
using FindMyPet.TableModel;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FindMyPet.MyServiceStack.Providers
{
    public interface IOwnerProvider
    {
        Task<Owner> CreateOwnerAsync(CreateOwnerRequest request);
        Task<Owner> GetOwnerAsync(OwnerRequest request);
        Task<Owner> UpdateOwnerAsync(UpdateOwnerRequest request);
        Task<List<Owner>> SearchOwnersAsync(SearchOwnerRequest request);
    }

    public class OwnerProvider : IOwnerProvider
    {
        private readonly IOwnerDataAccess _ownerDataAccess;
        private readonly IOwnerMapper _ownerMapper;

        public OwnerProvider(IOwnerDataAccess ownerDataAccess, IOwnerMapper ownerMapper)
        {
            if (ownerDataAccess == null)
                throw new ArgumentNullException(nameof(ownerDataAccess));

            if (ownerMapper == null)
                throw new ArgumentNullException(nameof(ownerMapper));

            _ownerDataAccess = ownerDataAccess;
            _ownerMapper = ownerMapper;
        }

        public async Task<Owner> CreateOwnerAsync(CreateOwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var table = _ownerMapper.MapCreateRequestToTable(request);

            var newId = await _ownerDataAccess.AddOwnerAsync(table)
                                              .ConfigureAwait(false);

            var newTable = await _ownerDataAccess.GetOwnerByIdAsync(newId)
                                                 .ConfigureAwait(false);

            return _ownerMapper.MapOwnerTableToOwner(newTable);
        }

        public async Task<Owner> GetOwnerAsync(OwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var table = await _ownerDataAccess.GetOwnerByIdAsync(request.Id)
                                              .ConfigureAwait(false);

            return _ownerMapper.MapOwnerTableToOwner(table);
        }

        public async Task<Owner> UpdateOwnerAsync(UpdateOwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var table = await _ownerDataAccess.GetOwnerByIdAsync(request.Id)
                                              .ConfigureAwait(false);

            table = _ownerMapper.MapUpdateRequestToTable(request, table);
            await _ownerDataAccess.UpdateOwnerAsync(table);

            var updatedTable = await _ownerDataAccess.GetOwnerByIdAsync(request.Id)
                                                     .ConfigureAwait(false);

            return _ownerMapper.MapOwnerTableToOwner(updatedTable);
        }

        public async Task<List<Owner>> SearchOwnersAsync(SearchOwnerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            Expression<Func<OwnerTable, bool>> exp = (x) => x.Id > 0;

            if (!string.IsNullOrEmpty(request.MembershipId))
                exp = exp.And<OwnerTable>(x => x.MembershipId == request.MembershipId);

            if (!string.IsNullOrEmpty(request.FirstName))
                exp = exp.And<OwnerTable>(x => x.FirstName.Contains(request.FirstName));

            if (!string.IsNullOrEmpty(request.LastName))
                exp = exp.And<OwnerTable>(x => x.LastName.Contains(request.LastName));

            if (!string.IsNullOrEmpty(request.Email))
                exp = exp.And<OwnerTable>(x => x.Email == request.Email);

            var response = await _ownerDataAccess.SearchOwnersAsync(exp);

            return response.ConvertAll(owner => _ownerMapper.MapOwnerTableToOwner(owner));
        }
    }
}