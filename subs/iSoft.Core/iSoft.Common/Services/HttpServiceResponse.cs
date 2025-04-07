using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Services
{
  public class HttpServiceResponse
  {
    public HttpStatusCode StatusCode { get; set; }
    public string ResponseString { get; set; }
    public HttpServiceResponse(HttpStatusCode statusCode, string responseString)
    {
      StatusCode = statusCode;
      ResponseString = responseString;
    }
  }
}
