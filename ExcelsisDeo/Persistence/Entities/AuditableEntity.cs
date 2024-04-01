namespace ExcelsisDeo.Persistence.Entities
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set;}
    }
}
