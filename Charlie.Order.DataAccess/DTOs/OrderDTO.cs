using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charlie.Order.DataAccess.Dtos
{
    public class OrderDTO
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public List<OrderProductDTO> Products { get; set; }
    }
}
