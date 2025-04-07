using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iSoft.Common.Services
{
  public class HttpService
  {
    //private static readonly HttpClient _client =  new HttpClient();

    public static async Task<HttpServiceResponse> GetAsync(string uri, Dictionary<string, string> headers)
    {
      HttpRequestMessage requestMessage = new HttpRequestMessage()
      {
        Method = HttpMethod.Get,
        RequestUri = new Uri(uri)
      };

      // Add headers to the HttpRequestMessage
      if (headers != null)
      {
        foreach (var header in headers)
        {
          requestMessage.Headers.Add(header.Key, header.Value);
        }
      }
      HttpClient _client = new HttpClient();
      using HttpResponseMessage response = await _client.SendAsync(requestMessage);

      if (response != null)
      {
        if (response.StatusCode == HttpStatusCode.OK)
        {
          string json = await response.Content.ReadAsStringAsync();
          return new HttpServiceResponse(response.StatusCode, json);
        }
        else
        {
          return new HttpServiceResponse(response.StatusCode, response.ToString());
        }
      }
      return new HttpServiceResponse(HttpStatusCode.BadRequest, "response = null");
    }

    public static async Task<HttpServiceResponse> PostAsync(string uri, HttpMethod httpMethod, Dictionary<string, string> headers, object data = null, string serializedData = "", string contentType= "application/json")
    {
      if (data != null)
      {
        serializedData = JsonConvert.SerializeObject(data);
      }
      using HttpContent content = new StringContent(serializedData, Encoding.UTF8, contentType);

      HttpRequestMessage requestMessage = new HttpRequestMessage()
      {
        Content = content,
        Method = httpMethod,
        RequestUri = new Uri(uri)
      };

      // Add headers to the HttpRequestMessage
      if (headers != null)
      {
        foreach (var header in headers)
        {
          requestMessage.Headers.Add(header.Key, header.Value);
        }
      }
      HttpClient _client = new HttpClient();
      using HttpResponseMessage response = await _client.SendAsync(requestMessage);

      if (response != null)
      {
        if (response.StatusCode == HttpStatusCode.OK)
        {
          string json = await response.Content.ReadAsStringAsync();
          return new HttpServiceResponse(response.StatusCode, json);
        }
        else
        {
          return new HttpServiceResponse(response.StatusCode, response.ToString());
        }
      }
      return new HttpServiceResponse(HttpStatusCode.BadRequest, "response = null");
    }
  }
}
