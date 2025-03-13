namespace SimpleAuthorization.Models
{
    public class UserRegistration
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}
