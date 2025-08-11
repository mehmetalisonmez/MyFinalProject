using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        //Bir kişinin claimlerini ararken .NET bizi biraz uğraştırıyor. Bizim buradaki kodları yazmamız gerekiyor
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType) //ClaimsPrincipal : Bir kişinin claimlerine ulaşmak için o anki JWT ile gelen kişinin claimlerine erişmek için .NET'de olan class
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result; //Kişinin rollerini bulmam için yazıyoruz. (?) null olabileceğini işaret eder
        }

        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);  //Direk rolleri döndüren extension
        }
    }
}


//Bu kod, .NET Core uygulamalarında kullanıcıların claim'lerini (yetkilerini) kolayca alabilmek için iki extension metod içeriyor. Bu metodlar, JWT ile oturum açmış bir kullanıcının claim'lerine ve rollere erişmeyi sağlıyor.
//Detaylı Açıklama
//ClaimsPrincipal Nedir?

//ClaimsPrincipal, bir kullanıcının kimlik bilgilerini ve claim'lerini temsil eden bir sınıftır. JWT ile kimlik doğrulama yapıldığında, kullanıcının claim'leri ClaimsPrincipal üzerinden erişilebilir hale gelir.
//Extension Metodlar Nedir?

//Extension metodlar, mevcut sınıflara yeni metodlar eklemenin bir yoludur. Bu metodlar, var olan sınıfın kodunu değiştirmeden o sınıfa yeni özellikler eklemenizi sağlar. Bu durumda, ClaimsPrincipal sınıfına yeni metodlar ekliyoruz.


//1. Claims Metodu
//Bu metod, belirli bir claim türüne (claimType) sahip tüm claim'leri alır.
//   Açıklama:
//    ClaimsPrincipal claimsPrincipal: Bu, extension metodun hangi sınıfa uygulanacağını belirtir.
//    string claimType: İlgili claim türü. Örneğin, ClaimTypes.Role kullanılarak roller alınabilir.
//    claimsPrincipal?.FindAll(claimType): claimsPrincipal nesnesinde belirtilen türdeki tüm claim'leri bulur.
//    Select(x => x.Value): Bulunan claim'lerin değerlerini seçer.
//    ToList(): Seçilen değerleri listeye çevirir ve döndürür.

//2. ClaimRoles Metodu
//Bu metod, kullanıcının rollerini almayı sağlar.
//   Açıklama:
//    ClaimsPrincipal sınıfına ClaimRoles adında yeni bir extension metod eklenmiştir.
//    Bu metod, Claims metodunu kullanarak ClaimTypes.Role türündeki claim'leri alır ve döndürür.
//    ClaimTypes.Role, kullanıcı rolleri için önceden tanımlanmış bir claim türüdür.






//    Özet

//    Claims Metodu: Belirtilen claim türüne sahip tüm claim'leri alır.
//    ClaimRoles Metodu: Kullanıcının rollerini almayı sağlar.
//    Extension Metodlar: ClaimsPrincipal sınıfına yeni metodlar ekleyerek, JWT ile oturum açmış kullanıcıların claim'lerine ve rollerine kolayca erişim sağlar.

//Bu metodlar, .NET Core uygulamalarında kullanıcıların claim'lerini yönetmeyi ve kontrol etmeyi kolaylaştırır. Örneğin, bir kullanıcı belirli bir role sahip mi diye kontrol etmek istediğinizde bu metodları kullanabilirsiniz.