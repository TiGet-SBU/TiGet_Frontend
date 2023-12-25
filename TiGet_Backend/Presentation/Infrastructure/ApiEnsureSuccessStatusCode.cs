using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rhazes.BuildingBlocks.Common.Exception;
using System;
using System.Net.Http;
using System.Web.Http;

namespace Rhazes.Services.Identity.API.Infrastructure
{
    public static class ApiEnsureSuccessStatusCode
    {
        public static void EnsureSuccessStatus(this HttpResponseMessage response)
        {

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                var result = string.Empty;
                ValidationProblemDetails validationProblemDetails = null;
                try
                {
                    result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    validationProblemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(result);
                }
                catch (Exception)
                {
                    throw new HttpResponseException(response);
                }


                if (validationProblemDetails != null && validationProblemDetails.Errors.Count > 0)
                    throw new DomainException(validationProblemDetails.Title, validationProblemDetails.Errors);
                else
                {
                    if (string.IsNullOrEmpty(result))
                        throw new HttpResponseException(response);
                    else
                    {

                        string message = "";
                        Exception exception = null;
                        try
                        {
                            exception = JsonConvert.DeserializeObject<Exception>(result);
                            do
                            {
                                message = exception.Message;
                                exception = exception.InnerException;
                            } while (exception != null);
                        }
                        catch (Exception)
                        {
                            throw new HttpResponseException(response);
                        }

                        throw new Exception(message);

                    }
                }

            }

        }
    }
}
