using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.DTO
{
    public class UserRegisterDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalCode { get; set; }
        public Guid NationalityId { get; set; }
        public int UserType { get; set; }
        public Guid CreateById { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? Deleted { get; set; }

        public static UserRegisterDTO FromApplicationUser(ApplicationUser applicationUser)
        {
            return new UserRegisterDTO()
            {
                Id = applicationUser.Id,
                CreateById = applicationUser.CreateById,
                CreateDate = applicationUser.CreateDate,
                Deleted = applicationUser.Deleted,
                LastName = applicationUser.LastName,
                Name = applicationUser.Name,
                UserType = applicationUser.UserType,
                UserName = applicationUser.UserName

            };

        }
    }
}
