namespace InvoiceBillingSystem.Models
{
    public class Company
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Ensure ID is generated
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property (One company can have multiple users)
        public ICollection<User> Users { get; set; } = new List<User>();
    }

}
