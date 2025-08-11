using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    //Middleware aslında sarmallamaya ytarıyor olurda apimiz'de bir hata olursa nasıl davranayım kodunu buraya yazıyor olacağız
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)  //Benim bütün kodlarımı try,catch'e alıyor
        {
            try //hata yoksa kod çalışır
            {
                await _next(httpContext);
            }
            catch (Exception e) //hata varsa handle edeceğiz demek
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e) //HATAYI KONTROLDEN GEÇİRİYORUZ
        {
            httpContext.Response.ContentType = "application/json";    //Tarayıcıya diyoruz ki ben sana bir tane json yolladım haberin olsun
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError; 

            string message = "Internal Server Error";
            IEnumerable<ValidationFailure> errors;
            if (e.GetType() == typeof(ValidationException)) //Aldığımız hata validationException ise bir mesaj oluşturup onu gösteriyoruz.
            {
                message = e.Message;
                errors = ((ValidationException)e).Errors;
                httpContext.Response.StatusCode = 400;

                return httpContext.Response.WriteAsync(new ValidationErrorDetails
                {
                    StatusCode = 400,
                    Message = message,
                    ValidationErrors = errors
                }.ToString());
            }

            return httpContext.Response.WriteAsync(new ErrorDetails    //Sistem hata verince bura döner
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message
            }.ToString()); 
        }
    }
}
