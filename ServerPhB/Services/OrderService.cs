using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServerPhB.Models;
using ServerPhB.Data;

namespace ServerPhB.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public Order TrackOrder(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public async Task<List<Order>> GetOrdersByClientId(string clientId)
        {
            return await _context.Orders.Where(o => o.ClientID == clientId).ToListAsync();
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
        }

        public async Task<List<Order>> GetOrdersByStatus(string status)
        {
            return await _context.Orders.Where(o => o.Status == status).ToListAsync();
        }
    }
}
