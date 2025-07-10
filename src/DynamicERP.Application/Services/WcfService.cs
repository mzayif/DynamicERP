using DynamicERP.Core.Interfaces.Services;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace DynamicERP.Application.Services;

/// <summary>
/// WCF servisleri için implementasyon
/// Windows Communication Foundation servislerine özel metodlar
/// </summary>
public class WcfService : IWcfService
{
    private readonly HttpClient _httpClient;
    private readonly ILoggerService _logger;

    public WcfService(HttpClient httpClient, ILoggerService logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// WCF servisine istek gönderir (BasicHttpBinding)
    /// </summary>
    public async Task<HttpResponse<TResponse>> SendWcfRequestAsync<TRequest, TResponse>(
        string url, 
        string soapAction, 
        TRequest request, 
        WcfBindingType bindingType = WcfBindingType.BasicHttp,
        Dictionary<string, string>? headers = null, 
        int timeout = 30) 
        where TRequest : class 
        where TResponse : class
    {
        try
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Binding tipine göre URL'i ayarla
            var serviceUrl = GetServiceUrl(url, bindingType);
            
            // Request'i XML'e dönüştür
            string soapBody;
            if (request is string stringRequest)
            {
                soapBody = stringRequest;
            }
            else
            {
                soapBody = SerializeToXml(request);
            }

            // WCF için SOAP envelope oluştur
            var soapEnvelope = CreateWcfSoapEnvelope(soapBody, bindingType);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, serviceUrl)
            {
                Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml; charset=utf-8")
            };

            // WCF için gerekli header'ları ekle
            AddWcfHeaders(requestMessage, soapAction, bindingType);

            // Diğer header'ları ekle
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            // Timeout ayarla
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));

            var response = await _httpClient.SendAsync(requestMessage, cts.Token);
            stopwatch.Stop();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = new HttpResponse<TResponse>
            {
                StatusCode = (int)response.StatusCode,
                RawContent = responseContent,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            };

            // Response header'larını ekle
            foreach (var header in response.Headers)
            {
                result.Headers[header.Key] = string.Join(", ", header.Value);
            }

            // Yanıtı deserialize et
            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseContent))
            {
                try
                {
                    result.Data = DeserializeWcfResponse<TResponse>(responseContent, bindingType);
                }
                catch (Exception ex)
                {
                    result.HasDeserializationError = true;
                    result.DeserializationErrorMessage = ex.Message;
                    _logger.LogWarning($"WCF yanıt deserializasyon hatası: {ex.Message}");
                }
            }

            _logger.LogInformation($"WCF isteği tamamlandı: {serviceUrl}, Binding: {bindingType}, Durum: {response.StatusCode}, Süre: {stopwatch.ElapsedMilliseconds}ms");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"WCF isteği hatası: {url}, Binding: {bindingType}, Hata: {ex.Message}");
            return new HttpResponse<TResponse>
            {
                StatusCode = 0,
                RawContent = string.Empty,
                ErrorMessage = ex.Message,
                ElapsedMilliseconds = 0
            };
        }
    }

    /// <summary>
    /// WCF servisine güvenli istek gönderir (WSHttpBinding)
    /// </summary>
    public async Task<HttpResponse<TResponse>> SendWcfSecureRequestAsync<TRequest, TResponse>(
        string url, 
        string soapAction, 
        TRequest request, 
        string username, 
        string password,
        Dictionary<string, string>? headers = null, 
        int timeout = 30) 
        where TRequest : class 
        where TResponse : class
    {
        try
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Request'i XML'e dönüştür
            string soapBody;
            if (request is string stringRequest)
            {
                soapBody = stringRequest;
            }
            else
            {
                soapBody = SerializeToXml(request);
            }

            // WS-Security ile SOAP envelope oluştur
            var soapEnvelope = CreateWcfSecureSoapEnvelope(soapBody, username, password);

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml; charset=utf-8")
            };

            // WS-Security header'larını ekle
            AddWcfSecureHeaders(requestMessage, soapAction);

            // Diğer header'ları ekle
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            // Timeout ayarla
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));

            var response = await _httpClient.SendAsync(requestMessage, cts.Token);
            stopwatch.Stop();

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = new HttpResponse<TResponse>
            {
                StatusCode = (int)response.StatusCode,
                RawContent = responseContent,
                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            };

            // Response header'larını ekle
            foreach (var header in response.Headers)
            {
                result.Headers[header.Key] = string.Join(", ", header.Value);
            }

            // Yanıtı deserialize et
            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseContent))
            {
                try
                {
                    result.Data = DeserializeWcfResponse<TResponse>(responseContent, WcfBindingType.WSHttp);
                }
                catch (Exception ex)
                {
                    result.HasDeserializationError = true;
                    result.DeserializationErrorMessage = ex.Message;
                    _logger.LogWarning($"WCF güvenli yanıt deserializasyon hatası: {ex.Message}");
                }
            }

            _logger.LogInformation($"WCF güvenli istek tamamlandı: {url}, Durum: {response.StatusCode}, Süre: {stopwatch.ElapsedMilliseconds}ms");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"WCF güvenli istek hatası: {url}, Hata: {ex.Message}");
            return new HttpResponse<TResponse>
            {
                StatusCode = 0,
                RawContent = string.Empty,
                ErrorMessage = ex.Message,
                ElapsedMilliseconds = 0
            };
        }
    }

    /// <summary>
    /// WCF servisine TCP isteği gönderir (NetTcpBinding)
    /// </summary>
    public async Task<HttpResponse<TResponse>> SendWcfTcpRequestAsync<TRequest, TResponse>(
        string url, 
        string soapAction, 
        TRequest request,
        Dictionary<string, string>? headers = null, 
        int timeout = 30) 
        where TRequest : class 
        where TResponse : class
    {
        // NetTcpBinding için özel implementasyon gerekir
        // Bu örnekte HTTP üzerinden TCP benzeri davranış simüle ediyoruz
        return await SendWcfRequestAsync<TRequest, TResponse>(url, soapAction, request, WcfBindingType.NetTcp, headers, timeout);
    }

    /// <summary>
    /// WCF servisinin WSDL dokümantasyonunu alır
    /// </summary>
    public async Task<string> GetWcfWsdlAsync(string url, int timeout = 30)
    {
        try
        {
            var wsdlUrl = url.EndsWith("?wsdl") ? url : $"{url}?wsdl";
            
            var response = await _httpClient.GetAsync(wsdlUrl);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"WSDL alma hatası: {url}, Hata: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// WCF servisinin endpoint'lerini keşfeder
    /// </summary>
    public async Task<List<string>> DiscoverWcfEndpointsAsync(string url, int timeout = 30)
    {
        try
        {
            var endpoints = new List<string>();
            
            // WSDL'den endpoint'leri çıkar
            var wsdl = await GetWcfWsdlAsync(url, timeout);
            
            // XML'den service ve port bilgilerini çıkar
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(wsdl);
            
            var serviceNodes = xmlDoc.SelectNodes("//wsdl:service", GetWsdlNamespaceManager());
            if (serviceNodes != null)
            {
                foreach (XmlNode serviceNode in serviceNodes)
                {
                    var portNodes = serviceNode.SelectNodes(".//wsdl:port", GetWsdlNamespaceManager());
                    if (portNodes != null)
                    {
                        foreach (XmlNode portNode in portNodes)
                        {
                            var addressNode = portNode.SelectSingleNode(".//soap:address", GetWsdlNamespaceManager());
                            if (addressNode != null)
                            {
                                var location = addressNode.Attributes?["location"]?.Value;
                                if (!string.IsNullOrEmpty(location))
                                {
                                    endpoints.Add(location);
                                }
                            }
                        }
                    }
                }
            }
            
            return endpoints;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Endpoint keşif hatası: {url}, Hata: {ex.Message}");
            return new List<string>();
        }
    }

    #region Private Methods

    /// <summary>
    /// Binding tipine göre servis URL'ini ayarlar
    /// </summary>
    private string GetServiceUrl(string url, WcfBindingType bindingType)
    {
        return bindingType switch
        {
            WcfBindingType.NetTcp => url.Replace("http://", "net.tcp://").Replace("https://", "net.tcp://"),
            WcfBindingType.NetNamedPipe => url.Replace("http://", "net.pipe://").Replace("https://", "net.pipe://"),
            _ => url
        };
    }

    /// <summary>
    /// WCF için SOAP envelope oluşturur
    /// </summary>
    private string CreateWcfSoapEnvelope(string soapBody, WcfBindingType bindingType)
    {
        var envelope = bindingType switch
        {
            WcfBindingType.WSHttp => $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"">
  <soap:Header>
    <wsa:Action>{soapBody}</wsa:Action>
    <wsa:To>{soapBody}</wsa:To>
  </soap:Header>
  <soap:Body>
    {soapBody}
  </soap:Body>
</soap:Envelope>",
            
            _ => $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Header/>
  <soap:Body>
    {soapBody}
  </soap:Body>
</soap:Envelope>"
        };

        return envelope;
    }

    /// <summary>
    /// WS-Security ile SOAP envelope oluşturur
    /// </summary>
    private string CreateWcfSecureSoapEnvelope(string soapBody, string username, string password)
    {
        return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"">
  <soap:Header>
    <wsa:Action>http://tempuri.org/IService/Operation</wsa:Action>
    <wsa:To>http://tempuri.org/</wsa:To>
    <wsse:Security>
      <wsse:UsernameToken>
        <wsse:Username>{username}</wsse:Username>
        <wsse:Password>{password}</wsse:Password>
      </wsse:UsernameToken>
    </wsse:Security>
  </soap:Header>
  <soap:Body>
    {soapBody}
  </soap:Body>
</soap:Envelope>";
    }

    /// <summary>
    /// WCF header'larını ekler
    /// </summary>
    private void AddWcfHeaders(HttpRequestMessage request, string soapAction, WcfBindingType bindingType)
    {
        request.Headers.Add("SOAPAction", soapAction);
        
        if (bindingType == WcfBindingType.WSHttp)
        {
            request.Headers.Add("Content-Type", "application/soap+xml; charset=utf-8");
        }
        else
        {
            request.Headers.Add("Content-Type", "text/xml; charset=utf-8");
        }
    }

    /// <summary>
    /// WS-Security header'larını ekler
    /// </summary>
    private void AddWcfSecureHeaders(HttpRequestMessage request, string soapAction)
    {
        request.Headers.Add("SOAPAction", soapAction);
        request.Headers.Add("Content-Type", "application/soap+xml; charset=utf-8");
    }

    /// <summary>
    /// WCF yanıtını deserialize eder
    /// </summary>
    private T? DeserializeWcfResponse<T>(string xml, WcfBindingType bindingType) where T : class
    {
        if (typeof(T) == typeof(string))
            return xml as T;

        // SOAP envelope'dan body'yi çıkar
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        
        var bodyNode = xmlDoc.SelectSingleNode("//soap:Body", GetSoapNamespaceManager());
        if (bodyNode != null)
        {
            var innerXml = bodyNode.InnerXml;
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using var stringReader = new StringReader(innerXml);
            return serializer.Deserialize(stringReader) as T;
        }

        return null;
    }

    /// <summary>
    /// Object'i XML'e serialize eder
    /// </summary>
    private string SerializeToXml<T>(T obj)
    {
        if (obj is string stringObj)
            return stringObj;

        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, obj);
        return stringWriter.ToString();
    }

    /// <summary>
    /// SOAP namespace manager oluşturur
    /// </summary>
    private XmlNamespaceManager GetSoapNamespaceManager()
    {
        var nsManager = new XmlNamespaceManager(new NameTable());
        nsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
        nsManager.AddNamespace("soap12", "http://www.w3.org/2003/05/soap-envelope");
        return nsManager;
    }

    /// <summary>
    /// WSDL namespace manager oluşturur
    /// </summary>
    private XmlNamespaceManager GetWsdlNamespaceManager()
    {
        var nsManager = new XmlNamespaceManager(new NameTable());
        nsManager.AddNamespace("wsdl", "http://schemas.xmlsoap.org/wsdl/");
        nsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/wsdl/soap/");
        nsManager.AddNamespace("soap12", "http://schemas.xmlsoap.org/wsdl/soap12/");
        return nsManager;
    }

    #endregion
} 