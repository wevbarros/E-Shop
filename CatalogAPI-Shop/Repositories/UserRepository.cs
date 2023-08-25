namespace apiYuGiOh.Repository
{
    using User.Models;
    public static class UserRepository
    {
        public static User Get(string name, string password)
        {
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "Yugi", Password = "Yugi", Role = "admin" },
                new User { Id = Guid.NewGuid(), Name = "Weevil", Password = "Weevil", Role = "user" }
            };
            return users.Where(x => x.Name.ToLower() == name.ToLower() && x.Password == x.Password).FirstOrDefault();
        }
    }
}