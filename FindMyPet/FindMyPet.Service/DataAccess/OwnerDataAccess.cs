using FindMyPet.TableModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IOwnerDataAccess
    {
        Task<int> AddOwnerAsync(OwnerTableModel ownerTable);
        Task<int> UpdateOwnerAsync(OwnerTableModel ownerTable);
        Task<OwnerTableModel> GetOwnerByIdAsync(int ownerId);
        Task<OwnerTableModel> GetOwnerByMembershipIdAsync(string membershipId);
        Task<List<OwnerTableModel>> SearchOwnersAsync(Expression<Func<OwnerTableModel, bool>> predicate);
    }

    public class OwnerDataAccess : IOwnerDataAccess
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IBaseDataAccess<OwnerTableModel> _ownerBaseDataAccess;

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
            ownerTable.Code = Guid.NewGuid();
            ownerTable.CreatedOn = DateTime.Now;

            return await _ownerBaseDataAccess.AddAsync(ownerTable);
        }

        public async Task<int> UpdateOwnerAsync(OwnerTableModel ownerTable)
        {
            return await _ownerBaseDataAccess.UpdateAsync(ownerTable);
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
            }

            return ownerTableModel;
        }

        public async Task<List<OwnerTableModel>> SearchOwnersAsync(Expression<Func<OwnerTableModel, bool>> predicate)
        {
            return await _ownerBaseDataAccess.GetListFilteredAsync(predicate);
        }
    }
}