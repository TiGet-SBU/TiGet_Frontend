using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    using OtpSharp;
    using System;
    using System.Text;
    internal static class CustomRfc6238AuthenticationService
    {
        internal static int ComputeTotp(byte[] securityToken)
        {
            var totp = new Totp(securityToken, 180, OtpHashMode.Sha512, 6);
            return int.Parse(totp.ComputeTotp());
        }

        public static int GenerateCode(byte[] securityToken)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            return ComputeTotp(securityToken);
        }

        public static bool ValidateCode(byte[] securityToken, string code)
        {
            var totp = new Totp(securityToken, 180, OtpHashMode.Sha512, 6);

            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }
            long timeStepMatched;
            if (totp.VerifyTotp(DateTime.UtcNow, code, out timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay))
            {
                return true;
            }

            return false;
        }
    }
}
