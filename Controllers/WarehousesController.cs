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
        private IWarehouseService _service;
        
        public WarehousesController(IWarehouseService service){
            _service = service;
        }

        [HttpPost]
        public IActionResult CompleteOrder(Order order)
        {
            _service.DoesProductExist(order.IdProduct);
            _service.DoesWarehouseExist(order.IdWarehouse);
            _service.GetTheValidOrderId(order);
            _service.CompeleteTheOrder(order);

            return Ok(200);
        }
    }
}