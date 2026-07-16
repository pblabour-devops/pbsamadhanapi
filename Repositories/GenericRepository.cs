using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private DbSet<T> dbEntity;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            dbEntity = _context.Set<T>();
        }

        public void Delete(Int64 id)
        {
            T model = dbEntity.Find(id);
            dbEntity.Remove(model);
        }
        public void DeleteRange(IEnumerable<T> model)
        {
            dbEntity.RemoveRange(model);
        }

        public T GetById(Int64 id)
        {
            return dbEntity.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return dbEntity.ToList();
        }

        public void Insert(T entity)
        {
            dbEntity.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
        }
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbEntity;
            foreach (Expression<Func<T, object>> include in includes)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> SavechangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void  Savechange()
        {
           _context.SaveChanges();
        }
    }
    public class Generic_SP_Repository : IGeneric_SP_Repository
    {
        private readonly AppDbContext _context;
        public Generic_SP_Repository(AppDbContext context)
        {
            _context = context;
        }

        public Task<GetLicenceNumberAndActTypeViewModel> CallStoreProcedureReaderAsyncList => throw new NotImplementedException();

        public async Task<List<T>> CallStoreProcedureReaderAsync<T>(string spName, List<StoreProcedureParm> storeProcedureParms) where T : class
        {
            string query = QueryBuilderStoreProcedureWithParameters(spName, storeProcedureParms);
            return await _context.Set<T>().FromSqlRaw(query).ToListAsync();
        }
        public async Task<List<T>> CallStoreProcedureReaderAsyncWithoutParams<T>(string spName) where T : class
        {
            StringBuilder query = new StringBuilder();
            query.Append("EXEC " + spName);
            return await _context.Set<T>().FromSqlRaw(query.ToString()).ToListAsync();
        }

        public async Task<IntReturn> CallStoreProcedureReaderScalerAsync(string spName, List<StoreProcedureParm> storeProcedureParms)
        {
            string query = QueryBuilderStoreProcedureWithParameters(spName, storeProcedureParms);
            var result = await _context.Set<IntReturn>().FromSqlRaw(query).ToListAsync();
            return result.FirstOrDefault();
        }

        private string QueryBuilderStoreProcedureWithParameters(string spName, List<StoreProcedureParm> storeProcedureParms)
        {
            StringBuilder query = new StringBuilder();
            query.Append("EXEC " + spName);
            if (storeProcedureParms.Count > 0)
            {
                query.Append(" ");
                int i = 1;
                foreach (var parm in storeProcedureParms)
                {
                    if (parm.isNumber)
                        query.Append("@" + parm.ParmName + "=" + parm.ParmValue);
                    else
                        query.Append("@" + parm.ParmName + "='" + parm.ParmValue + "'");
                    if (i != storeProcedureParms.Count)
                        query.Append(",");
                    i++;
                }
            }
            return query.ToString();
        }
    }
}
