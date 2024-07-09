using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    // IResult'ın somut sınıfı
    // C# ' da this = class'ın kendisini  base = miras aldığı temel sınıfı kasteder
    public class Result : IResult
    {
        public Result(bool success,string message):this(success) // / : this(success) ifadesi, bu sınıfın tek parametreli constructor'ını success parametresi ile çağırır ve çalıştırır.
        {
            Message = message;  //Biz Message propertysinin set'i olmadığını söyledik ama burtada set ettik nasıl oldu? get 'Read Only' dir read only'ler constructor'da set edilebilirler!
        }
        
        public Result(bool success)
        {
            Success = success; 
        }

        public bool Success { get; }
        public string Message { get; }
    }
}
