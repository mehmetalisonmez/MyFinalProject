using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ClaimExtensions
    {
        //Extensions sayesinde var olan bir class'a kendi metodlarımızı ekleriz! . Extension yazabilmek için metod ve class'ın static olması gerekir
        public static void AddEmail(this ICollection<Claim> claims, string email)   //ICollection<Claim>   biz böyle bir şey görürsek o şu demek : AddEmail metodu Claim içine eklenecek!!!
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)   //Bana gönderilen roles'ları listeye çevir her birine tek tek dolaş her rolü claime ekle
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }
}
