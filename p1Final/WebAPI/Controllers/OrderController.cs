using Microsoft.AspNetCore.Mvc;
using BL;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IBL _bl;

        public OrderController(IBL bl)
        {
            _bl = bl;
        }
        // GET: api/<OrderController>
        // Gets all products of a specified store
        [HttpGet("GetAllOrders")]
        public ActionResult<List<Order>> Get()
        {
            List<Order> allOrders = _bl.GetAllOrders();
            if (allOrders.Count == 0)
            {
                return NoContent();
            }
            return Ok(allOrders);
        }

        // GET api/<OrderController>/5
        [HttpGet("/GetOrderByOrderID/{id}")]
        public Order Get(int id)
        {
            return _bl.GetOrderByOrderID(id);
        }

        [HttpPost]
        public ActionResult Post(int productId, string storeName, int quantity, int userId)
        {
            List<Product> allProducts = _bl.GetAllProducts();
            Product product = allProducts.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                _bl.AddOrder(product.storeID, productId, storeName, product.ProductName, quantity, product.Price, userId, DateTime.Now);
                _bl.UpdateInventory(productId, product.Inventory - quantity);
                return Ok("Order successfully placed");
            }
            else
            {
                return BadRequest("There was an incorrect input");
            }
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
