using System.Text.Json.Serialization;

namespace ExcelsisDeo.Persistence.Entities
{
    public class Basket : AuditableEntity
    {
        public long Id { get; set; }

        public Guid UserId { get; init; }
        public User User { get; set; }
        
        public virtual List<BasketItem> BasketItems { get; set; } = new();
    }

    public class BasketItem
    {
        public long Id { get; set; }

        public required Guid ProductId { get; set; }

        public required uint Quantity { get; set; }

        public long BasketId { get; set; }
        [JsonIgnore]
        public Basket Basket { get; set; }
    }
}
