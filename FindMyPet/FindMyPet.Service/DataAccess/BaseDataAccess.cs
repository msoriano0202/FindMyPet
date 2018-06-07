using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FindMyPet.MyServiceStack.DataAccess
{
    public interface IBaseDataAccess<T>
    {
        Task<int> AddAsync(T data);
        Task<int> UpdateAsync(T data);
        Task<int> DeleteByIdAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetListAsync();
        Task<List<T>> GetListFilteredAsync(Expression<Func<T, bool>> predicate);
    }

    public class BaseDataAccess<T> : IBaseDataAccess<T>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public BaseDataAccess(IDbConnectionFactory dbConnectionFactory)
        {
            if (dbConnectionFactory == null)
                throw new ArgumentNullException(nameof(dbConnectionFactory));

            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> AddAsync(T data)
        {
            long newId;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                newId = await dbConnection.InsertAsync<T>(data, selectIdentity: true)
                                          .ConfigureAwait(false);
            }

            return (int)newId;
        }

        // Updates a row in the Person table. Note that this call updates
        // all fields, in order to update only certain fields using OrmLite,
        // use an anonymous type like the below line, which would only
        // update the FirstName and LastName fields:
        // _dbConnection.Update(new { FirstName = “Gene”, LastName = “Rayburn” });
        public async Task<int> UpdateAsync(T data)
        {
            var rowsAffected = 0;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                rowsAffected = await dbConnection.UpdateAsync<T>(data).ConfigureAwait(false);
            }
            return rowsAffected;
        }

        public async Task<int> DeleteByIdAsync(int id)
        {
            var rowsAffected = 0;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                rowsAffected = await dbConnection.DeleteByIdAsync<T>(id)
                                                 .ConfigureAwait(false);
            }

            return rowsAffected;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            T data;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                data = await dbConnection.SingleByIdAsync<T>(id)
                                         .ConfigureAwait(false);

                await dbConnection.LoadReferencesAsync(data);
            }
            return data;
        }

        public async Task<List<T>> GetListAsync()
        {
            List<T> list;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                list = await dbConnection.SelectAsync<T>()
                                         .ConfigureAwait(false);
            }

            return list;
        }
        
        public async Task<List<T>> GetListFilteredAsync(Expression<Func<T,bool>> predicate)
        {
            List<T> list;
            using (var dbConnection = _dbConnectionFactory.Open())
            {
                list = await dbConnection.SelectAsync<T>(predicate)
                                         .ConfigureAwait(false);
            }

            return list;
        }
    }
}