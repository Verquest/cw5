using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw5.Models;
using cw5.services;
using Microsoft.AspNetCore.Mvc;

namespace cw5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _service;
        
        public WarehousesController(IWarehouseService service){
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int idProduct, int idWarehouse, int amount)
        {
            var doesProductExist = await _service.DoesProductExist(idProduct);
            var doesWarehouseExist = await _service.DoesWarehouseExist(idWarehouse);
            if(!doesProductExist || !doesWarehouseExist){
                return NotFound("Product/Warehouse does not exist");
            }

            var orderId = await _service.GetTheValidOrderId(idProduct, amount);
            var order = new Order
            {
                Id = orderId,
                IdProduct = idProduct,
                IdWarehouse = idWarehouse,
                Amount = amount,
                CreatedAt = DateTime.Now,
                FulfilledAt = null
            };

            if(orderId == -1)
            {
                return NotFound("Order does not exist");
            }
            if(orderId == -2)
            {
                return NotFound("Order has already been fulfilled");
            }
            var hasOrderBeenFulfilled = await _service.HasOrderBeenFulfilled(orderId);
            if(hasOrderBeenFulfilled){
                return NotFound("Order has already been fulfilled");
            }
            if(doesProductExist)
            {
                var idOfOrder = await _service.CompeleteTheOrder(order);
                return Ok("Order created, id=" + idOfOrder);
            }
        
            return BadRequest("Order creation time invalid");
        }
        
        [HttpGet]
         public IActionResult CompleteOrder()
        {
            return Ok();
        }
    }
}