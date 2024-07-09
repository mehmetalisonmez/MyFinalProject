using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    //Context: Db tabloları ile proje class'larını bağlamak
    public class NorthwindContext : DbContext
    {
        //İlk önce veritabanım nerede onu belirtmem gerekiyor!!
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // bu metod benim projem hangi veritabanı ile ilişkili onu belirteceğimiz yer!
        {
            //başına @ koyma sebebimiz şu normalde \ (ters slash) \\ oalrak yazılır C# da çünkü  \'ın başka anlamı vardır
            // biz @ ile \ yazabiliyoruz direkt. (Kaçırma operatörü gibi düşün)
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=master;Trusted_Connection=true"); //Sql servera bağlanıcaz.. 
            //Server=(localdb)\mssqlocaldb  ==  Burada adını belirttik ,,, Database=master == çekeceğimiz database'in adını yazdık.
            // Trusted_Connection=true   == Kullanıcı adı şifre olmadan erişebileceğimizi belirttik. Gerçek projelerde çok güçlü domain var ise bu şekilde giriş yapılır
        }

        public DbSet<Product>  Products { get; set; }  //Benim hangi class'ım veritabanındaki hangi tabloya denk geliyor onu belirtyioruz
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
