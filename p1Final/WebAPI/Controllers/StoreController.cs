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
    public class StoreController : ControllerBase
    {
        private IBL _bl;

        public StoreController(IBL bl)
        {
            _bl = bl;
        }


        // GET: api/<StoreController>
        [HttpGet("GetAllStores")]
        public List<Store> Get()
        {
            return _bl.GetAllStores();
;
        }
        //Uses ID to find store

        // GET api/<StoreController>/id
        [HttpGet("GetStoreByID/{id}")]
        public ActionResult<Store> GetStoreById(int id)
        {
                Store store = _bl.GetStoreByID(id);
                if (store.Id == null)
                {
                    return NoContent();
                }
                return Ok(store);
        }

        //Allows addition of store

        // POST api/<StoreController>
        [HttpPost]
        public ActionResult Post([FromBody] Store storeToAdd)
        {   try
            {
                _bl.AddStore(storeToAdd);
                return Created("Successfully added store", storeToAdd);
            }
            catch (DuplicateRecordException ex)
            {
                return Conflict(ex.Message);
            } finally
            {
                Log.Information("New Store Was Created");
            }

        }

        [HttpGet("storeOrder/{storeId}")]
        public ActionResult<List<Order>> GetStoreOrders(int storeId, string select)
        {
            if (select == "date")
            {
                List<Order> allOrders = _bl.GetAllOrdersStoreDate(storeId);
                return Ok(allOrders);
            }

            else if (select == "price")
            {
                List<Order> allOrders = _bl.GetAllOrdersStorePrice(storeId);
                return Ok(allOrders);
            }
                return BadRequest();
            
        }

        // PUT api/<StoreController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StoreController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
