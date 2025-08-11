using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60) //ctor'da default değer 60 süre vermezsek veri 60 dk boyunca cache'da durur sonra uçar bellkten
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();  //Hangi CacheManager ' ı kullancağımızı belirttik
        }

        public override void Intercept(IInvocation invocation)
        {
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}"); //ilk önce metodumuzun ismini bulmaya çalışıyoruz.ReflectedType demek namespace'ini ve çalıştığı interface i  demek.Yani bu satırda metodun namespace'ini, çalıştığı interface'i ve ismini yan yana getiriyoruz
            //Mesela ProductManager'da GetAll() metodunun Reflected type'ı = Business.Concrete.IroductService
            var arguments = invocation.Arguments.ToList();  //Metodun parametrelerini liste haline getiriyouz (Arguments = parmetreler )
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})"; //Parametre varsa methodName ' e ekleriz yoksa null döner ve key oluşur!!
            if (_cacheManager.IsAdd(key))  //cache'de var mı bu key' e sahip bir şey bak bakayım
            {
                invocation.ReturnValue = _cacheManager.Get(key);  //Varsa db'ye gitmeden cache'dan getir (invocation.ReturnValue demek kendimiz manuel bir return oluştururuz.)
                return;
            }
            invocation.Proceed();  //Cache'de yoksa metodu normal .çalıştır (verileri db'den getir)
            _cacheManager.Add(key, invocation.ReturnValue, _duration); //Sonrasında cache'e ekle!!
        }
    }
}
