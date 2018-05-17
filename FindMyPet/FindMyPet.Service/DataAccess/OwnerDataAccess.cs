using FindMyPet.TableModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IOwnerDataAccess
    {
        Task<int> AddOwnerAsync(OwnerTable ownerTable);
        Task<int> UpdateOwnerAsync(OwnerTable ownerTable);
        Task<OwnerTable> GetOwnerByIdAsync(int ownerId);
        Task<List<OwnerTable>> SearchOwnersAsync(Expression<Func<OwnerTable, bool>> predicate);
    }

    public class OwnerDataAccess : IOwnerDataAccess
    {
        private readonly IBaseDataAccess<OwnerTable> _ownerBaseDataAccess;

        public OwnerDataAccess(IBaseDataAccess<OwnerTable> ownerBaseDataAccess)
        {
            if (ownerBaseDataAccess == null)
                throw new ArgumentNullException(nameof(ownerBaseDataAccess));

            _ownerBaseDataAccess = ownerBaseDataAccess;
        }

        public async Task<int> AddOwnerAsync(OwnerTable ownerTable)
        {
            ownerTable.Code = Guid.NewGuid();
            ownerTable.CreatedOn = DateTime.Now;

            return await _ownerBaseDataAccess.AddAsync(ownerTable);
        }

        public async Task<int> UpdateOwnerAsync(OwnerTable ownerTable)
        {
            return await _ownerBaseDataAccess.UpdateAsync(ownerTable);
        }

        public async Task<OwnerTable> GetOwnerByIdAsync(int ownerId)
        {
            return await _ownerBaseDataAccess.GetByIdAsync(ownerId);
        }

        public async Task<List<OwnerTable>> SearchOwnersAsync(Expression<Func<OwnerTable, bool>> predicate)
        {
            return await _ownerBaseDataAccess.GetListFilteredAsync(predicate);
        }
    }
}