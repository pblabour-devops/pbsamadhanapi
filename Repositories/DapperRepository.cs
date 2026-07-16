using Dapper;
using Microsoft.Extensions.Configuration;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace pbsamadhannetcoreapi.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        //private readonly IConfiguration _config;
        //private string Connectionstring = "SQLServerConnection_SYS_O";

        //public DapperRepository(IConfiguration config)
        //{
        //    _config = config;
        //}
        //public void Dispose()
        //{

        //}

        //public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        //{
        //    throw new NotImplementedException();
        //}

        //public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        //{
        //    using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
        //    return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        //}

        //public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        //{
        //    using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
        //    return db.Query<T>(sp, parms, commandType: commandType).ToList();
        //}

        //public DbConnection GetDbconnection()
        //{
        //    return new SqlConnection(_config.GetConnectionString(Connectionstring));
        //}

        //public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        //{
        //    T result;
        //    using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
        //    try
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();

        //        using var tran = db.BeginTransaction();
        //        try
        //        {
        //            result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
        //            tran.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Rollback();
        //            throw ex;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (db.State == ConnectionState.Open)
        //            db.Close();
        //    }

        //    return result;
        //}

        //public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        //{
        //    T result;
        //    using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
        //    try
        //    {
        //        if (db.State == ConnectionState.Closed)
        //            db.Open();

        //        using var tran = db.BeginTransaction();
        //        try
        //        {
        //            result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
        //            tran.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Rollback();
        //            throw ex;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (db.State == ConnectionState.Open)
        //            db.Close();
        //    }

        //    return result;
        //}
        private readonly IDbConnection _db;

        public DapperRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("SQLServerConnection_SYS_O"));
        }
        public async Task<int> AddupdateData(string command, object parmas, CommandType commandType)
        {
            int result;
            result = await _db.ExecuteAsync(command, parmas,null,null, commandType);
            return result;
        }

        public async Task<List<T>> Get<T>(string command, object parms, CommandType commandType)
        {
            List<T> result = new List<T>();
            result= (await _db.QueryAsync<T>(command, parms,null,null, commandType)).ToList();
            return result;
        }

        public async Task<T> GetByIdAsync<T>(string command, object parms)
        {
            T result;
            result = (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();
            return result;
        }


        public async Task<SqlMapper.GridReader> GetMultipleResultSets<GridReader>(string command, object parms)
        {
            //List<T> result = new List<T>();
            SqlMapper.GridReader data=null;
            try
            {
                data = await _db.QueryMultipleAsync(command, parms, null,null,CommandType.StoredProcedure);
                //var clearence = data.ReadFirst<ProcessImd_ClearenceViewModel>();
                //var documemts = data.Read<ProcessImd_ClearencesDocumemtViewModel>();
                //var logs = data.Read<ProcessImd_ClearencesLogViewModel>();

                //var fees = data.Read<ProcessImd_ClearencesFeeViewModel>();
                ////var yyy = parms.GetType().GetProperty("ServiceCode");
                ////var kk= yyy.GetValue(null);

                //var applicantDetails = data.Read<ProcessImd_ClearencesApplicantDetailsViewModel>();

                //int ss = 6;

                //if (ss == 2) // Building Plan
                //{
                //    var buildingPlan = data.Read<ProcessImd_ClearencesBuildingPlanViewModel>();
                //}

                //else if (ss == 3) // Principal Employer
                //{
                //    var establishmentRegistration = data.Read<ProcessImd_ClearencesEstablishmentRegistrationViewModel>();
                //}

                //else if (ss == 4) // Contract Labour
                //{
                //    var contractLabour = data.Read<ProcessImd_ClearencesContractLabourViewModel>();
                //}

                //else if (ss == 5) // Shop
                //{
                //    var shopLicence = data.Read<ProcessImd_ClearencesShopLicenceViewModel>();
                //}
                //else if (ss == 6) // Factory
                //{
                //    var factoryLicence = data.Read<ProcessImd_ClearencesFactoryLicenceViewModel>();
                //}
                //else if(ss == 35) // BOCW 
                //{
                //    var bocwRegistration = data.Read<ProcessImd_ClearencesBocwRegistrationViewModel>();
                //}
                //else if (ss == 25) // Trade Union 
                //{
                //    var tradeUnion = data.Read<ProcessImd_ClearencesTradeUnionViewModel>();
                //}
                //else if(ss == 17)
                //{
                //    var interstatePrincipalEmployers = data.Read<ProcessImd_ClearencesInterstatePrincipalEmployersViewModel>();
                //}
                //else if(ss == 22)
                //{
                //    var motorTransport = data.Read<ProcessImd_ClearencesMotorTransportViewModel>();
                //}
            }
            catch (Exception ex)
            {

                var tt = ex.Message;
            }
            

            //result = (await _db.QueryAsync<T>(command, parms)).ToList();
            return data;
        }


        public void Dispose()
        {
            _db.Close();
            _db.Dispose();
        }
    }
}
