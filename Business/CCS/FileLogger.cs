using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CCS
{
    public class FileLogger : ILogger   //Dosyaya loglama, veritabanına loglama, uzak sunucuya loglama yapılabilir o yüzden bunlarınn hepsi ILogger
    {
        public void Log()
        {
            Console.WriteLine("Dosyaya loglandı");
        }
    }
}
