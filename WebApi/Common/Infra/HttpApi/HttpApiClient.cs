using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace Common.Infra.HttpApi;

/// <summary>
/// Represents a client for making HTTP API requests.
/// </summary>
  public class HttpApiClient
  {
      /// <summary>
      /// The HTTP client used for making requests.
      /// </summary>
      private static readonly HttpClient _client = new();

      /// <summary>
      /// The base URL for the API.
      /// </summary>
      private readonly string _baseUrl;

      /// <summary>
      /// Initializes a new instance of the <see cref="HttpApiClient"/> class with the specified base URL.
      /// </summary>
      /// <param name="baseUrl"></param>
      public HttpApiClient(string baseUrl)
      {
          _baseUrl = baseUrl; // Ensure the base URL ends with a slash
      }

      /// <summary>
      /// Sends a PUT request to the specified endpoint with the provided data.
      /// </summary>
      /// <param name="endPoint"></param>
      /// <param name="data"></param>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public async Task<HttpApiJsonResponse?> PutAsync<T>(string endPoint, T data)
      {
          var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
          var httpResponseMessage = await _client.PutAsync($"{_baseUrl}{endPoint}", content);

          var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
      }

      /// <summary>
      /// Sends a PUT request to the specified endpoint without any data.
      /// </summary>
      /// <param name="endPoint"></param>
      /// <returns></returns>
      public async Task<HttpApiJsonResponse?> PutAsync(string endPoint)
      {
          var httpResponseMessage = await _client.PutAsync($"{_baseUrl}{endPoint}", null);

          var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
      }

      /// <summary>
      /// Sends a POST request to the specified endpoint with the provided data.
      /// </summary>
      /// <param name="endPoint"></param>
      /// <param name="data"></param>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public async Task<HttpApiJsonResponse?> PostAsync<T>(string endPoint, T data)
      {
          var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
          var httpResponseMessage = await _client.PostAsync($"{_baseUrl}{endPoint}", content);

          var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
          if (string.IsNullOrWhiteSpace(responseContent))
          {
              return new HttpApiJsonResponse(false, httpResponseMessage.ReasonPhrase, DateTime.UtcNow);
          }
          try
          {
              return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
          }
          catch
          {
              return new HttpApiJsonResponse(false, responseContent, DateTime.UtcNow);
          }
      }

      /// <summary>
      /// Sends a POST request to the specified endpoint without any data.
      /// </summary>
      /// <param name="endPoint"></param>
      /// <returns></returns>
      public async Task<HttpApiJsonResponse?> PostAsync(string endPoint)
      {
          var httpResponseMessage = await _client.PostAsync($"{_baseUrl}{endPoint}", null);

          var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
      }

      /// <summary>
      /// Sends a GET request to the specified endpoint and returns the response as a JSON object.
      /// </summary>
      /// <param name="endPoint"></param>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public async Task<T?> GetAsync<T>(string endPoint)
      {
          var stream = await _client.GetStringAsync($"{_baseUrl}{endPoint}").ConfigureAwait(false);
          return JsonConvert.DeserializeObject<T>(stream);
      }

      /// <summary>
      /// Sends a GET request to the specified endpoint with the provided query parameters.
      /// </summary>
      /// <param name="endPoint"></param>
      /// <param name="args"></param>
      /// <returns></returns>
      public async Task<HttpApiJsonResponse?> DeleteAsync(string endPoint, Dictionary<string, object>? args = null)
      {
          var queryString = args == null ? "" : $"?{args.Keys.Select(k => $"{k}={args[k]}").Aggregate((x, y) => $"{x}&{y}")}";
          var httpResponseMessage = await _client.DeleteAsync($"{_baseUrl}{endPoint}{queryString}");

          var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
          return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
      }

      /// <summary>
      /// Sends a POST request to the specified endpoint with the provided data and returns the response as a JSON object.
      /// </summary>
      /// <param name="endPoint"></param>
      /// <param name="data"></param>
      /// <typeparam name="TRequest"></typeparam>
      /// <typeparam name="TResponse"></typeparam>
      /// <returns></returns>
      public async Task<HttpApiJsonResponse<TResponse>?> PostAsync<TRequest, TResponse>(string endPoint, TRequest data)
      where TResponse : class
      {
          var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
          var httpResponseMessage = await _client.PostAsync($"{_baseUrl}{endPoint}", content);

          var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

          if (string.IsNullOrWhiteSpace(responseContent))
          {
              return new HttpApiJsonResponse<TResponse>(false, httpResponseMessage.ReasonPhrase, default, DateTime.UtcNow);
          }

          try
          {
              return JsonConvert.DeserializeObject<HttpApiJsonResponse<TResponse>>(responseContent);
          }
          catch
          {
              return new HttpApiJsonResponse<TResponse>(false, responseContent, default, DateTime.UtcNow);
          }
      }

      /// <summary>
      /// Sends a GET request to the specified endpoint with the provided request body and returns the response as a JSON object.
      /// </summary>
      /// <param name="endPoint"></param>
      /// <param name="requestBody"></param>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public async Task<HttpApiJsonResponse<T>> GetJsonAsync<T>(string endPoint, object? requestBody = null)
      {
          var request = new HttpRequestMessage
          {
              Method = HttpMethod.Get,
              RequestUri = new Uri(_baseUrl + endPoint),
              Content = requestBody == null
                  ? null
                  : new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, MediaTypeNames.Application.Json),
          };

          using var httpClient = new HttpClient();
          var httpResponseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);
          return await ParseResponseAsync<T>(httpResponseMessage).ConfigureAwait(false);
      }

      /// <summary>
      /// Sends a POST request to the specified endpoint with the provided data and returns the response as a JSON object.
      /// </summary>
      /// <param name="endPoint"></param>
      /// <param name="data"></param>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      public async Task<HttpApiJsonResponse<T>> PostJsonAsync<T>(string endPoint, object data)
      {
          using var httpClient = new HttpClient();
          var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
          var httpResponseMessage = await httpClient.PostAsync($"{_baseUrl}{endPoint}", content);
          return await ParseResponseAsync<T>(httpResponseMessage).ConfigureAwait(false);
      }

      /// <summary>
      /// Parses the response from the HTTP request and returns it as a JSON object.
      /// </summary>
      /// <param name="httpResponseMessage"></param>
      /// <typeparam name="T"></typeparam>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
      private static async Task<HttpApiJsonResponse<T>> ParseResponseAsync<T>(HttpResponseMessage httpResponseMessage)
      {
          var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
          if (string.IsNullOrWhiteSpace(responseContent))
          {
              return new HttpApiJsonResponse<T>(false, httpResponseMessage.ReasonPhrase, default, DateTime.UtcNow);
          }
          else
          {
              return JsonConvert.DeserializeObject<HttpApiJsonResponse<T>>(responseContent)
                  ?? throw new Exception("Ocurrió un error al intentar deserealizar la respuesta de la API.");
          }
      }
  }