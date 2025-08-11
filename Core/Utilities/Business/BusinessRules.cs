using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BusinessRules
    { 
        //İş kuralı motoru yazdık basitce!!
        public static IResult Run(params IResult[] logics)   //logic = kural
        {
            foreach (var logic in logics) // bütün kuralları gez
            {
                if (!logic.Success) //kurala uymayan varsa
                { 
                    return logic; //uymayan kuralı döndür
                }
            }
            return null;
        }

        //public static List<IResult> Run2(params IResult[] logics)    //Bu şekilde liste şeklinde tüm hatalarıda döndürebiliriz
        //{

        //    List<IResult > errorResults = new List<IResult>();
        //    foreach (var logic in logics)
        //    {
        //        if (!logic.Success)
        //        {
        //            errorResults.Add(logic);
                    
        //        }
        //    }
        //    return errorResults;
        //}

    }
}
