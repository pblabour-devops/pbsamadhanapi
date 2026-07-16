using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories.Implementations
{
    public interface IAuthRepository
    {
        Task<User> GetUserByUserName(LoginViewModel loginViewModel);
        Task<GenericResponseTemplateModel<Int64>> CreateNewUserProfile(UserProfile userProfile);
        Task<GenericResponseTemplateModel<Int64>> MapUserWithProfile(string userRefId, Int64 userProfileRefId);
    }
}
