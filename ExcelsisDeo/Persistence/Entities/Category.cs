namespace ExcelsisDeo.Persistence.Entities
{
    public class Category : AuditableEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
