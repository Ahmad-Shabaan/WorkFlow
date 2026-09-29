using Application.Features.Authentication.DTOs;
using Application.Features.Comments.DTOs;
using Application.Features.Tasks.DTOs;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Enums;
using Infrastructure.Exceptions;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public IdentityService(RoleManager<IdentityRole<int>> roleManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, JwtService jwtService, RefreshTokenService refreshTokenService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var isValidEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (isValidEmail != null)
                throw new OperationFailedException($"This {isValidEmail} is already used");

            var user = _mapper.Map<ApplicationUser>(registerDto);
            if (string.IsNullOrEmpty(user.UserName))
                user.UserName = user.Email?.Split("@")[0];
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new OperationFailedException($"Failed to assign role: {errors}");
            }

            // give new users default role "employee"
            var roleResult = await _userManager.AddToRoleAsync(user, "Employee");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new OperationFailedException($"Failed to assign default role: {errors}");
            }

        }

        public async Task<LoginDto> Login(string email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException("User not found");
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new InvalidCredentialException("Invalid credentials");
            var (accessToken, expiryDate) = await _jwtService.GenerateAccessToken(user, _userManager);
            var (refreshToken, refreshExpiryDate) = await _refreshTokenService.GenerateRefreshToken(user, cancellationToken);
            return new LoginDto(accessToken, refreshToken, expiryDate, refreshExpiryDate);
        }

        public async Task<LoginDto> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            var result = await _refreshTokenService.ValidateRefreshToken(refreshToken, cancellationToken);
            var (accessToken, expiryDate) = await _jwtService.GenerateAccessToken(result.user, _userManager);
            return new LoginDto(accessToken, result.refreshToken, expiryDate, result.expiryDate);
        }


        public async Task Logout(string refreshToken, CancellationToken cancellationToken)
        {
            await _refreshTokenService.ValidateRefreshToken(refreshToken, cancellationToken);
            await _refreshTokenService.RevokeRefreshToken(refreshToken, cancellationToken);
        }


        public async Task<UserInfo?> GetUser(Guid userId)
        {
            var user = await RetriveUser(userId);
            if (user is null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = new UserInfo(user.Id, user.PublicId, user.FirstName + " " + user.LastName, user.Email!, user.Project, roles.AsReadOnly());
            return userInfo;
        }

        public async Task<int?> GetUserId(Guid userId)
        {
            var user = await RetriveUser(userId);
            if (user is null) return null;
            return user.Id;
        }
        public async Task<IEnumerable<string>> GetUserRoles(Guid userId)
        {
            var user = await RetriveUser(userId);
            if (user is null)
                return [];
            return await _userManager.GetRolesAsync(user);
        }


        public async Task<List<UserDto>> GetAllUsers(int adminId)
        => await _userManager.Users.Where(u => u.Id != adminId).Select(u => new UserDto(u.PublicId, u.FirstName + " " + u.LastName, u.Email!)).ToListAsync();

        public async Task<IEnumerable<EmployeeDto>> GetTargetUsers(List<int> ids)
        => await _userManager.Users.Where(u => ids.Contains(u.Id)).Select(u => new EmployeeDto(u.PublicId, u.FirstName + " " + u.LastName)).ToListAsync();

        public async Task<IEnumerable<AuthorDto>> GetTargetUsersName(List<int> ids)
        => await _userManager.Users.Where(u => ids.Contains(u.Id)).Select(u => new AuthorDto(u.Id, u.FirstName + " " + u.LastName)).ToListAsync();

        public async Task<EmployeeDto?> GetUserName(int id)
        => await _userManager.Users.Where(u => u.Id == id).Select(u => new EmployeeDto(u.PublicId, u.FirstName + " " + u.LastName)).FirstOrDefaultAsync();

        public async Task AssignRole(Guid userId, int roleId)
        {
            var user = await RetriveUser(userId) ?? throw new NotFoundException($"Can not find user with {userId}");
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role is null || string.IsNullOrWhiteSpace(role.Name))
                throw new NotFoundException($"Cannot find role with Id {roleId}");

            if (await _userManager.IsInRoleAsync(user, role.Name))
                return;

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new OperationFailedException($"Failed to assign role: {errors}");
            }
        }
        private async Task<ApplicationUser?> RetriveUser(Guid userId)
            => await _userManager.Users.FirstOrDefaultAsync(u => u.PublicId == userId);

    }
}
