namespace DI_Practice.Models
{
    public class QueryUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public string City { get; set; } = null!;
        public string StateName { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string QueryText { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public decimal? QueryAmount { get; set; }
    }
}
