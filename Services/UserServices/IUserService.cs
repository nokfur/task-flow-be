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
        Task<ICollection<MemberResponse>> GetBoardMembers(string boardId);
        Task AddMemberToBoard(string boardId, ICollection<MemberAddRequest> request);
        Task RemoveMemberFromBoard(string boardId, string memberId);
        Task UpdateMemberRole(string boardId, string memberId, string role);
        Task ChangePassword(UserChangePasswordRequest request, string? userId);
        Task<ICollection<UserDetailResponse>> GetAllUsers();
    }
}
