using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
    public interface IDapperRepository : IDisposable
    {
        Task<T> GetByIdAsync<T>(string command, object parms);
        Task<List<T>> Get<T>(string command, object parms, CommandType commandType);
        Task<int> AddupdateData(string command, object parmas, CommandType commandType);
        Task<SqlMapper.GridReader> GetMultipleResultSets<GridReader>(string command, object parms);
    }
}
