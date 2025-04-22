namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; } // Foreign key for Role
        public Role Roles { get; set; } = new Role(); // Navigation property
    }
}
