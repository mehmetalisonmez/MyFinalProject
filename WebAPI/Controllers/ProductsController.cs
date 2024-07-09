using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  //Attribute = bir class ile ilgili bilgi verme(imzalama) yöntemidir. Burada bu class bir controllerdir bilgilendirmesi görevini yapar
    public class ProductsController : ControllerBase
    {
        //Loosely coupled (Gevşek bağlılık) : Bir bağlılık  var ama soyuta bağlılık var.
        //IoC Container (Inversion of Control) : Bellekte bir kutu gibi düşün. new ProductManager(), new EfProductDal() gibi bunların adreslerini barındırır ve mesela Constructor injection da bu türler lazım olduğunda ilk olarak IoC container'a bakılır ihtiyacı var ise oradan karşılanır
        IProductService _productService;

        public ProductsController(IProductService productService)  //Eğer AddSingleton(Program.cs'de) yazmasaydık hiç burası çalışmayacaktı API'de çünkü burada bağımlılık var.  
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()             //Postman WebAPI test aracıdır
        {
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);     
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int productId) 
        {
            var result = _productService.GetById(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("add")]
        public IActionResult Add(Product product) //PostRequest'lerde bir şey göndermem lazım. Data'yı postful'da body kıusmında json tipi olara kverbiliriz
        {
            var result = _productService.Add(product);
            if (result.Success)   
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
