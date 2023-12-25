using Identity.Domain.AggregatesModel.UserAggregate;

namespace Identity.API.Application.DTO
{
    public class UserDeleteDTO
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public static UserDeleteDTO FromApplicationUser(ApplicationUser applicationUser)
        {
            return new UserDeleteDTO()
            {
                Id = applicationUser.Id.ToString(),
                LastName = applicationUser.LastName,
                Name = applicationUser.Name,
                Email = applicationUser.Email,
            };
        }

    }
}
