using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Office_MAUI.Utilities
{
    // Custom exception class to handle API-related exceptions
    public class ApiException : Exception
    {
        public HttpResponseMessage Response { get; set; } // HttpResponseMessage associated with the exception

        public ApiException(HttpResponseMessage response)
        {
            this.Response = response; // Constructor to initialize the ApiException with an HttpResponseMessage
        }

        // Property to get the HTTP status code of the response
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.Response.StatusCode;
            }
        }

        // Property to get a list of errors associated with the exception
        public IEnumerable<string> Errors
        {
            get
            {
                return this.Data.Values.Cast<string>().ToList();
            }
        }
    }
}
