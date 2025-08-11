using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        //CRUD OPERASYONLARI = SİLME, GÜNCELLEME, EKLEME, YAZDIRMA
        IResult Add(Product product);
        IDataResult<Product> GetById(int produtId);
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int id);
        IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);
        IDataResult<List<ProductDetailDto>> getProductDetails();
        IResult AddTransactionalTest(Product product); // Transaction yönetimi uygulamalrda tutarlılığı korumak için yaptığımız yöntem
        //Örneğin benim ehsabım 100 tl var kerem in hesabına 10 tl aktarıcam bu nedemek benim hesabımda 10 tl eksilecek şekilde update edilmesi
        // ve Kerem'in ehsabına 10 tl artacak şekilde update elimesi yani 2 işlem var aynı süreçte 2 tane veritabanı işi var fakat
        //benim hesabımdan giderken güncelledi fakar kerem'in hesabına yazrken sistem hata verdi .İşlemi geri almak gerekiyor.Bu nasıl yapılır?
        //.NET ' de dispose pattern'de TransactionalScope denen sınıf ile yapılır
    }
}
