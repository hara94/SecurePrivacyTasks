namespace SecurePrivacyTask1.Models.DTO
{
    public class RegisterDto
    {
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public bool ConsentGiven { get; set; }

        // Permissions
        public bool CanCreateUsers { get; set; }
        public bool CanDeleteUsers { get; set; }
        public bool CanEditUsers { get; set; }
    }
}