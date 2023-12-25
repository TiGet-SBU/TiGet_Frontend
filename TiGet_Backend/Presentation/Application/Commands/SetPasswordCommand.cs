using MediatR;
using Rhazes.Services.Identity.Infrastructure;
using System;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class SetPasswordCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string CurrentPassword { get; set; }
       
        public SetPasswordCommand(
        Guid id,
        string password,
        string confirmPassword,
        string currentPassword
        )
        {
            Id = id;
            Password = password;
            ConfirmPassword = confirmPassword;
            CurrentPassword = currentPassword;
        }
    }
}
