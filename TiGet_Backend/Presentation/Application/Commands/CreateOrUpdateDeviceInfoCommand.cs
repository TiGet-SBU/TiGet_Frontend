using MediatR;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using Rhazes.Services.PadidarServerIdentity.Models.HumanViewModels;
using System;
using System.Runtime.Serialization;

namespace Rhazes.Services.Identity.API.Application.Commands
{
    [DataContract]
    public class CreateOrUpdateDeviceInfoCommand : IRequest<bool>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string DeviceName { get; set; }
        public string AppVersion { get; set; }
        public string OS { get; set; }
        public string OSVersion { get; set; }
        public DateTime LastActivity { get; set; }
        public string GoogleToken { get; set; }
        public CreateOrUpdateDeviceInfoCommand(
        string fullName,
        string phoneNumber,
        string deviceName,
        string appVersion,
        string os,
        string osVersion,
        DateTime lastActivity,
        string googleToken
            )
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            DeviceName = deviceName;
            AppVersion = appVersion;
            OS = os;
            OSVersion = osVersion;
            LastActivity = lastActivity;
            GoogleToken = googleToken;
        }
    }
}
