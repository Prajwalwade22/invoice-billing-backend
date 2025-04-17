namespace InvoiceBillingSystem.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "Customer"; // Default role

        public string Phone {  get; set; }

        public Guid CompanyId { get; set; }
        public Company Company { get; set; } 
    }
}
