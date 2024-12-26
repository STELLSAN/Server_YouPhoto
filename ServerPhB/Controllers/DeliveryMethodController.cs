using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerPhB.Data;
using ServerPhB.Models;

namespace ServerPhB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryMethodController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DeliveryMethodController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("fetch-delivery-methods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _context.DeliveryMethods.ToListAsync();
            return Ok(deliveryMethods);
        }
    }
}
