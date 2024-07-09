using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Core.Utilities.Interceptors
{

    public class AspectInterceptorSelector : IInterceptorSelector
    {
        //reflection yapısı var class, metod özelliklerin erişme
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute> //Git class'ın attribute'larını oku ve onları bir listeye koy
                (true).ToList();
            var methodAttributes = type.GetMethod(method.Name)  //Git method'un attributelarını oku
                .GetCustomAttributes<MethodInterceptionBaseAttribute>(true); 
            classAttributes.AddRange(methodAttributes);
            //classAttributes.Add(new ExceptionLogAspect(typeof(FileLogger)));   //Bu kod şuan bizde log olmadığı için çalışmaz ama şu anlama gelir. Otomatik olarak sistemdeki bütün metodları log2a dahil et

            return classAttributes.OrderBy(x => x.Priority).ToArray();  //Onların çalışma sırasınıda priority'e göre sırala
        }
    }


}
