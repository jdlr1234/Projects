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
    public class CustomerController : ControllerBase
    {
        private IBL _bl;

        public CustomerController(IBL bl)
        {
            _bl = bl;
        }

        [HttpGet("GetAllCustomers")]

        public List<Customer> Get()
        {
            return _bl.GetAllCustomers();
        }

        // GET api/<CustomerController>/
        [HttpGet("Login")]
        public ActionResult Get(string username, string password)
        {
            Customer existing = _bl.Login(new Customer { UserName = username, Password = password });
            if (existing.Id <= 0)
            {
                return BadRequest("User does not exist");
            }
            else
            {
                if (existing.Password == password)
                {
                    Log.Information("User with username: " + existing.UserName + " logged in.");
                    return Ok("You've successfully logged in");
                }
                else
                {
                    return BadRequest("Incorrect password");
                }
            }
        }

        //used to create user

        // POST api/<CustomerController>
        [HttpPost("CreateAccount")]
        public ActionResult Post([FromBody] Customer customerToAdd)
        {
            try
            {
                _bl.AddCustomer(customerToAdd);
                return Created("Successfully created", customerToAdd);
            }
            catch (DuplicateRecordException ex)
            {
                return Conflict(ex.Message);
            }
        }
        [HttpGet("GetCustomerOrder/{userId}")]
        public ActionResult<List<Order>> GetCustomerOrders(int userId, string select)
        {
            if (select == "date")
            {
                List<Order> allOrders = _bl.GetAllOrdersDate(userId);
                return Ok(allOrders);
            }

            else if (select == "price")
            {
                List<Order> allOrders = _bl.GetAllOrdersPrice(userId);
                return Ok(allOrders);
            }
            else
            {
                return BadRequest();
            }
        }
    }


}

