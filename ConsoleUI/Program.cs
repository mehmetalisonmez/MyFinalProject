using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System.Net.Http.Headers;

internal class Program
{
    //SOLID
    //O = OpenClosedPrinciple   => Yeni bir özellik ekliyorsan mevcutta hiçbir koduna dokunamazsın 
    private static void Main(string[] args)
    {
        //ProductManager constructor'ı ona hangi veri yöntemi ile çalışacağımızı söylememizi ister.
        //İşteInterface ile çalışmasaydık sonradan başka bir veri erişim yöntemi eklendiğinde sıkıntı yaşayacaktık !!
        //ProductManager productManager = new ProductManager(new InMemoryProductDal());

        //foreach (var product in productManager.GetAll())
        //{
        //    Console.WriteLine(product.ProductName);
        //}
        ProductManager productManager2 = new ProductManager(new EfProductDal());

        Console.WriteLine("Tüm ürünler");
        Console.WriteLine("-------------");
        foreach (var product in productManager2.GetAll()) //tüm ürünlerin ismini yazdırır
        {
            Console.WriteLine(product.ProductName);
        }


        Console.WriteLine("Kategorisi 2 olan ürünler");
        Console.WriteLine("-------------");
        foreach (var product2 in productManager2.GetAllByCategoryId(2))  //2 kategorisindeki ürünlerin ismini yazdırır.
        {
            Console.WriteLine(product2.ProductName);
        }


        Console.WriteLine("Unit Price'ları 50-100 arasında olan ürünler");
        Console.WriteLine("-------------");
        foreach (var product3 in productManager2.GetByUnitPrice(50,100))
        {
            Console.WriteLine(product3.ProductName);
        }
    }
}