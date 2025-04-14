using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml;
using Visitors_Management.IRepository;
using Visitors_Management.Models;

namespace Visitors_Managements.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUser_Login _IUser_Login;
        private StringValues authorizationToken;
        public object? RzWebApi { get; private set; }

        public TokenController(IConfiguration config, IUser_Login IUser_Login)
        {
            _config = config;
            _IUser_Login = IUser_Login;
        }
        [HttpPost]
        //api/Token/CreateToken
        public IActionResult CreateToken([FromBody] VM_LoginModel objModel)
        {
            IActionResult response = Unauthorized();
            var User = Authenticate(objModel);
            if (User != null)
            {
                var TokenString = BuildToken();
                TokenString = GenerateJSONWebToken(objModel);
                objModel.TOKEN = TokenString;
                _IUser_Login.Insert_Token(objModel);
                response = Ok(new { token = TokenString });
            }
            return response;
        }
        private string BuildToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateJSONWebToken(VM_LoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userInfo.USERNAME),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.PASSWORD),
                new Claim("USERNAME", userInfo.USERNAME),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private VM_LoginModel? Authenticate(VM_LoginModel objModel)
        {
            var Result = _IUser_Login.Select_User(objModel);
            if (Result.Count == 0)
            {
                objModel = null;
            }
            else
            {
                objModel.USERNAME = Result[0].USERNAME;
                objModel.TOKEN = Result[0].TOKEN;
            }
            return objModel;
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public String Authorize(VM_LoginModel? objModel)
        {
            var claims = objModel.USERNAME;
            Request.Headers.TryGetValue("Authorization", out authorizationToken);
            objModel.TOKEN = authorizationToken;
            return "OK";
        }
        [HttpPost]
        public string ReturnXmlDocument(HttpRequestMessage request)
        {
            var doc = new XmlDocument();
            doc.Load(request.Content.ReadAsStreamAsync().Result);
            return doc.DocumentElement.OuterXml;
        }
    }
}