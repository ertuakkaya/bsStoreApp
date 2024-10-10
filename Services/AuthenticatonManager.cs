using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;

namespace Services
{
    internal class AuthenticatonManager : IAuthenticationService
    {


        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;


        private User? _user;

        public AuthenticatonManager(ILoggerService logger, IMapper mapper, IConfiguration configuration, UserManager<User> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var signinCredentials = GetSigninCredentials(); // kimlik bilgilerini al
            var claims = await GetClaims(); // talepleri al
            var tokenOptions = GenerateTokenOptions(signinCredentials, claims); // token seçeneklerini oluştur

            var refreshToken = GenerateRefreshToken(); // yenileme tokeni oluştur
            _user.RefreshToken = refreshToken; // kullanıcıya yenileme tokeni ata

            if (populateExp)
            {
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // tokenin geçerlilik süresini ayarla
            }

            await _userManager.UpdateAsync(_user); // kullanıcıyı güncelle

            var accessToken =  new JwtSecurityTokenHandler().WriteToken(tokenOptions); // token yaz

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

       


        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {

            var user = _mapper.Map<User>(userForRegistrationDto);

            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);
            }

            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto)
        {


            _user = await _userManager.FindByNameAsync(userForAuthenticationDto.UserName);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuthenticationDto.Password));

            if (!result)
            {
                _logger.LogWarning($"{nameof(ValidateUser)} : Authentication failed! Wrong username or password!");
            }

            return result;

        }


        private SigningCredentials GetSigninCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }


        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim> // username bilgisini 
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }




        /// <summary>
        /// Generates the token options for JWT authentication.
        /// </summary>
        /// <param name="signinCredentials">The signing credentials.</param>
        /// <param name="claims">The claims.</param>
        /// <returns>The generated token options.</returns>
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signinCredentials
            );
            return tokenOptions;
        }




        /// <summary>
        /// Generates a refresh token.
        /// </summary>
        /// <returns>The generated refresh token.</returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }



        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;


            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new RefreshTokenBadRequestExcepiton();
            }


            _user = user;
            return await CreateToken(false);
        }
    }
}
