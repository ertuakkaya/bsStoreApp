using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthenticationService
    {

        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);

        Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto);

        // Token oluştur
        Task<TokenDto> CreateToken(bool populateExp);

        Task<TokenDto> RefreshToken(TokenDto tokenDto );


    }
}
