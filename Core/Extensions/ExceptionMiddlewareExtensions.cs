using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>(); //Bizim kullanmak istediğimiz kendi yazdığımız middleware ExceptionMiddleware bunu belirtiyoruz
        }
        //middleware'ler UseHttpsRedirection(), UseCors() Api'de program.cs'deki bunlara middleware deniyor.
        //Biz kendi middleware'lerimizi yazmak istiyorsak. bu yolları izlemeliyiz
    }
}
