using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetUserByListIdQueryHandler : IRequestHandler<GetUserByListIdQuery, List<UserDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByListIdQueryHandler(
            IUserRepository userRepository
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
        }

        public async Task<List<UserDTO>> Handle(GetUserByListIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetByListIdAsync(request.listUserId);

            return result != null ? UserByListIdDTO.FromApplicationUser(result) : null;
        }
    }

    public class UserByListIdDTO
    {

        public static List<UserDTO> FromApplicationUser(List<ApplicationUser> listApplicationUser)
        {
            List<UserDTO> users = new List<UserDTO>();
            foreach (var applicationUser in listApplicationUser)
            {
                var user = new UserDTO()
                {
                    Id = applicationUser.Id,
                    LastName = applicationUser.LastName,
                    Name = applicationUser.Name,
                    Email = applicationUser.Email,
                    NationalCode = applicationUser.UserName,
                    UserName = applicationUser.UserName,
                    PhoneNumber = applicationUser.PhoneNumber,
                    MedicalLicenseNumber = applicationUser.MedicalLicenseNumber,

                    UserType = applicationUser.UserType
                };
                users.Add(user);
            }
           
            return users;
        }
    }

}

