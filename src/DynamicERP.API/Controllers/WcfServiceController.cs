using DynamicERP.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DynamicERP.API.Controllers;

/// <summary>
/// WCF servisleri için test controller'ı
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WcfServiceController : ControllerBase
{
    private readonly IWcfService _wcfService;
    private readonly ILogger<WcfServiceController> _logger;

    public WcfServiceController(IWcfService wcfService, ILogger<WcfServiceController> logger)
    {
        _wcfService = wcfService;
        _logger = logger;
    }

    /// <summary>
    /// WCF servisine istek gönderir
    /// </summary>
    [HttpPost("test-wcf")]
    public async Task<IActionResult> TestWcf([FromBody] TestWcfRequest request)
    {
        try
        {
            // Generic tipleri belirle
            var requestType = Type.GetType(request.RequestType) ?? typeof(object);
            var responseType = Type.GetType(request.ResponseType) ?? typeof(object);
            
            // Request'i deserialize et
            var requestData = JsonSerializer.Deserialize(request.RequestData, requestType);
            
            // Generic metod çağır
            var method = typeof(IWcfService).GetMethod("SendWcfRequestAsync")?.MakeGenericMethod(requestType, responseType);
            var task = (Task)method!.Invoke(_wcfService, new object[] { request.Url, request.SoapAction, requestData!, request.BindingType, request.Headers, request.Timeout })!;
            await task.ConfigureAwait(false);
            
            // Result'ı al
            var resultProperty = task.GetType().GetProperty("Result");
            var response = resultProperty!.GetValue(task);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WCF test hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// WCF güvenli servisine istek gönderir
    /// </summary>
    [HttpPost("test-wcf-secure")]
    public async Task<IActionResult> TestWcfSecure([FromBody] TestWcfSecureRequest request)
    {
        try
        {
            // Generic tipleri belirle
            var requestType = Type.GetType(request.RequestType) ?? typeof(object);
            var responseType = Type.GetType(request.ResponseType) ?? typeof(object);
            
            // Request'i deserialize et
            var requestData = JsonSerializer.Deserialize(request.RequestData, requestType);
            
            // Generic metod çağır
            var method = typeof(IWcfService).GetMethod("SendWcfSecureRequestAsync")?.MakeGenericMethod(requestType, responseType);
            var task = (Task)method!.Invoke(_wcfService, new object[] { request.Url, request.SoapAction, requestData!, request.Username, request.Password, request.Headers, request.Timeout })!;
            await task.ConfigureAwait(false);
            
            // Result'ı al
            var resultProperty = task.GetType().GetProperty("Result");
            var response = resultProperty!.GetValue(task);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WCF güvenli test hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// WCF servisinin WSDL'ini alır
    /// </summary>
    [HttpPost("get-wsdl")]
    public async Task<IActionResult> GetWsdl([FromBody] GetWsdlRequest request)
    {
        try
        {
            var wsdl = await _wcfService.GetWcfWsdlAsync(request.Url, request.Timeout);
            return Ok(new { Wsdl = wsdl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WSDL alma hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// WCF servisinin endpoint'lerini keşfeder
    /// </summary>
    [HttpPost("discover-endpoints")]
    public async Task<IActionResult> DiscoverEndpoints([FromBody] DiscoverEndpointsRequest request)
    {
        try
        {
            var endpoints = await _wcfService.DiscoverWcfEndpointsAsync(request.Url, request.Timeout);
            return Ok(new { Endpoints = endpoints });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Endpoint keşif hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }
}

/// <summary>
/// WCF test isteği modeli
/// </summary>
public class TestWcfRequest
{
    public string Url { get; set; } = string.Empty;
    public string SoapAction { get; set; } = string.Empty;
    public string RequestType { get; set; } = "System.Object";
    public string ResponseType { get; set; } = "System.Object";
    public JsonElement RequestData { get; set; }
    public WcfBindingType BindingType { get; set; } = WcfBindingType.BasicHttp;
    public Dictionary<string, string>? Headers { get; set; }
    public int Timeout { get; set; } = 30;
}

/// <summary>
/// WCF güvenli test isteği modeli
/// </summary>
public class TestWcfSecureRequest
{
    public string Url { get; set; } = string.Empty;
    public string SoapAction { get; set; } = string.Empty;
    public string RequestType { get; set; } = "System.Object";
    public string ResponseType { get; set; } = "System.Object";
    public JsonElement RequestData { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Dictionary<string, string>? Headers { get; set; }
    public int Timeout { get; set; } = 30;
}

/// <summary>
/// WSDL alma isteği modeli
/// </summary>
public class GetWsdlRequest
{
    public string Url { get; set; } = string.Empty;
    public int Timeout { get; set; } = 30;
}

/// <summary>
/// Endpoint keşif isteği modeli
/// </summary>
public class DiscoverEndpointsRequest
{
    public string Url { get; set; } = string.Empty;
    public int Timeout { get; set; } = 30;
} 