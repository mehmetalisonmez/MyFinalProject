using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    //Kural = Bir iş sınıfı başka sınıfları new'lemez. Injection uygulanır
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;


        //Bir entityManager kendisi hariç başka bir şeyi enjekte edemez!! Yani IProductDal enjekte ettik ama ICategoryDal enjekte edemeyiz
        //Ancak Service enjekte edebilir
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }


        //Claim : İddia etmek   product.add veya admin claimlerinden birine sahip olması gerekiyor
        [SecuredOperation("product.add,admin")]  //admin , product.add bunlara claim denir (yekisi olanlar)
        [ValidationAspect(typeof(ProductValidator))]     //Add metodunu doğrula ProductValidator kullanarak
        [CacheRemoveAspect("IProductService.Get")]  //IProductService' deki bütün Get'leri sil cache'den
        public IResult Add(Product product)
        {
            //validation : Eklemeye çalıştığımızda varlık/obje(mesela bu örnekte product) bu nesnenin yapısal uygun olup oılmadığını kontrol etmeye doğrulama denir. Mesela Productname 2 karakterden uzun olmalı gibisinden
            //Loglama = yapılan operasyonların bir yerde kaydını tutmak. Kim ne zaman, nerede, neyi ekledi gibi şeyleri tutar

            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName), CheckIfProductCountOfCategoryCorrect(product.CategoryId),CheckIfCategoryLimitExceded());
            //result ya kurala uymayan ya da null'dır
            if(result != null)
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }
        [CacheAspect]
        public IDataResult<Product> GetById(int produtId)
        {
            return new SuccessDataResult<Product> (_productDal.Get(p=>p.ProductId == produtId));
        }

        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {          
                //İş kodları varsa yazılır burada
            if(DateTime.Now.Hour == 1)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);   
            }

            return new SuccessDataResult<List<Product>> (_productDal.GetAll(),Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

       

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> getProductDetails()
        {
            if (DateTime.Now.Hour == 13)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<ProductDetailDto>> (_productDal.GetProductDetails());
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId) //İş kodu parçacıkları bu şekilde private yazılır
        {
            // Select count(*) from products where categoryId = 1      altta yazdığımız kod bunu çalıştırır. Yanialttaki kod bütün veriyi getiriyor sonra sayıyı buluyor gibi düşünme
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result >= 100)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult(); 
        }

        private IResult CheckIfProductNameExists(string productName) 
        {            
            var result = _productDal.GetAll(p => p.ProductName == productName).Count;     //Böylede yapabiliriz veya Any() metodu ile boolean sonuç döndürebilriiz!!
            if (result > 0)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);

            }
            return new SuccessResult();

        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if(result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);

            }
            return new SuccessResult();

        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            Add(product);
            if(product.UnitPrice<10)
            {
                throw new Exception("");
            }
            Add(product);
            return null;

        }
    }
}
//Ürünleri listeliyoruz diyelim ki ben öyle bir şey yazıcam ki ben GetAll metodunun üstüne bir aspect yazıcam [CacheAspect]  gibi
// o metodun belli bir süre cache'den gelmesini istiyorum. Yani daha önce herhangi bir kullanıcı bu metodu çağırdıysa. Herhangi bir kullanıcı binlerce
//kullanıcı olabilir. Herhangi bir kullanıcı o metodu çalıştırdıysa  ve o data değişmöediyse bir dah bir daha veritabanına gitmesine gerek yok 
//veya GetAllByCategoryId metodu çağırıldı diyelim. Kategoriye göre getirirken bir kategoriyi başkası çağırdıysa onu cache'lemek  ve ondan sonra
//diğer tüm isteklerde onun cache'den getirilmesini istiyorum.

//Cache olayını bir kaç yöntemle yapabiliriz. Biz Microsoft'un kendi içinde olan (.NET CORE'un) cache mekanizmasını kullanıcaz . 
//buna in memory cache deniyor. Kısacası yapılan istekler (cache'lenmek) istenen şeyler bellekte tutuluyor bu bellek bizim server'daki belleğimiz
//ondan sonra eğer cache'de o data varsa veritabanına gitmeksizin o datayı getiriyoruz.İlk olarak Cache için istediğimiz kadar süre verebiliriz. 
//İkinci olarak mesela bir ürün eklenmesi veya güncellenmesi, siilinmesi durumlarındada ben cache'in uçurulmasını istiyorum işte bu mimariyi kuracağız.


//Cache'lemek istediğimiz datayı key,value pair dediğimiz bir pair ile tutarız bellekte.
//key = cache'e verdiğmiiz isimdir. Örneğin GetAll metodu parametresiz. Bu metoda bir key vermek istediğimizde Business.Concrete.ProductManager.GetAll diyebilirz
//Parametreli olduğunda Business.Concrete.ProductManager.GetById(1)    //Parantez içinde parametre değerini



//Cache sistemlerinde biz microsoft'un hazır sistemini kullanıcaz dedik. redis  gibi daha gelişmiş cache sistemleride var DEVARCHİTECTURE ' da implementasyonuna bakabiliriz.
//Hatta RentCar projende dene uygulamayı!!