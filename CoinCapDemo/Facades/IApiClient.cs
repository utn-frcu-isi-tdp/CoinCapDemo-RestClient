namespace CoinCapDemo.Facades
{
    internal interface IApiClient
    {

        T Post<T>(string resourceUrl, object? body1 = null, Dictionary<string, string>? urlParams = null) where T : new();

        T Get<T>(string resourceUrl, object? body = null, Dictionary<string, string>? urlParams = null) where T : new();

        T Patch<T>(string resourceUrl, object? body = null, Dictionary<string, string>? urlParams = null) where T : new();

        T Delete<T>(string resourceUrl, object? body = null, Dictionary<string, string>? urlParams = null) where T : new();

    }
}
