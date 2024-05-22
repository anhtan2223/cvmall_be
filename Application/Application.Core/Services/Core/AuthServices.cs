using AutoMapper;
using Framework.Core.Extensions;
using Framework.Core.Helpers.Auth;
using Microsoft.Extensions.Options;
using Application.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Framework.Core.Helpers.Cache;
using Domain.Entities;
using Application.Common.Abstractions;
using Infrastructure.Contracts;
using Application.Core.Interfaces.Core;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Application.Core.Services.Core
{

    public class AuthServices : BaseService, IAuthServices
    {
        private readonly AuthSetting settings;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserRole> userRoleRepository;
        private readonly IRepository<Role> roleRepository;

        public AuthServices(IOptions<AuthSetting> _settings, IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            settings = _settings.Value;
            userRepository = _unitOfWork.GetRepository<User>();
            userRoleRepository = _unitOfWork.GetRepository<UserRole>();
            roleRepository = _unitOfWork.GetRepository<Role>();
        }

        public async Task<AuthLoginResponse> Login(AuthLoginRequest request)
        {
            // Check user login
            var user = userRepository
                        .GetQuery()
                        .ExcludeSoftDeleted()
                        .Where(x => x.user_name == request.user_name && x.is_actived == true)
                        .FirstOrDefault();

            // Response UnAuthorized
            if (user == null || !CryptographyProcessor.AreEqual(request.password, user.hashpass, user.salt))
                return null;

            // Generate token
            var data = new AuthLoginResponse()
            {
                access_token = JwtHelpers.GenerateToken(user, settings.JWTSecret),
                refresh_token = JwtHelpers.GenerateRefreshToken(user, settings.JWTSecret)
            };

            return data;
        }

        public async Task<AuthLoginResponse> Refresh(string refresh_token)
        {
            // Check refresh token
            var id = JwtHelpers.ValidateToken(refresh_token, settings.JWTSecret);

            if (id == null)
                return null;

            // Check user id
            var user = userRepository
                        .GetQuery()
                        .ExcludeSoftDeleted()
                        .FilterById((Guid)id)
                        .FirstOrDefault();

            // Response UnAuthorized
            if (user == null)
                return null;

            // Generate new token
            var data = new AuthLoginResponse()
            {
                access_token = JwtHelpers.GenerateToken(user, settings.JWTSecret),
                refresh_token = JwtHelpers.GenerateRefreshToken(user, settings.JWTSecret)
            };
            return data;
        }

        public Task<int> ChangePassword(Guid id, AuthChangePassRequest request)
        {
            var user = userRepository
                        .GetQuery()
                        .FindActiveById(id)
                        .FirstOrDefault();


            if (user == null || !CryptographyProcessor.AreEqual(request.current_password, user.hashpass, user.salt))
                return Task.FromResult(0);

            user.salt = CryptographyProcessor.CreateSalt(20);
            user.hashpass = CryptographyProcessor.GenerateHash(request.new_password, user.salt);
            userRepository.UpdateEntityAsync(user);

            return _unitOfWork.SaveChangesAsync();
        }

        public bool CheckUserAuthorized(Guid user_id, string path, string action)
        {
            var userRole = userRoleRepository.GetQuery().ExcludeSoftDeleted()
                            .Include(x => x.user)
                            .Where(x => x.user.id == user_id)
                            .Select(x => x.role_cd)
                            .Distinct()
                            .ToList();

            var permissions = roleRepository
                               .GetQuery()
                               .ExcludeSoftDeleted()
                               .Where(x => userRole.Contains(x.code))
                               .Include(x => x.permissions)
                               .SelectMany(x => x.permissions)
                               .ExcludeSoftDeleted()
                               .Include(x => x.function)
                               .Select(x => x.function)
                               .Where(x => !string.IsNullOrEmpty(x.method))
                               .Distinct()
                               .OrderBy(x => x.code)
                               .ToArray();

            var check = permissions.Where(x => x.path.ToLower() == path.ToLower() && x.method.ToUpper() == action).FirstOrDefault();
            return check != null ? true : false;
        }
    }
}
