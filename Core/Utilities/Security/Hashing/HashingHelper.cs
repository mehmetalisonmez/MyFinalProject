using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper 
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash,out byte[] passwordSalt)  //Bu verdiğimiz kod verdiğimiz şifrenin hash ve salt kodunu üretecek
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) //hmac System.Security.Cryptography.HMACSHA512 bir nesnesidir. Bu kodu ezbere bilmek zorunda değiliz ne ne yapıyor bil yeter gerisi dökümantasyon.bu satırda hangi algoritmayı kullanacağımızı söyleriz
            {
                passwordSalt = hmac.Key; //biz salt değerini  hmac.Key kullanıyoruz (Kullandığımız algoritmanın key değerini)  Başka bir şeyde kullanabiliriz tabiki standart olmalı çünkü şifreyi çözerken salta ihtiyacımız olacak //Her kullanıcı için başka bir key oluşturulur
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));  //Bir stringi byte'a çevirmek için Encoding.UTF8.GetBytes kullanılır. Bu satır 
            }
        }

        //Sonradan sisteme girmek isteyen kişinin verdiği passwordun bizim veri kaynağımızdaki hash ile ilgili salta göre eşleşip eşleşmediğini verdiğimiz fonksiyondur
        public static bool VerifyPasswordHash(string password, byte[] passwordHash,byte[] passwordSalt)
        { //Kullanıcın gönderdiği password'ü yine aynı algoritma  ile hashleseydin  karşımıza hash'ler eşit çıkar mı?. Paramaterdeki password kullanıcı giriş yapmaya çalışırken ki gönderdiği parola
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //doğrularkende aynı algoritmayı ama daha önce onu oluşttururken kullandığımız passwordSalt'u kullanarak doğruluyoruz 
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
           
        }
    }
}
