using Microsoft.AspNetCore.Mvc;
using BL;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {

        private IBL _bl;


        public InventoryController (IBL bl)
        {
            _bl = bl;
        }


        // GET api/<InventoryController>/5
        [HttpGet("RestockLowInventory")]
        public List<Product> Get()
        {
            _bl.ReplenishInventory();
            return _bl.GetAllProducts();
        }



    }
}
