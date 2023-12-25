using System;
using System.Runtime.Serialization;
using MediatR;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class SetIrimcConfirmedCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public bool IrimcConfirmed { get; set; }

        public SetIrimcConfirmedCommand(
                Guid userId,
                bool irimcConfirmed
                )
        {
            UserId = userId;
            IrimcConfirmed = irimcConfirmed;
        }
    }
}
