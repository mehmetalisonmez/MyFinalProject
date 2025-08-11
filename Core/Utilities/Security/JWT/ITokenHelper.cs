using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationsClaims);
        //Bu fonksiyon Client'den gelen kullanıcı adı ve şifre Api'ye gelicek.Api' de eğer doğruysa bu operasyonumuz çalışır ve ilgili user için veritabanına gdder veritabanından
        // kullanıcının claimlerini buluşturacak orada JWT üretecek ve geri Client'a gönderecek
        
    }
}
