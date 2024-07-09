using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{//Bağımlılık Çözümleyiciler (Dependency Resolvers)
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance(); //Biri senden IProductService isterse ona ProductManager new'leyip ver. WebAPI program.cs içindeki AddSingleton() metodu işlevi ile aynı
            builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();                                                                               //

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();  //Çalışan uygulama içerisinde

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()   // İmplşemente edilmiş ınterface'leri bul
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector() //Onlara onmlar için AspectInterceptorSelector' çağır 
                }).SingleInstance();  //Kısaca autofac bizim bütün sınıflarımız için önce bunu çalıştırıyor. Git bak bakayım bu adamın aspect'i var mı?   
        }
    }
}
// autofac aynı zamanda Interceptor görevi veriyor bu yüzden kullanıyoz