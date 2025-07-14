using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using BusinessObjects;
using BusinessObjects.Constants;
using BusinessObjects.DTOs.User.Request;
using BusinessObjects.DTOs.User.Response;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Repositories;
using Services.Utils;

namespace Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly JwtSecurityTokenHandler tokenHandler;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _config = config ?? throw new ArgumentNullException(nameof(IConfiguration));
            tokenHandler = new JwtSecurityTokenHandler();
        }

        private string GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> accessClaims = new()
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
                new Claim("id", user.Id),
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("role", user.Role)
            };

            var access = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: accessClaims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credential
                );

            var accessToken = tokenHandler.WriteToken(access);

            return accessToken;
        }

        public async Task<UserLoginResponseModel> Login(UserLoginRequestModel request)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email.Equals(request.Email));

            if (user == null) throw new CustomException("This email is not registered");

            bool isPasswordValid = PasswordHasher.VerifyPassword(request.Password, user.Password, user.Salt);
            if (!isPasswordValid) throw new CustomException("Password incorrect");

            return new UserLoginResponseModel()
            {
                Token = GenerateJWT(user)
            };
        }

        public async Task Register(UserRegisterRequestModel request)
        {
            if (await _unitOfWork.Users.IsExistAsync(u => u.Email.Equals(request.Email)))
                throw new CustomException("This email has been used");

            var newUser = _mapper.Map<User>(request);

            var (hashedPassword, salt) = PasswordHasher.HashNewPassword(request.Password);
            newUser.Password = hashedPassword;
            newUser.Salt = salt;

            newUser.CreatedAt = DateTime.Now;
            newUser.Role = UserRoles.User;

            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UserProfileResponseModel> GetUserProfile(string? userId)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId));
            if (user == null) throw new CustomException("User Id not exist");

            return _mapper.Map<UserProfileResponseModel>(user);
        }

        public async Task<ICollection<UserProfileResponseModel>> SearchUser(string search, List<string> exceptIds)
        {
            var users = await _unitOfWork.Users.GetAsync(u => 
                (u.Name.ToLower().Contains(search.ToLower()) || u.Email.ToLower().Contains(search.ToLower()))
                && !exceptIds.Contains(u.Id));

            return _mapper.Map<ICollection<UserProfileResponseModel>>(users);
        }

        public async Task AddMemberToBoard(string boardId, ICollection<string> emails)
        {
            var board = await _unitOfWork.Boards.SingleOrDefaultAsync(x => x.Id.Equals(boardId), "Members");

            if (board == null) throw new CustomException("Board Id not exist");

            var existEmails = board.Members.Select(x => x.Email);
            var membersToAdd = await _unitOfWork.Users.GetAsync(x => emails.Contains(x.Email) && !existEmails.Contains(x.Email));

            var notFoundEmails = emails.Except(membersToAdd.Select(m => m.Email));
            if (notFoundEmails.Any()) throw new CustomException("Email not found " + String.Join(", ", notFoundEmails), StatusCodes.Status404NotFound);

            foreach (var member in membersToAdd)
            {
                board.Members.Add(member);
            }

            _unitOfWork.Boards.Update(board);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveMemberFromBoard(string boardId, ICollection<string> emails)
        {
            var board = await _unitOfWork.Boards.SingleOrDefaultAsync(x => x.Id.Equals(boardId), "Members");

            if (board == null) throw new CustomException("Board Id not exist");

            var existEmails = board.Members.Select(x => x.Email);
            var membersToRemove = await _unitOfWork.Users.GetAsync(x => emails.Contains(x.Email) && existEmails.Contains(x.Email));

            var notFoundEmails = emails.Except(membersToRemove.Select(m => m.Email));
            if (notFoundEmails.Any()) throw new CustomException("Email not found " + String.Join(", ", notFoundEmails), StatusCodes.Status404NotFound);

            foreach (var member in membersToRemove)
            {
                board.Members.Remove(member);
            }

            _unitOfWork.Boards.Update(board);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangePassword(UserChangePasswordRequestModel request, string? userId)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId));

            if (user == null) throw new CustomException("User not found");

            bool isPasswordValid = PasswordHasher.VerifyPassword(request.OldPassword, user.Password, user.Salt);
            if (!isPasswordValid) throw new CustomException("Old Password incorrect");

            var (hashedPassword, newSalt) = PasswordHasher.HashNewPassword(request.NewPassword);
            user.Password = hashedPassword;
            user.Salt = newSalt;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ICollection<UserDetailResponseModel>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            return _mapper.Map<ICollection<UserDetailResponseModel>>(users);
        }
    }
}
