using System;

namespace WEB_API_Practice_Project_1.Models.BEL
{
    public class QueryUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public string QueryText { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal? QueryAmount { get; set; }
    }
}
