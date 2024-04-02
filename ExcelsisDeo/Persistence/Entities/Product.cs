namespace ExcelsisDeo.Persistence.Entities
{
    public class Product : AuditableEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required uint InStockQuantity { get; set; }
        //public required bool IsActive { get; set; }

        public required Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
