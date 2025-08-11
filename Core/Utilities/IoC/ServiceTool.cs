using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.IoC
{
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        //Web API'de veya autofac'de oluşturduğumuz injectionlar varya o injectionları oluşturabilmemizi sağlıyor.
        public static IServiceCollection Create(IServiceCollection services) 
        {
            ServiceProvider = services.BuildServiceProvider();//.NET' in servislerini al ve build et
            return services;
        }
    }
}
