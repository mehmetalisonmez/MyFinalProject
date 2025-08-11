using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
    public class SigningCredentialsHelper
    {
        //Sen JSON WEB TOKEN Sistemi yöneteceksin senin anahtarın budur şifreleme algoritmanda budur diyoruz burada.
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)  //Bizim için bu JasonWebToken servislerinin işte o WebAPI'de web apinin kullanılabileceği JWT'larının oluşturulabilmesi için 
        {
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature); //Anahtar olarak securityKey kullan şifreleme olarakda HMACSHA512'yi kullan
        }
    }

    //Credentials = mesela bizim lkullanıcı adı ve  parolamız user credential'dır.Bir sisteme girmek için elimizde olanlardır
}
