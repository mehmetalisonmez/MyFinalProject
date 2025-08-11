using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    //IAuthService ve AuthManager' da ekleyelim
    //AuthManager'da kullanıcın sisteme log in olması token vermek, register olması ve token vermek gibi işlemleri içersin
    //Şimdi biz user log in edeceğiz ama password ' ü hashlenmiş bir şekilde tutuyoruz bunun için önce bir DTO yazalım
    //DTO adı = UserForLoginDTO  birde register içi UserForRegisterDto
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(string email);  //Kullanıcı var mı kontrol ederiz
        IDataResult<AccessToken> CreateAccessToken(User user);
    }
}
