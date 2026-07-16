using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IBusinessEntityService
    {
        //Task<GenericFormModel<BusinessEntity>> GetBusinessEntityDetail(Int64 id);
        Task<GenericServiceResultTemplate> AddUpdate_BusinessEntityDetail(string requestData);
    }
}
