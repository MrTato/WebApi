using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class TokenController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Authenticate(Usuario usuario)
        {
            if (usuario == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            bool isCredentialValid = (usuario.Username == "admin" && usuario.Password == "123456");

            if(isCredentialValid)
            {
                var token = GenerateTokenJWT();
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }

        }

        private Token GenerateTokenJWT()
        {
            var now = DateTime.UtcNow;
            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                notBefore: now,
                expires: now.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials
                );

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);

            Token token = new Token();
            token.AccessToken = jwtTokenString;
            token.ExpiresIn = Convert.ToInt32(expireTime) * 60;

            return token;
        }
    }
}
