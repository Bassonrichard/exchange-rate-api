using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ExchangeRate.Api.Models.Errors
{
    public class BadRequestError : ApiError
    {
        public BadRequestError()
            : base(400, HttpStatusCode.BadRequest.ToString())
        {
        }


        public BadRequestError(string message)
            : base(400, HttpStatusCode.BadRequest.ToString(), message)
        {
        }
    }
}