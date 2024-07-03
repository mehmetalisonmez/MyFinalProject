using DataAccess.Abstract;
using Entities.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : IProductDal
    {
        //Entity Framework microsoft'un bir ürünüdür.
        //ORM (Object Relational Mapping) dediğimiz bir ürünüdür.LINQ destekli çalışır
        //Amaç : ORM demek veritabanındaki tabloyu sanki classmış gibi onunla ilişkilendirir.
        //ORM, Veritabanı ensneleri ile kodlar arasında bir ilişki kurup veritabanı işlemlerini yapmma süreci
        

        //NuGet 
        public void Add(Product entity)
        {
            using (NorthwindContext context =new NorthwindContext())  //C#'a özel using içine yadığımız nesneler using bitince anında garbage collector'a giderek beni bellekten at diyor!
            { //usingg içine yazarsak performans artar ama yazmasakta çalışır kod
                var addedEntity = context.Entry(entity); //Veri kaynağından benim görderdiğim Product'a bir tane nesneyi eşleştir.Ancak bu bir ekleme olacağı için herhangi bir şey ile eşleştirmeyecek onun yerine direkt ekleyecek. Çünkü yeni ekleme olduğu için veri kaynağında yok.
                addedEntity.State = EntityState.Added;  //State = durum demek, yani ben addedEntity ' nin durumunu bu satırda belirtiyorum Ekleyeyim mi? Sileyim mi? Güncelleyeyim mi? biz burada Ekledik! 
                context.SaveChanges(); 
                //Aslında 1. satır = Referansı yaklama 2.Satır= Onun eklenecek bir nesne olduğunu belirttik   3. Satır=Ve şimdi ekle, Tüm işlemleri gerçekleştirdi.

            }
        }

        public void Delete(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext()) 
            {
                var deletedEntity = context.Entry(entity); 
                deletedEntity.State = EntityState.Deleted; 
                context.SaveChanges();
            }
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return context.Set<Product>().SingleOrDefault(filter);
            }
        }

        //Lambda gibi bir yazı yazmak için Expression<Func<Product, bool>> filter bu parametre gerekli unutma!
        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                return filter == null ? context.Set<Product>().ToList() : context.Set<Product>().Where(filter).ToList();
                //context.Set<Product>()  = DBSet'deki Product olana yerleş(Kısaca Products tablosu ile çalışacağını belirttik) 
                //context.Set<Product>().ToList()  = Veritabanındaki bütün tabloya eriş ve liste halinde geri döndür (select * from Products) arka planda bu kodu çalıştırırç
            }
        }

        public void Update(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }

    }
}
