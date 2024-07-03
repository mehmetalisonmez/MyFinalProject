using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    //I = interface olduğunu    Product = hangi tabloya işaret ettiğini    Dal = hangi katmanda bulunduğunu (data access layer)
    public interface IProductDal : IEntityRepository<Product>
    {
        
    }
}
// Yani bu benim productla ilgili veritabanında yapacağım operasyonları içeren interface

//Adonet,entity framework etc. Data access katmanında kullanacağımızı farklı yollar bunlar gibi şeyler
//Bu gibi teknolojiler gibi alternatifli şeyler kullanıyorsak hemen orada klasörleme tekniğine gidiyoruz. (Con crete üzerinde yeni folder)