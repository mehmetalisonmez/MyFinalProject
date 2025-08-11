using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager //Burada farklı yöntemler kullanabiliriz dedik. Benim hangi alternatifim olursa olsun ICacheManager'ı implemente edecek!
    {
        T Get<T> (string key);  //Ben sana key vereyim sen bellekten o key'e karşılık gelen datayı bana ver diyoruz.  T olma sebebi biz veritabanından bir product 'da çekebiliriz veya product listeside çekebiliriz.
        object Get(string key);  //üsttekinin generic'siz yapılışı genellikle üstteki kullanılır ama buda kullaniblir . Ancak cast etemmiz gerekcek
        void Add(string key,object value,int duration); //key, value demiştik bunu ProductManager'da açıkladık bizim value'mız her tür olabileceği için object. Duration cache'de ne kadar duracağı(dk,sny vb türde tutabiliriz biz belirleriz)

        //Şimdi diyelkim k iGetAllByCategoryId metodu çağırıldı biz bunu cache'den mi getirelim veritabnaından mı bunun kararını cache'de varsa cache'den yoksa veritabanından getiririz ve cache'a ekleriz
        //Üstteki mantıkla
        bool IsAdd(string key);  //Cache'de var mı kontrolü yaparız
        void Remove(string key); //Cache'dan uçurma metodu. ben sana bir key verreyim sen onu uçur
        void RemoveByPattern(string pattern); 
        //Mesela GettALlByCategoryId b parametrik hangi key'i vericez bir sürü olabilir dolayısıyla böyle bir durumda bir pattern yazarız mesela desem ki
        //ismi(key'i) get ile başlayanları uçur veya isminde(key'inde) category olanları uçur. Ben ona bir tane RegularExpression (pattern) versem başı sonu
        //önemli değil içinde get veya cetegory olanlar gibi. Bizi baya bi koruyacak yöntem olacak bu. Yani farklı farklı kullanım senaryolarını yapıyor olacağız!!!


    }
}
