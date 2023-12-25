using Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Application.DTO
{
    public class UserRegisterDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public static UserRegisterDTO FromApplicationUser(ApplicationUser applicationUser)
        {
            return new UserRegisterDTO()
            {
                Id = applicationUser.Id,
                LastName = applicationUser.LastName,
                Name = applicationUser.Name,

            };

        }
    }
}
