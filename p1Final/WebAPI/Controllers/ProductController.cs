using Microsoft.AspNetCore.Mvc;
using Models;
using BL;
using CustomExceptions;
using Serilog;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IBL _bl;
        public ProductController(IBL bl)
        {
            _bl = bl;
        }
        // GET: api/<ProductController>
        // Gets all products of a specified store
        [HttpGet("GetAllProducts")]
        public List<Product> Get()
        {
            return _bl.GetAllProducts();
         
        }

        [HttpGet("GetAllProductsFromStore")]
        public ActionResult<List<Product>> Get(int storeID)
        {
            List<Product> allProducts = _bl.GetAllProducts(storeID);
            if (allProducts.Count == 0)
            {
                return NoContent();
            }
            return Ok(allProducts);
        }


        // POST api/<ProductController>
        [HttpPost("AddProduct")]
        public ActionResult Post([FromBody] Product productToAdd)
        {
            try
            {
                _bl.AddProduct(productToAdd);
                return Created("Successfully added product", productToAdd);
            }
            catch (DuplicateRecordException ex)
            {
                return Conflict(ex.Message);
            }
            finally
            {
                Log.Information("New Product was added");
            }
        }


        // DELETE api/<OrderController>/5
        /*[HttpDelete("{id}")]
        public void Delete(int id)
        {
            _bl.DeleteProduct(id);
        }
        */

    }
}
