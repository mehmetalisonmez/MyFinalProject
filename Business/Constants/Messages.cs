using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    //Constants = sabitler
    //MaintenanceTime = bakım zamanı
    public static class Messages //static olursa new'lememize gerek yok class ismi ile çağırabiliriz özelliklerini
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductsListed= "Ürünler listelendi";
    }
}
