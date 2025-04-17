namespace InvoiceBillingSystem.Models
{
    public class UserActivity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string IpAddress { get; set; }
        public string NetworkAddress { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public string OS { get; set; }

        public string Country { get; set; }  
        public string City { get; set; }
        public string Region { get; set; }  
        public string ISP { get; set; }  
        public string ASN { get; set; } //Autonomous System Number (Network ID)
        public string Organization { get; set; } 

        public DateTime ActivityTime { get; set; }

        public DateTime? ActivityEndTime { get; set; } 
        public double? SessionDuration { get; set; }

       
    }

   
}
