using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess    
{
    //generic constraint (generic kısıt)
    //T:class : T referans tip olabilir. (T'yi kısıtladık. Şuan gidip değer tip ile oluşturamayız mesela)
    //T:class,IEntity : T referans tip olacak ancak ya IEntitiy ya da IEntity implemente etmiş olan bir nesne olabilir demek 
    //new() : new'lenebilir olmalı
    //T:class,IEntity,new()  :  Bu kısıtlama ile IEntity'yi parametre olarak veremeyiz. Sadece IEntity implemente etmiş nesneler T olabilir!
    public interface IEntityRepository <T> where T:class,IEntity,new() 
    {
        List<T> GetAll(Expression<Func<T,bool>> filter =null);  //LINQ ile filtreleme yapmak için kullanabileceğimiz yapı Expression'dır
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
      //  List<T> GetAllByCategory(int categoryId); //Ürünleri categoryId' ye göre filtreleme işlemini yapacak metod GetAll metoduna Expression verdiğimiz için bu koda gerek kalmadı istediğimiz filtreyi expression sayesinde uygularız

        //Core katmanı oluştursak; biz hem bu projemizde hem farklı projemizde(rentCar) gibi bu katmanı uygulasak işimizx çok rahatlar!
        //Core katmanında evrensel kodlarımızı yazabilir. Her projeye uygun kodlar yani
    }
}
