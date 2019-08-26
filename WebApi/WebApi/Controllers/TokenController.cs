using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TokenController : ControllerBase

    {
        [HttpPost("token")]
        public  ActionResult GetToken(string user, string password)
        {
            //security key
           
            //add claims
            if (user == "deneme" && password == "110")
            {
                string securityKey = "this_is_our_super_security_key_for_token_validation_project_2019_08_23$smesk.in";
                //symnetric security key
                var symenticSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
                //signing credentials
                var signingCredentials = new SigningCredentials(symenticSecurityKey, SecurityAlgorithms.HmacSha256Signature);
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                claims.Add(new Claim(ClaimTypes.Role, "Reader"));
                var token = new JwtSecurityToken(
                issuer: "smesk.in",
                audience: "readers",
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials,
                claims: claims
                );
                return Ok(new JwtSecurityTokenHandler().WriteToken(token) );
            }
            return BadRequest("Hatalı giriş");
        }
    }
}