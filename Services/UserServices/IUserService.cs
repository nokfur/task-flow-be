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
        Task<UserLoginResponse> Login(UserLoginRequest request);
        Task Register(UserRegisterRequest request);
        Task<UserProfileResponse> GetUserProfile(string? userId);
        Task<ICollection<UserProfileResponse>> SearchUser(string search, List<string> exeptIds);
        Task AddMemberToBoard(string boardId, ICollection<string> emails);
        Task RemoveMemberFromBoard(string boardId, ICollection<string> emails);
        Task ChangePassword(UserChangePasswordRequest request, string? userId);
        Task<ICollection<UserDetailResponse>> GetAllUsers();
    }
}
