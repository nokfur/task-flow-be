using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;

namespace Services.UserServices
{
    public interface IUserService
    {
        Task<UserLoginResponseModel> Login(UserLoginRequestModel request);
        Task Register(UserRegisterRequestModel request);
        Task<UserProfileResponseModel> GetUserProfile(string? userId);
        Task<ICollection<UserProfileResponseModel>> SearchUser(string search, List<string> exeptIds);
        Task AddMemberToBoard(string boardId, ICollection<string> emails);
        Task RemoveMemberFromBoard(string boardId, ICollection<string> emails);
        Task ChangePassword(UserChangePasswordRequestModel request, string? userId);
    }
}
