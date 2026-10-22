using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GridView_ObjectDataSource_Practice
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
        public bool IsActive { get; set; }
        public decimal? QueryAmount { get; set; }
    }
}