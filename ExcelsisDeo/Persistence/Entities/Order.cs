using ExcelsisDeo.Persistence.Entities;

namespace ExcelsisDeo.Persistence.Entities
{
    public class Order
    {
        public long Id { get; set; }

        public required string City { get; set; }
        public required string Street { get; set; }
        public required string PostalCode { get; set; }

        public OrderStatus Status { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public virtual List<OrderElement> OrderElements { get; set; } = new();
    }
}

public class OrderElement
{
    public long Id { get; set; }
    public required Guid ProductId { get; set; }
    public required uint Quantity { get; set; }

    public long OrderId { get; set; }
    public Order Order { get; set; }
}

public enum OrderStatus
{
    Placed = 0,
    Paid = 1,
    InRealization = 2,
    Sent = 3,
    Delivered = 4,
    Canceled = 5,
}
