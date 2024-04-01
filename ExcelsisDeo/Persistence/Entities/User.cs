namespace ExcelsisDeo.Persistence.Entities
{
    public class User : AuditableEntity
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.User;

        public Address Address { get; set; }
        public Basket Basket { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}

public enum Role
{
    User = 0,
    Admin = 1
}
