namespace User.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set;}
        public string? Role { get; set; }
        
    }
}