using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ICrudService
    {
        public Task<CRUD_CreateUpdateOperationResponse> CreateUpdate(object requestData, User user);
        public Task<CRUD_CreateUpdateOperationResponse> OfficerCreateUpdate(object requestData, User user);
    }
}
