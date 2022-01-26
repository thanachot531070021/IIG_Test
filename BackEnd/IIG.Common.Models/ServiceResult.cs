using System;
using System.Collections.Generic;
using System.Net;

namespace IIG.Common.Models
{
    public class ServiceResult
    {
        public int code { get; set; }
        public int errorLogId { get; set; }
        public List<string> messages { get; set; }
        public dynamic data { get; set; }
        public bool isSuccess { get; set; }


        public ServiceResult()
        {
            code = HttpStatusCode.OK.GetHashCode();
            isSuccess = true;
            messages = new List<string>();
        }

        public void DoFailed(params string[] pMessages)
        {
            code = HttpStatusCode.BadRequest.GetHashCode();
            isSuccess = false;
            messages.AddRange(pMessages);
        }

        public void DoFailed(int pCode, params string[] pMessages)
        {
            code = pCode;
            isSuccess = false;
            messages.AddRange(pMessages);
        }

        public void DoError(Exception ex, int? _errorLogId = null)
        {
            code = HttpStatusCode.InternalServerError.GetHashCode();
            isSuccess = false;
            messages.Add("Message: " + ex.Message);
            messages.Add("ExceptionType: " + ex.GetType().ToString());
            messages.Add("StackTrace" + ex.StackTrace);

            Exception innerException = ex.InnerException;
            while (innerException != null)
            {
                messages.Add("******************************");
                messages.Add("Message: " + innerException.Message);
                messages.Add("ExceptionType: " + innerException.GetType().ToString());
                messages.Add("StackTrace" + innerException.StackTrace);
                innerException = innerException.InnerException;
            }

            if (_errorLogId != null)
            {
                errorLogId = _errorLogId.Value;
            }
        }

        public void DoBadRequest(string messageBadRequest)
        {
            code = (int)HttpStatusCode.BadRequest;
            isSuccess = false;
            messages.Add(messageBadRequest);
        }
    }
}
