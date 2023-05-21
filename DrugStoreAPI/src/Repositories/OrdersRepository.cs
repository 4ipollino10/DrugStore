using DrugStoreAPI.Data;
using DrugStoreAPI.DTOs.OrderDTOs;
using DrugStoreAPI.Entities;
using DrugStoreAPI.src.Repositories;
using DrugStoreAPI.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace DrugStoreAPI.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public OrdersRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Order> InsertOrder(Order order)
        {
            var insertedOrder = await applicationDbContext.Orders.AddAsync(order);
            await applicationDbContext.SaveChangesAsync();

            return insertedOrder.Entity;
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await applicationDbContext.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await applicationDbContext.Orders.ToListAsync();
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            var currentOrder = await applicationDbContext.Orders.FindAsync(order.Id);
            
            foreach(var orderDrug in order.OrdersDrugs)
            {
                orderDrug.Drug = await applicationDbContext.Drugs.FindAsync(orderDrug.DrugId);
            }

            currentOrder.OrderStatus = order.OrderStatus;
            currentOrder.OrdersDrugs = order.OrdersDrugs;
            currentOrder.AppointedDate = order.AppointedDate;
            currentOrder.ReceivingDate = order.ReceivingDate;
            
            await applicationDbContext.SaveChangesAsync();

            return currentOrder;
        }

        public async Task<IEnumerable<Order>> FindOrderByTypeIs(OrderStatus type)
        {
            var result = applicationDbContext.Orders.Where(o => o.OrderStatus == type);
            
            return await result.ToListAsync();
        }
    }
}
