namespace SecurePrivacyTask1.Models.DTO
{

    public class UserDto
    {
        public string Email { get; set; }       
        public string UserName { get; set; }    
        public string Address { get; set; }     
        public string City { get; set; }        
        public string Phone { get; set; }       
        public bool ConsentGiven { get; set; }  
        public bool CanCreateUsers { get; set; }
        public bool CanDeleteUsers { get; set; }
        public bool CanEditUsers { get; set; }  
    }

}