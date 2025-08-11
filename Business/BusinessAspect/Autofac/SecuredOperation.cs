using Business.Abstract;
using Business.Constants;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;

namespace Business.BusinessAspect.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor; //Bir JWT isteği yapıyoruz ya aynı anda binlerce kişi istek yapabilir her bir kişi için her istek için bir httpcontext'i oluşur . Yani herkese bir tane thread oluşur

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(','); //Bir metni benim belirttiğim karaktere göre ayırıp array'e atar
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

             //MESELA WİNDOWS FORM'DA ÇALIŞIYOR İSEK (ServiceTool çalışma mantığını anlatmak adına örnek
  //          productService = ServiceTool.ServiceProvider.GetService<IProductService>();  //Bu gidip autofac'de bizim yaptığımız injection değerlerini alacak
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}
//ServiceTool bizim injection altyapımızı aynen okuyabilmemizi sağlayacak 

//Business katmanına Autofax.Extensions.DependencyInjection ve Autofax.Extensions..DynamixProxy paketlerişni yüklemeyi unutma
//Microsoft.Extensions.DependencyInjection bunuda unutmas