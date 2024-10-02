using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantFaves_BackEnd_.Models;

namespace RestaurantFaves_BackEnd_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
         private RestFaveDbContext dbContext = new RestFaveDbContext();

        [HttpGet()]
        public IActionResult GetAll(string? restaurant = null, bool? orderAgain = null)
        {
            List<Order> result = dbContext.Orders.ToList();
            if (restaurant != null)
            {
                result = result.Where(o => o.Restaurant.ToLower().Contains(restaurant.ToLower())).ToList();
            }
            if (orderAgain != null)
            {
                result = result.Where(o => o.OrderAgain == orderAgain).ToList();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Order result = dbContext.Orders.FirstOrDefault(o => o.Id == id);
            if (result == null)
            {
                return NotFound("No matching id");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost()]
        public IActionResult AddOrder([FromBody] Order newOrder)
        {
            newOrder.Id = 0;
            dbContext.Orders.Add(newOrder);
            dbContext.SaveChanges();
            return Created($"/api/Orders/{newOrder.Id}", newOrder);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Order updated)
        {
            if(updated.Id != id)
            {
                return BadRequest("Ids don't match");
            }
            if(!dbContext.Orders.Any(o => o.Id == id))
            {
                return NotFound("No matcing ids");
            }
            dbContext.Orders.Update(updated);
            dbContext.SaveChanges();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            Order result = dbContext.Orders.FirstOrDefault(o => o.Id == id);
            if (result == null)
            {
                return NotFound("No matching id");
            }
            else
            {
                dbContext.Orders.Remove(result);
                dbContext.SaveChanges();
                return NoContent();
            }
        }
    }
}
