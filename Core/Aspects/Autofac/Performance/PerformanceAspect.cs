using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private int _interval; //interval dediğimiz aspect'i yazdığımız metoda attribute'a bu interval'ı parametre olarak veririz 5 verirsek bu metod 5 sny'den fazla sürüyorsa beni uyar deriz. Bizde özellikle performans zaafiyetine  neden olan metodu kolaylıkla buluruz
        private Stopwatch _stopwatch;  //Stopwatch= timer görevi görür (kronometre)

        public PerformanceAspect(int interval)
        {
            _interval = interval;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
        }


        protected override void OnBefore(IInvocation invocation)  //Metodun önünde kronometreyi başlatıyoruız
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation) //Metod bittiğinde  geçen süre hesaplanır
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)   //Geçen süre interval'dan büyükse if içindeki kodu ypar. Bu if içine artık bunuy mail oalrak mı log alarak mı yönetiriz orası bize kalmış
            {
                Debug.WriteLine($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
            }
            _stopwatch.Reset();
        }
    }
}
