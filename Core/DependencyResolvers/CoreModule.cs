using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache(); // bu satır IMemoryCache _memoryCache ' in karşılığını verir. NET CORE KENDİSİ OTOMATİK INJECTİON YAPIYOR!! ((Arka planda hazır bir IMemoryCache injection'ı yapar
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();  //Mesela redis'e geçeceğimiz zaman bu satırda MemoryCacheManager yerine RedisCacheManager yazmam yeterli sistem Redis' e geçer!!
            serviceCollection.AddSingleton<Stopwatch>();
            
        }
    }
}
