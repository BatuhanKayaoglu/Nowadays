using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nowadays.Common.Results
{
    //I will come back to this when a validation error occurs at some point.
    public class ValidationResponseModel
    {
        public IEnumerable<string> Errors { get; set; }
        public int HttpStatusCode { get; set; }
        public string? ErrorDetails { get; set; }


        public ValidationResponseModel()
        {
            Errors = new List<string>();
            HttpStatusCode = 500;
            ErrorDetails = string.Empty;
        }


        public ValidationResponseModel(IEnumerable<string> errors, int httpStatusCode = 500, string errorDetails = "")
        {
            Errors = errors;
            HttpStatusCode = httpStatusCode;
            ErrorDetails = errorDetails;
        }

        public ValidationResponseModel(string errorMessage, int httpStatusCode = 500, string errorDetails = "")
            : this(new List<string> { errorMessage }, httpStatusCode, errorDetails)
        {

        }

        [JsonIgnore]
        public string FlattenErrors => Errors != null ? string.Join(Environment.NewLine, Errors) : string.Empty;   //  For my own use within the system. I don't need to go back outside.

    }
}
