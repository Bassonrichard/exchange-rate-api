using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ExchangeRate.Api.Models.Errors
{
    public class NotFoundError : ApiError
    {
        public NotFoundError()
            : base(404, HttpStatusCode.NotFound.ToString())
        {
        }


        public NotFoundError(string message)
            : base(404, HttpStatusCode.NotFound.ToString(), message)
        {
        }
    }
}
