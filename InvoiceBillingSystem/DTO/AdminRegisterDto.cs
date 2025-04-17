namespace InvoiceBillingSystem.DTO
{
    public class AdminRegisterDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }
        public Guid Company_Id { get; set; }
    }
}
