using CoinCapDemo.Facades.Impl;
using CoinCapDemo.Models.Responses;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System.Reflection;

var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

try
{
    logger.Info("Inicializando RestClient.....");
    var apiClient = new ApiClient("https://api.coincap.io", 5000);

    logger.Info("Realizando petición a recurso assets...");
    var assets = apiClient.Get<AssetsResponse>("v2/assets",
                                               urlParams: new Dictionary<string, string> { { "limit", "10" } }).Data;

    Console.WriteLine("Assets");
    Console.WriteLine("==========================");
    foreach (var asset in assets)
    {
        Console.WriteLine(JsonConvert.SerializeObject(asset));
    }
    Console.WriteLine("==========================\n");

    var firstAsset = assets.First();

    logger.Info($"Realizando petición a recurso assets/{firstAsset.Id}/history...");
    var assetHistory = apiClient.Get<AssetHistoryResponse>($"v2/assets/{firstAsset.Id}/history",
                                                           urlParams: new Dictionary<string, string> { { "interval", "m5" } }).Data;

    Console.WriteLine($"Asset history for {firstAsset.Name}");
    Console.WriteLine("==========================");
    foreach (var asset in assetHistory)
    {
        Console.WriteLine(JsonConvert.SerializeObject(asset));
    }
    Console.WriteLine("==========================\n");
    logger.Info("Ejecución finalizada exitosamente.");
}
catch (Exception ex)
{
    logger.Error("Ejecución finalizada con errores.", ex);
    Console.WriteLine("Ha ocurrido un error.", ex);
}

Console.ReadLine();
