using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Core.Utilities.Security.Encryption
{
    public class SecurityKeyHelper
    {
        public static SecurityKey CreateSecurityKey(string securityKey)  //appsettings.jos dosyasındaki yazdığımız SecurityKey'den bahsediyoruz
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));  //Buraları araştırabilirim
            //Bizim securityKey'imiz bir byte haline ve simetrik Security Key' e dönüştürüyor
        }
    }
}
