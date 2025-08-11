using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public class AccessToken
    {
        public string Token { get; set; }  //Kullanıcıya bir token vericez
        public DateTime Expiration { get; set; } //Token'ın ne kadar süreceğini vereceğiz
    }
}
