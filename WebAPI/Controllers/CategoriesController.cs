using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        //Loosely coupled (Gevşek bağlılık) : Bir bağlılık  var ama soyuta bağlılık var.
        //IoC Container (Inversion of Control) : Bellekte bir kutu gibi düşün. new ProductManager(), new EfProductDal() gibi bunların adreslerini barındırır ve mesela Constructor injection da bu türler lazım olduğunda ilk olarak IoC container'a bakılır ihtiyacı var ise oradan karşılanır
        ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)  //Eğer AddSingleton(Program.cs'de) yazmasaydık hiç burası çalışmayacaktı API'de çünkü burada bağımlılık var.  
        {
            _categoryService = categoryService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()             //Postman WebAPI test aracıdır
        {
            var result = _categoryService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int productId)
        {
            var result = _categoryService.GetById(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost("add")]
        public IActionResult Add(Category category) 
        {
            var result = _categoryService.Add(category);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
