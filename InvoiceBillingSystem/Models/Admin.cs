using InvoiceBillingSystem.Enum;

namespace InvoiceBillingSystem.Models
{
    public class Admin
    {
        public Guid AdminId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }

        public string Role { get; set; } = "Admin";

        public Guid Company_Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
