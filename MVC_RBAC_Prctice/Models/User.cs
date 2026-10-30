namespace MVC_RBAC_Prctice.Models
{
    public class User
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Roles { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}
