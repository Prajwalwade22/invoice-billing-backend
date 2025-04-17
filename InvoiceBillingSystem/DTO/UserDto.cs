namespace InvoiceBillingSystem.DTO
{
    public class UserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid CompanyId { get; set; } 
        public string Phone { get; set; }
    }
}
