using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Contracts
{
    public interface IProductReservationFailed
    {
        Guid OrderId { get; }
        Guid ProductId { get; }
        int Quantity { get; }
        string Reason { get; }
    }
}
