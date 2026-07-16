using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
   public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(Int64 id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(Int64 id);
        void DeleteRange(IEnumerable<T> model);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes);
        Task<int> SavechangeAsync();
        void Savechange();
    }
    public interface IGeneric_SP_Repository
    {
        Task<GetLicenceNumberAndActTypeViewModel> CallStoreProcedureReaderAsyncList { get; }

        Task<List<T>> CallStoreProcedureReaderAsync<T>(string spName, List<StoreProcedureParm> storeProcedureParms) where T : class;
        Task<List<T>> CallStoreProcedureReaderAsyncWithoutParams<T>(string spName) where T : class;
        Task<IntReturn> CallStoreProcedureReaderScalerAsync(string spName, List<StoreProcedureParm> storeProcedureParms);
    }
    
    #region Interface Upload Documents

    #endregion Interface Upload Documents
}
