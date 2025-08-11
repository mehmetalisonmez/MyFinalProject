using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        //Microsoft'un kendi kütüphanesini kullancağız
        IMemoryCache _memoryCache;  //Bu bir interface dolayısıyla onu çözemmiz lazım contructor'dan ejekte edersek çalışmz çünkü zincir WebAPI => Business => DataAccess
                                    //şeklinde ilerliyor. Aspect bambaşka bir zincirin içinde dolayısıyla bağımlılık zincirinin içinde değil. Biz ServiceTool yazmıştık CoreModule ' ın içine yazacağız!!

        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }
        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key,value,TimeSpan.FromMinutes(duration));  //ne kadar duration verirsek o kadar cache'da duracak kodu veriyor burası
        }

        public T Get<T>(string key)  //Buraları ezzbere bilmemize gerek yok!! mantığı anla yeterli. Yapıya doğru koyduktan sonra hhayatımızda 1 kere kullanıcaz zaten
        {
            return _memoryCache.Get<T>(key);
        }
        public object Get(string key)  //Buraları ezzbere bilmemize gerek yok!! mantığı anla yeterli. Yapıya doğru koyduktan sonra hhayatımızda 1 kere kullanıcaz zaten
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);  //bir şey döndürmesini istemiyorsak C# ' daki teknik (out _)   Ben sadece bellekte böyle bir key var mı yok mu onu istiyorum data'yı istemiyorum diyoruz
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);  //Burası bizim mülakatta olaya hakim birisi varsa karşımıza çıkar. Bunların varyasyonlarını sorar mesela burada biz
                                       // Aslında buradaki şeyler  memoryCache'i yazan elemanlar her şeyi yazmış  ben niye böyle metodlar yapıp yapıp duruyorum
                                       //Sektördeki birsürü kod direkt metod yazmadan mesela memoryCache.Remove(key) şeklinde çağırırlar
                                       //Ama biz ne yapıyoruz gidip MemoryCacheManager diye bir şey oluşturup onun içine yazıyorum bizim derdimiz sadece
                                       //Microsoft'un memory cache'inin eklemek değil eğer MemoryCacheManager yapmazsak yarın öbür gün başka bir cache yönetiminde patlarız
                                       //Biz ne yapıyoruz .NET CORE'dan gelen cache kodlarını kendimize uyarlıyoruz. ICacheManager'da yazdığımız kodlar sayesinde 
                                       //Bu yaptığımıza Adapter Pattern deniyor . (Varolan bir sistemi kendi sistemimize uyarlama)
        }

        public void RemoveByPattern(string pattern)  //parametre olarak verdiğimiz pattern'e göre silme işlemi yapacak! (Ezbere bilmeye gerek yok mantık bil yeterli)
        {  //Bu metod  çalışma anında bellektren silmeye yarar. Yani elimizde bir sınıfın instance'ı var bellek te ve ona çalışma naında müdahale etmek istiyoruz
          //bunu biz reflection ile ypaarız.
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance); //Git bellekteki EntriesCollection'ı bul 
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;  //Definition'ı _memoryCache olanları bul
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)   //her bir casche elemanını gez 
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null); 
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();  //Her bir cache elemanından bu kurala uyanları keysToRemove içine at

            foreach (var key in keysToRemove)  
            {
                _memoryCache.Remove(key);  //ismi uyanları tek tek cache'den atarız
            }
        }
    }
}

//MemCache'de 3. parti bir şeydir oda cache sistemidir!! redis gibi memory cache gibi

//Microsoft bellekte cache datalarını EntriesCollection diye bir yerde tutar!!