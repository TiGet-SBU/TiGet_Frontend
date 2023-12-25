using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rhazes.Services.Identity.Domain.Validators
{
    internal static class LogSerializer
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            Formatting = Formatting.Indented
        };

        static LogSerializer()
        {
            JsonSettings.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="logObject">The object.</param>
        /// <returns></returns>
        public static string Serialize(object logObject)
        {
            return JsonConvert.SerializeObject(logObject, JsonSettings);
        }
    }
}
