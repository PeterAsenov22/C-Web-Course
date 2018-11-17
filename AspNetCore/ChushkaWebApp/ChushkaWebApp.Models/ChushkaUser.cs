namespace ChushkaWebApp.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ChushkaUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
