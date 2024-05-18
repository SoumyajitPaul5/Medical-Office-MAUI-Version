using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Office_MAUI.Utilities
{
    public static class Jeeves
    {
        // Base URI for the API on the Internet
        public static Uri DBUri = new Uri("https://mowebapi2024.azurewebsites.net/");

        // Method to create an ApiException from an HttpResponseMessage
        public static ApiException CreateApiException(HttpResponseMessage response)
        {
            // Read the response content as a string
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            // Define an anonymous type to deserialize the error object
            var anonymousErrorObject = new { message = "", errors = new Dictionary<string, string[]>() };

            // Deserialize the error object
            var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            // Create a new ApiException with the original HttpResponseMessage
            var ex = new ApiException(response);

            // Add the error message to the exception data if present
            if (deserializedErrorObject?.message != null)
            {
                ex.Data.Add(-1, deserializedErrorObject?.message);
            }

            // Add individual errors to the exception data if present
            if (deserializedErrorObject.errors != null)
            {
                foreach (var err in deserializedErrorObject.errors)
                {
                    ex.Data.Add(err.Key, err.Value[0]);
                }
            }

            return ex;
        }
    }
}
