using CoinCapDemo.Exceptions;
using log4net;
using RestSharp;
using System.Reflection;

namespace CoinCapDemo.Facades.Impl
{
    internal class ApiClient : IApiClient
    {

        private readonly RestClient restClient;
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public ApiClient(string baseUrl, int timeout)
        {
            var options = new RestClientOptions(baseUrl)
            {
                ThrowOnAnyError = true,
                MaxTimeout = timeout
            };

            restClient = new RestClient(options);
        }

        public T Delete<T>(string resourceUrl, object? body = null, Dictionary<string, string>? urlParams = null) where T : new()
        {
            return Action<T>(resourceUrl, Method.Delete, body, urlParams);
        }

        public T Get<T>(string resourceUrl, object? body = null, Dictionary<string, string>? urlParams = null) where T : new()
        {
            return Action<T>(resourceUrl, Method.Get, body, urlParams);
        }

        public T Patch<T>(string resourceUrl, object? body = null, Dictionary<string, string>? urlParams = null) where T : new()
        {
            return Action<T>(resourceUrl, Method.Patch, body, urlParams);
        }

        public T Post<T>(string resourceUrl, object? body = null, Dictionary<string, string>? urlParams = null) where T : new()
        {
            return Action<T>(resourceUrl, Method.Post, body, urlParams);
        }

        private T Action<T>(string resourceUrl, Method method, object? body, Dictionary<string, string>? urlParams) where T : new()
        {
            var request = new RestRequest(resourceUrl, method);

            AddRequestParams(ref request, urlParams);

            AddJsonBody(ref request, body);

            var response = restClient.Execute<T>(request);

            if (!response.IsSuccessful)
            {
                logger.Error($"Action: {method} - Resource: {resourceUrl} - Response Status Code: {response.StatusCode} - Response Content: {response.Content}");
                throw new BadGatewayException($"Ha ocurrido un error al interactuar con el recurso {method} - {resourceUrl}");
            }

            return response.Data != null ? response.Data : new T();
        }

        private void AddJsonBody(ref RestRequest request, object? body)
        {
            if (body != null)
            {
                request.AddJsonBody(body);
            }
        }

        private void AddRequestParams(ref RestRequest request, Dictionary<string, string>? urlParams)
        {
            if (urlParams != null)
            {
                foreach (var key in urlParams.Keys)
                {
                    request.AddParameter(key, urlParams[key], ParameterType.QueryString);
                }
            }
        }
    }
}
