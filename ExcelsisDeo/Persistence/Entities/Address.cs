namespace ExcelsisDeo.Persistence.Entities
{
    public class Address
    {
        public required Guid Id { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string PostalCode { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
