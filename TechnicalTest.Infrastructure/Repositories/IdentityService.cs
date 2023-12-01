using AspNetCoreHero.Results;
using AspNetCoreHero.ThrowR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.Domain.Entities;
using TechnicalTest.Application.DTOs;
using TechnicalTest.Application.Interfaces.Repositories;
using TechnicalTest.Application.Enums;

namespace TechnicalTest.Infrastructure.Repositories
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly JWTSettings _jwtSettings;
        public IdentityService(
            UserManager<User> userManager, 
            IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            Throw.Exception.IfNull(user, nameof(user), $"No Accounts Registered with {request.Email}.");

            var password = await _userManager.CheckPasswordAsync(user, request.Password);
            Throw.Exception.IfFalse(password, $"Invalid Credentials for '{request.Email}'.");

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, ipAddress);
            var response = new TokenResponse
            {
                Id = user.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                IssuedOn = jwtSecurityToken.ValidFrom.ToLocalTime(),
                ExpiresOn = jwtSecurityToken.ValidTo.ToLocalTime(),
                Email = user.Email,
                UserName = user.UserName,
                IsVerified = user.EmailConfirmed
            };
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            return Result<TokenResponse>.Success(response, "Authenticated");
        }

        public async Task<Result<string>> RegisterAsync(RegisterRequest request, bool ValidateExists = true)
        {
            MailAddress address = new MailAddress(request.Email);
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                if (!ValidateExists)
                {
                    return Result<string>.Success(userWithSameEmail.Id, message: "User Registered.");
                }
                else
                {
                    throw new ApiException($"Email {request.Email} is already registered.");
                }           
            }
            else
            {
                var user = new User
                {
                    Email = request.Email,
                    UserName = address.User,
                    OrganizationId = request.OrganizationId,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());
                    return Result<string>.Success(user.Id, message: "User Registered.");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }          
        }

        private async Task<JwtSecurityToken> GenerateJWToken(User user, string ipAddress)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);
            return JWTGeneration(claims);
        }

        private JwtSecurityToken JWTGeneration(IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            var randomBytes = new byte[40];
            string refreshToken = string.Empty;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                refreshToken = Convert.ToBase64String(randomBytes);
            }
            return refreshToken.Replace("-", "");
        }
    }
}
