using FindMyPet.DTO.Shared;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FindMyPet.MyServiceStack.Shared
{
    public interface IPagingHelper<T>
    {
        Task<PagedResponse<T>> GetPagedRecords(SqlExpression<T> query, int? pageSize, int? pageNumber);
    }

    public class PagingHelper<T> : IPagingHelper<T>
    {
        public async Task<PagedResponse<T>> GetPagedRecords(SqlExpression<T> query, int? pageSize, int? pageNumber)
        {
            return null;
        }
    }
}