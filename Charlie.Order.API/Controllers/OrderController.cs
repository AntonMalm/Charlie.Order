using Charlie.Order.DataAccess.DataModels;
using Charlie.Order.DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IMessageQueueService _messageQueueService;

        public OrderController(
            IRepository<Order> orderRepository,
            IMessageQueueService messageQueueService)
        {
            _orderRepository = orderRepository;
            _messageQueueService = messageQueueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllAsync();

            // Map orders to DTOs
            var orderDtos = orders.Select(order => new OrderDTO
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                TotalPrice = order.TotalPrice,
                OrderItems = order.OrderItems.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            });

            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            // Map order to DTO
            var orderDto = new OrderDTO
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                TotalPrice = order.TotalPrice,
                OrderItems = order.OrderItems.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder(OrderCreateDTO orderDto)
        {
            // Validate customer using RabbitMQ
            var customerResponse = await _messageQueueService.RequestAsync<CustomerDTO>("GetCustomer", orderDto.CustomerId);
            if (customerResponse == null)
            {
                return BadRequest("Invalid CustomerId");
            }

            // Create order entity
            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                TotalPrice = orderDto.TotalPrice,
                OrderItems = new List<OrderItem>()
            };

            foreach (var orderItemDto in orderDto.OrderItems)
            {
                // Validate product using RabbitMQ
                var productResponse = await _messageQueueService.RequestAsync<ProductDTO>("GetProduct", orderItemDto.ProductId);
                if (productResponse == null)
                {
                    return BadRequest($"Invalid ProductId: {orderItemDto.ProductId}");
                }

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = orderItemDto.ProductId,
                    Quantity = orderItemDto.Quantity,
                    Price = productResponse.Price
                });
            }

            await _orderRepository.AddAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, OrderUpdateDTO updatedOrderDto)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            // Update order details
            existingOrder.TotalPrice = updatedOrderDto.TotalPrice;

            // Update order items
            existingOrder.OrderItems.Clear();
            foreach (var itemDto in updatedOrderDto.OrderItems)
            {
                var productResponse = await _messageQueueService.RequestAsync<ProductDTO>("GetProduct", itemDto.ProductId);
                if (productResponse == null)
                {
                    return BadRequest($"Invalid ProductId: {itemDto.ProductId}");
                }

                existingOrder.OrderItems.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = productResponse.Price
                });
            }

            await _orderRepository.UpdateAsync(existingOrder);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            await _orderRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}