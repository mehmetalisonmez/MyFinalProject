using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System.Net.Http.Headers;

// Restful, mimari bizim geliştirdiğimiz .net projemizi tanımayan bir java uygulaması bir kodlin, ios uygulamasını bizim ortamımızla iletişim kurmasını sağlayan bir yapıdır
internal class Program
{
    //SOLID
    //O = OpenClosedPrinciple   => Yeni bir özellik ekliyorsan mevcutta hiçbir koduna dokunamazsın 
    //I = kullanmayacağın bir şeyi yazma
    private static void Main(string[] args)
    {
        //ProductManager constructor'ı ona hangi veri yöntemi ile çalışacağımızı söylememizi ister.
        //İşteInterface ile çalışmasaydık sonradan başka bir veri erişim yöntemi eklendiğinde sıkıntı yaşayacaktık !!
        //ProductManager productManager = new ProductManager(new InMemoryProductDal());

        //foreach (var product in productManager.GetAll())
        //{
        //    Console.WriteLine(product.ProductName);
        //}

        // ProductTest();

        // CategoryTest();

        // DtoTest();

        //DTO(Data Transformation Object) aslında bir e-ticaret sitesine girdiğimizde bi ürünün listesinde ilişkisel tablosundaki verileride görebiliyoruz

        ProductManager productManager = new ProductManager(new EfProductDal());

        var result = productManager.getProductDetails();

        if (result.Success==true)
        {
            foreach (var product in result.Data)
            {
                Console.WriteLine(product.ProductName + "/" + product.CategoryName);
            }
        }
        else
        {
            Console.WriteLine(result.Message);
        }

    }

    private static void DtoTest()
    {
        ProductManager productManager = new ProductManager(new EfProductDal());

        foreach (var product in productManager.getProductDetails().Data)
        {
            Console.WriteLine(product.ProductName + "/" + product.CategoryName);
        }
    }

    private static void CategoryTest()
    {
        CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());

        foreach (var category in categoryManager.GetAll())
        {
            Console.WriteLine(category.CategoryName);
        }
    }

    private static void ProductTest()
    {
        ProductManager productManager2 = new ProductManager(new EfProductDal());

        Console.WriteLine("Tüm ürünler");
        Console.WriteLine("-------------");
        foreach (var product in productManager2.GetAll().Data) //tüm ürünlerin ismini yazdırır
        {
            Console.WriteLine(product.ProductName);
        }


        Console.WriteLine("Kategorisi 2 olan ürünler");
        Console.WriteLine("-------------");
        foreach (var product2 in productManager2.GetAllByCategoryId(2).Data)  //2 kategorisindeki ürünlerin ismini yazdırır.
        {
            Console.WriteLine(product2.ProductName);
        }


        Console.WriteLine("Unit Price'ları 50-100 arasında olan ürünler");
        Console.WriteLine("-------------");
        foreach (var product3 in productManager2.GetByUnitPrice(50, 100).Data)
        {
            Console.WriteLine(product3.ProductName);
        }
    }
}

//Controller bizim sistemizi kullanıcak clientlar bu tarayıcı mobil uygulama web uygulaması olabilir. Bize hangi operasyonlar için ve nasıl istekde bulunabilirleri Controller'da yazarız!