using IdentityModel;
using IdentityServer4;
using Newtonsoft.Json;
using Rhazes.Services.Identity.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    internal static class StringExtensions
    {
        public static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsPresent(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static Dictionary<string, object> ToClaimsDictionary(this IEnumerable<Claim> claims)
        {
            var d = new Dictionary<string, object>();

            if (claims == null)
            {
                return d;
            }

            var distinctClaims = claims.Distinct(new ClaimComparer());

            foreach (var claim in distinctClaims)
            {
                if (!d.ContainsKey(claim.Type))
                {
                    d.Add(claim.Type, GetValue(claim));
                }
                else
                {
                    var value = d[claim.Type];

                    var list = value as List<object>;
                    if (list != null)
                    {
                        list.Add(GetValue(claim));
                    }
                    else
                    {
                        d.Remove(claim.Type);
                        d.Add(claim.Type, new List<object> { value, GetValue(claim) });
                    }
                }
            }

            return d;
        }

        private static object GetValue(Claim claim)
        {
            if (claim.ValueType == ClaimValueTypes.Integer ||
                claim.ValueType == ClaimValueTypes.Integer32)
            {
                Int32 value;
                if (Int32.TryParse(claim.Value, out value))
                {
                    return value;
                }
            }

            if (claim.ValueType == ClaimValueTypes.Integer64)
            {
                Int64 value;
                if (Int64.TryParse(claim.Value, out value))
                {
                    return value;
                }
            }

            if (claim.ValueType == ClaimValueTypes.Boolean)
            {
                bool value;
                if (bool.TryParse(claim.Value, out value))
                {
                    return value;
                }
            }

            if (claim.ValueType == IdentityServerConstants.ClaimValueTypes.Json)
            {
                try
                {
                    return JsonConvert.DeserializeObject(claim.Value);
                }
                catch { }
            }

            return claim.Value;
        }
    }

    internal static class DateTimeExtensions
    {
        public static bool HasExceeded(this DateTime creationTime, int seconds, DateTime now)
        {
            return (now > creationTime.AddSeconds(seconds));
        }

        public static int GetLifetimeInSeconds(this DateTime creationTime, DateTime now)
        {
            return ((int)(now - creationTime).TotalSeconds);
        }

        public static bool HasExpired(this DateTime? expirationTime, DateTime now)
        {
            if (expirationTime.HasValue &&
                expirationTime.Value.HasExpired(now))
            {
                return true;
            }

            return false;
        }

        public static bool HasExpired(this DateTime expirationTime, DateTime now)
        {
            if (now > expirationTime)
            {
                return true;
            }

            return false;
        }
    }

    internal class TokenValidationLog
    {
        // identity token
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public bool ValidateLifetime { get; set; }

        // access token
        public string AccessTokenType { get; set; }
        public string ExpectedScope { get; set; }
        public string TokenHandle { get; set; }
        public string JwtId { get; set; }

        // both
        public Dictionary<string, object> Claims { get; set; }

        public override string ToString()
        {
            return LogSerializer.Serialize(this);
        }
    }
}
