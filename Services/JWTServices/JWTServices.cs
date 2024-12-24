using Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.JWTServices
{
    public class JWTService //: IJWTService
    {
    //    private readonly IConfiguration _config;
    //    private readonly JwtSecurityTokenHandler _tokenHandler;
    //    private readonly IRefreshTokenRepository _refreshTokenRepository;
    //    public JWTService(IConfiguration config, IRefreshTokenRepository refreshTokenRepository)
    //    {
    //        _config = config ?? throw new ArgumentNullException(nameof(config));
    //        _tokenHandler = new JwtSecurityTokenHandler();
    //        _refreshTokenRepository = refreshTokenRepository;
    //    }
    //    public string decodeToken(string jwtToken, string nameClaim)
    //    {
    //        Claim? claim = _tokenHandler.ReadJwtToken(jwtToken).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));

    //        return claim != null ? claim.Value : "Error!!!";
    //    }

    //    public string GenerateJWT<T>(T entity) where T : class
    //    {
    //        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:JwtKey"]));
    //        var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //        List<Claim> claims = new List<Claim>();

    //        if (entity is Account customer)
    //        {
    //            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleEnums.Customer.ToString()));
    //            claims.Add(new Claim("userid", customer.CustomerId.ToString()));
    //            claims.Add(new Claim("email", customer.Email));
    //            claims.Add(new Claim("username", customer.Username));
    //        }
           
          
    //        else
    //        {
    //            throw new ArgumentException("Unsupported entity type");
    //        }

    //        var token = new JwtSecurityToken(
    //           issuer: _config["JwtSettings:Issuer"],
    //           audience: _config["JwtSettings:Audience"],
    //           claims: claims,
    //           expires: DateTime.Now.AddMonths(1),
    //           signingCredentials: credential
    //           );


    //        var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
    //        return encodetoken;
    //    }

    //    public string GenerateRefreshToken()
    //    {
    //        var newRefreshToken = Guid.NewGuid().ToString();
    //        return newRefreshToken;
    //    }
    }
}
