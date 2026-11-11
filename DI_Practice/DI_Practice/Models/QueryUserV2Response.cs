namespace DI_Practice.Models
{
    public class QueryUserV2Response
    {
        public string Status { get; set; } = "Success";
        public int StatusCode { get; set; } = 200;
        public string ApiVersion { get; set; } = "2.0";
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public int TotalCount { get; set; }
        public List<QueryUserV2Dto> Data { get; set; } = [];
    }

    public class QueryUserV2Dto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public bool IsActive { get; set; }

        public ContactDetails Contact { get; set; } = new();
        public LocationDetails Location { get; set; } = new();
        public QueryDetails Query { get; set; } = new();
    }

    public class ContactDetails
    {
        public string Email { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
    }

    public class LocationDetails
    {
        public string City { get; set; } = null!;
        public string StateName { get; set; } = null!;
        public string Country { get; set; } = null!;
    }

    public class QueryDetails
    {
        public string Text { get; set; } = null!;
        public decimal? Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
