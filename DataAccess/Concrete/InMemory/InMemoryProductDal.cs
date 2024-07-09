using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryProductDal : IProductDal
    {
        //Bellek üzerinde product'la ilgili veri erişim kodlarının yazılacağı yer (InMemoryProductDal)
        List<Product> _products;
        public InMemoryProductDal()
        {
            //Oracle,Sql gibi veritabanından geliyormuş gibi simüle ediyoruz
            _products = new List<Product>
            {
                new Product(){ProductId=1,CategoryId=1,ProductName = "Bardak",UnitPrice=15,UnitsInStock = 15},
                new Product(){ProductId=2,CategoryId=1,ProductName = "Kmera",UnitPrice=500,UnitsInStock = 3},
                new Product(){ProductId=3,CategoryId=2,ProductName = "Telefon",UnitPrice=1500,UnitsInStock = 2},
                new Product(){ProductId=4,CategoryId=2,ProductName = "Klavye",UnitPrice=150,UnitsInStock = 65},
                new Product(){ProductId=5,CategoryId=2,ProductName = "Fare",UnitPrice=85,UnitsInStock = 1}  
            };

        }
        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {  //Silmek için LINQ yapısı bilirsek işimiz çok rahat ancak ben LINQ bilmiyorsak nasıl yaparız onuda göstereceğim
                     //LINQ bilmiyor isek

            /* Product productToDelete = null;
               foreach (var p in _products)
                  {

                      if (product.ProductId == p.ProductId)
                      {
                          productToDelete = p;
                      }

                  }
                  _products.Remove(productToDelete);  
           */

                     //LINQ = (Language Integrated Query)  ile çalışırsak

            Product productToDelete = _products.SingleOrDefault(p => p.ProductId == product.ProductId);      //SingleOrDefault(p=>)  Bu kod aslında yukarıdaki foreaxh döngüsünü yapıyor verdiğimiz p 'de oradaki p ile aynı işlev!    
            _products.Remove(productToDelete);
             //SingleOrDefault(p => p.ProductId == product.ProductId); bu kod her p için git bak bakayım SingleOrDefault(p => p.ProductId == product.ProductId bu eşitlik sağlanıyor mu? Eşitlik sağlanıyor ise productToDelete'e eşitle!



       //     _products.Remove(product);  //Akla ilk gelen sadece bu bu kodla silmek ama hata alırız. Bunun nedeni referans tip Interface'den gelen product'ın adresi bizim product aresi ile aynı adresi tutmuyor ki!
        }

        public List<Product> GetAll()
        {
            return _products;
        }

        public void Update(Product product)
        {
            //GÖnderdiğim ürün id'sine sahip olan listedeki ürünü bul demektir aşağıdaki kod
            Product productToUpdate = _products.SingleOrDefault(p => p.ProductId == product.ProductId);
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.CategoryId = product.CategoryId;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitsInStock = product.UnitsInStock;

        }

        public List<Product> GetAllByCategory(int categoryId)
        {
            return _products.Where(p => p.CategoryId == categoryId).ToList();  //Where kullanımıda LINQ yapısıdır 

            //Where koşulu içindeki şarta uyan bütün elemanları yeni bir liste haline getirip onu döndürür.
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<ProductDetailDto> GetProductDetails()
        {
            throw new NotImplementedException();
        }
    }
}
