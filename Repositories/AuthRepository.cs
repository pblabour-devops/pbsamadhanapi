using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Repositories
{

    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenericResponseTemplateModel<long>> CreateNewUserProfile(UserProfile userProfile)
        {
            GenericResponseTemplateModel<Int64> genericServiceResultTemplate = new GenericResponseTemplateModel<Int64>() { ResponseDataModel = 0, ErrorDesc = "", HasError = false };
            try
            {
                await _context.AddAsync(userProfile);
                await _context.SaveChangesAsync();
                genericServiceResultTemplate.ResponseDataModel = userProfile.UserProfileId;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex.InnerException;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<long>> MapUserWithProfile(string userRefId, Int64 userProfileRefId)
        {
            GenericResponseTemplateModel<Int64> genericServiceResultTemplate = new GenericResponseTemplateModel<Int64>() { ResponseDataModel = 0, ErrorDesc = "", HasError = false };
            try
            {
                UserProfileMapping userProfileMapping = new UserProfileMapping()
                {
                    DateOfUserAssign = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    Createddate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    UserRefId = userRefId,
                    UserProfileRefId = userProfileRefId
                };
                await _context.AddAsync(userProfileMapping);
                await _context.SaveChangesAsync();
                genericServiceResultTemplate.ResponseDataModel = userProfileMapping.UserProfileMappingId;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<User> GetUserByUserName(LoginViewModel loginViewModel)
        {
            User user = await _context.Users.Where(x => x.UserName.ToLower() == loginViewModel.UserName.ToLower().Trim()).FirstOrDefaultAsync();
            if (user!=null)
            {
                return user;
            }
            return null;
        }
    }
}
