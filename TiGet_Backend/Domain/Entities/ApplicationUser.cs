using Domain.Enums;

namespace Domain.Entities
{
    public record ApplicationUser : User
    {
        public ApplicationUser(Role role, string email, string passwordHash, string phoneNumber)
        {
            Role = role;
            Email = email;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;
        }
    }
}
