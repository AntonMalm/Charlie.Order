namespace Charlie.Order.DataAccess.Dtos
{
    public class OrderDTO
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public List<OrderProductDTO> Products { get; set; }
    }
}
