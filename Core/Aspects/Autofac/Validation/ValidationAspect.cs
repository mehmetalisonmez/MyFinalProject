using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Validation
{  //Autofac'i kullanarak Aspect yazıcaz! O yüzden Autofac klasörü var ilerleyen zamanda başka paketle Aspect yazabiliriz geçiş kolay olsun diye!
    public class ValidationAspect : MethodInterception    //ValidationAspect bir attribute
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)  //Bu diyor ki bana validatorType'ı ver. Attribute'lara type'ı böyle atarız Type ile.ç
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType)) //Eğer gönderilen validatorType IValidator değilse o zaman hata fırlat
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil");
            }

            _validatorType = validatorType;  
        }
        protected override void OnBefore(IInvocation invocation)  //İşte OnBefore içini doldurdfuğumuz yer burası!!! (override ile)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);  //Göndürdüğümüz tipte bir instance üretiyor!  (Bu reflection'dır) Reflection çalışma anında bir şeyleri çalıştırabilmemizi mesela new'leme işlemini burada çalışma anında yapıyor.
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];   //Mesela gönderdiğimiz tip ProductValidator olsun. Bu satırda ProductValitor'ın çalışma tipini bul ve onun generic argümanlarından ilkini bul diyor. 
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);  //Sonra çalıştıracağımız metodun(Add)  parametrelerine bak. Ve bir üstte bulunan Generic tipe eşit olanları bul diyor 
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);  //Bulunan parametrelirin her birini tek tek gez ve ValidationTool'u kullanarak Validate et!!!!!!
            }
        }
    }
}
