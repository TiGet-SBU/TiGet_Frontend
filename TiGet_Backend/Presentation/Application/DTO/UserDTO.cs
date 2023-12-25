using System;

namespace Identity.API.Application.DTO
{
    public class UserDTO 
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int UserType { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Email { get; set; }
        

    }
}
