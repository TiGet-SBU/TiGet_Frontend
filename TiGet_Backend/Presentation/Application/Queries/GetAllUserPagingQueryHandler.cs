using MediatR;
using Microsoft.AspNetCore.Identity;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.BuildingBlocks.Common.Models;
using Rhazes.Services.Identity.API.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rhazes.BuildingBlocks.Common;

namespace Rhazes.Services.Identity.API.Application.Queries
{
    public class GetAllUserPagingQueryHandler : IRequestHandler<GetAllUserPagingQuery, DataSourceResult>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUserPagingQueryHandler(IUserRepository userRepository
            )
        {
            _userRepository = MethodParameterChecker.CheckUp(userRepository);
        }

        public async Task<DataSourceResult> Handle(GetAllUserPagingQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetAllPagingAsync(request);


            return result != null ? UserPagingDTO.FromApplicationUser(result) : null;
        }
    }


    public class UserPagingDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public static DataSourceResult FromApplicationUser(DataSourceResult dataSource)
        {
            DataSourceResult result = new DataSourceResult()
            {
                Total = dataSource.Total,
                AggregateResults = dataSource.AggregateResults
            };

            List<UserPagingDTO> users = new List<UserPagingDTO>();

            foreach (ApplicationUser applicationUser in dataSource.Data)
            {
                users.Add(new UserPagingDTO()
                {
                    Id = applicationUser.Id.ToString(),
                    LastName = applicationUser.LastName,
                    Name = applicationUser.Name,
                    Email = applicationUser.Email,
                    NationalCode = applicationUser.UserName,
                    UserName = applicationUser.UserName,
                    PhoneNumber = applicationUser.PhoneNumber,
                    UserType = applicationUser.UserType.ToString()
                });
            }

            result.Data = users;

            return result;
        }

    }


}

