using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{

    public class EfEntityRepositoryBase<TEntity,TContext> : IEntityRepository<TEntity>
        where TEntity: class, IEntity, new() where TContext : DbContext, new()
    {
        //Entity Framework microsoft'un bir ürünüdür.
        //ORM (Object Relational Mapping) dediğimiz bir ürünüdür.LINQ destekli çalışır
        //Amaç : ORM demek veritabanındaki tabloyu sanki classmış gibi onunla ilişkilendirir.
        //ORM, Veritabanı nesneleri ile kodlar arasında bir ilişki kurup veritabanı işlemlerini yapmma süreci


        //NuGet 
        public void Add(TEntity entity)
        {
            using (TContext context = new TContext())  //C#'a özel using içine yadığımız nesneler using bitince anında garbage collector'a giderek beni bellekten at diyor!
            { //using içine yazarsak performans artar ama yazmasakta çalışır kod
                var addedEntity = context.Entry(entity); //Veri kaynağından benim görderdiğim Product'a bir tane nesneyi eşleştir.Ancak bu bir ekleme olacağı için herhangi bir şey ile eşleştirmeyecek onun yerine direkt ekleyecek. Çünkü yeni ekleme olduğu için veri kaynağında yok.
                addedEntity.State = EntityState.Added;  //State = durum demek, yani ben addedEntity ' nin durumunu bu satırda belirtiyorum Ekleyeyim mi? Sileyim mi? Güncelleyeyim mi? biz burada Ekledik! 
                context.SaveChanges();
                //Aslında 1. satır = Referansı yaklama 2.Satır= Onun eklenecek bir nesne olduğunu belirttik   3. Satır=Ve şimdi ekle, Tüm işlemleri gerçekleştirdi.

            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter )
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        //Lambda gibi bir yazı yazmak için Expression<Func<Product, bool>> filter bu parametre gerekli unutma!
        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
                //context.Set<Product>()  = DBSet'deki Product olana yerleş(Kısaca Products tablosu ile çalışacağını belirttik) 
                //context.Set<Product>().ToList()  = Veritabanındaki bütün tabloya eriş ve liste halinde geri döndür (select * from Products) arka planda bu kodu çalıştırırç
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
