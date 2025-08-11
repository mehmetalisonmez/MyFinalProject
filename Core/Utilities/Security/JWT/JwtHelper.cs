using Core.Entities.Concrete;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Core.Extensions;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }  //Bizim API'mizdeki appsettings.json dosyasındaki değerleri okumamıza yarıyor okumamıza yarıyor Yani, Configuration gördüğümüzde  appsettings.json aklımıza gelsin
        private TokenOptions _tokenOptions; //IConfiguration ile okuduğumuz değerleri atacağım nesne //Burası WebAPI'deki appsettings.json' dosyasındaki TokenOptionsları okumamıza yarar 
        private DateTime _accessTokenExpiration;
        public JwtHelper(IConfiguration configuration) 
        {
            Configuration = configuration; 
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            //Section demek mesela appsettings.json ' daki TokenOptions ve değerleri veya Logging ve değerleri birer section
            //burada TokenOptions parametre olarak veriyortuz GetSection'a. bu sayede TokenOptions sectionundaki değerleri al ve onu TokenOptions sınıfının değerleri ile matchle
        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration); //DateTime Now'a appsettings.jos da belirttiğimiz AccesTokenExpiration'ı ekler!
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey); //SecurityKey'i de TokenOptions'daki SecurityKey ' i oluştur (Hatırla CreateSecurityKey bizim securityKey'i byte hale getiriyordu
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey); //Hangi algoritmayı kullanacaktı onu ayarlamıştık
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims); //Ve sonra ortaya bir JSON WEB TOKEN üretimi ortaya çıkacak.JWT oluşturmak için gerekli parametreleri verdik burada
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler(); 
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims) //Verilen parmetrelere göre  bir tane Jwt oluşturulur
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,   //Bizden istenilen ilgili bilgileri veririz
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            //.NET ' de var olan ensneye yeni metodlar ekleyebiliriz buna extension denir.
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }
    }
}
