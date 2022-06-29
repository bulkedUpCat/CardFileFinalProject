using Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CardFileApi.JWT
{
    /// <summary>
    /// Interface to be implemented by class for generating Jwt tokens
    /// </summary>
    public interface IJwtHandler
    {
        SigningCredentials GetSigningCredentials();
        Task<List<Claim>> GetClaims(User user);
        JwtSecurityToken GenerateToken(SigningCredentials signingCredentials,
            List<Claim> claims);
    }
}
