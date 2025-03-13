namespace SimpleAuthorization.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string Fname { get; set; } 
        public string Lname { get; set; } 
        public string City { get; set; } 
        public string Phone { get; set; } 
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
    }
}
