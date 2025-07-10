namespace DynamicERP.Core.Interfaces.Services;

/// <summary>
/// WCF servisleri için özel interface
/// Windows Communication Foundation servislerine özel metodlar içerir
/// </summary>
public interface IWcfService
{
    /// <summary>
    /// WCF servisine istek gönderir (BasicHttpBinding)
    /// </summary>
    /// <typeparam name="TRequest">İstek modeli tipi</typeparam>
    /// <typeparam name="TResponse">Yanıt modeli tipi</typeparam>
    /// <param name="url">WCF servis URL'i</param>
    /// <param name="soapAction">SOAP Action</param>
    /// <param name="request">Gönderilecek veri</param>
    /// <param name="bindingType">Binding tipi (BasicHttp, WSHttp, NetTcp)</param>
    /// <param name="headers">İsteğe eklenecek header'lar</param>
    /// <param name="timeout">Timeout süresi (saniye)</param>
    /// <returns>Generic yanıt modeli</returns>
    Task<HttpResponse<TResponse>> SendWcfRequestAsync<TRequest, TResponse>(
        string url, 
        string soapAction, 
        TRequest request, 
        WcfBindingType bindingType = WcfBindingType.BasicHttp,
        Dictionary<string, string>? headers = null, 
        int timeout = 30) 
        where TRequest : class 
        where TResponse : class;

    /// <summary>
    /// WCF servisine istek gönderir (WSHttpBinding)
    /// </summary>
    /// <typeparam name="TRequest">İstek modeli tipi</typeparam>
    /// <typeparam name="TResponse">Yanıt modeli tipi</typeparam>
    /// <param name="url">WCF servis URL'i</param>
    /// <param name="soapAction">SOAP Action</param>
    /// <param name="request">Gönderilecek veri</param>
    /// <param name="username">Kullanıcı adı (WS-Security)</param>
    /// <param name="password">Şifre (WS-Security)</param>
    /// <param name="headers">İsteğe eklenecek header'lar</param>
    /// <param name="timeout">Timeout süresi (saniye)</param>
    /// <returns>Generic yanıt modeli</returns>
    Task<HttpResponse<TResponse>> SendWcfSecureRequestAsync<TRequest, TResponse>(
        string url, 
        string soapAction, 
        TRequest request, 
        string username, 
        string password,
        Dictionary<string, string>? headers = null, 
        int timeout = 30) 
        where TRequest : class 
        where TResponse : class;

    /// <summary>
    /// WCF servisine istek gönderir (NetTcpBinding)
    /// </summary>
    /// <typeparam name="TRequest">İstek modeli tipi</typeparam>
    /// <typeparam name="TResponse">Yanıt modeli tipi</typeparam>
    /// <param name="url">WCF servis URL'i (net.tcp://)</param>
    /// <param name="soapAction">SOAP Action</param>
    /// <param name="request">Gönderilecek veri</param>
    /// <param name="headers">İsteğe eklenecek header'lar</param>
    /// <param name="timeout">Timeout süresi (saniye)</param>
    /// <returns>Generic yanıt modeli</returns>
    Task<HttpResponse<TResponse>> SendWcfTcpRequestAsync<TRequest, TResponse>(
        string url, 
        string soapAction, 
        TRequest request,
        Dictionary<string, string>? headers = null, 
        int timeout = 30) 
        where TRequest : class 
        where TResponse : class;

    /// <summary>
    /// WCF servisinin WSDL dokümantasyonunu alır
    /// </summary>
    /// <param name="url">WCF servis URL'i</param>
    /// <param name="timeout">Timeout süresi (saniye)</param>
    /// <returns>WSDL içeriği</returns>
    Task<string> GetWcfWsdlAsync(string url, int timeout = 30);

    /// <summary>
    /// WCF servisinin endpoint'lerini keşfeder
    /// </summary>
    /// <param name="url">WCF servis URL'i</param>
    /// <param name="timeout">Timeout süresi (saniye)</param>
    /// <returns>Endpoint listesi</returns>
    Task<List<string>> DiscoverWcfEndpointsAsync(string url, int timeout = 30);
}

/// <summary>
/// WCF Binding tipleri
/// </summary>
public enum WcfBindingType
{
    /// <summary>
    /// BasicHttpBinding - Temel HTTP binding
    /// </summary>
    BasicHttp,
    
    /// <summary>
    /// WSHttpBinding - WS-Security destekli HTTP binding
    /// </summary>
    WSHttp,
    
    /// <summary>
    /// NetTcpBinding - TCP binding
    /// </summary>
    NetTcp,
    
    /// <summary>
    /// NetNamedPipeBinding - Named Pipe binding
    /// </summary>
    NetNamedPipe,
    
    /// <summary>
    /// WebHttpBinding - REST benzeri HTTP binding
    /// </summary>
    WebHttp
} 