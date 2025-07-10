using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DynamicERP.API.Controllers;

/// <summary>
/// Generic HTTP servisleri için test controller'ı
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HttpServiceController : ControllerBase
{
    private readonly IHttpService _httpService;
    private readonly ILogger<HttpServiceController> _logger;

    public HttpServiceController(IHttpService httpService, ILogger<HttpServiceController> logger)
    {
        _httpService = httpService;
        _logger = logger;
    }

    #region Temel Test Metodları

    /// <summary>
    /// GET isteği test eder ve generic model döner
    /// </summary>
    [HttpPost("test-get")]
    public async Task<IActionResult> TestGet([FromBody] TestGetRequest request)
    {
        try
        {
            // Generic tip belirtmek için request'ten al
            var responseType = Type.GetType(request.ResponseType) ?? typeof(object);
            
            // Generic metod çağır
            var method = typeof(IHttpService).GetMethod("GetAsync")?.MakeGenericMethod(responseType);
            var task = (Task)method!.Invoke(_httpService, new object[] { request.Url, request.Headers, request.Timeout })!;
            await task.ConfigureAwait(false);
            
            // Result'ı al
            var resultProperty = task.GetType().GetProperty("Result");
            var response = resultProperty!.GetValue(task);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET test hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// POST isteği test eder ve generic model döner
    /// </summary>
    [HttpPost("test-post")]
    public async Task<IActionResult> TestPost([FromBody] TestPostRequest request)
    {
        try
        {
            // Generic tipleri belirle
            var requestType = Type.GetType(request.RequestType) ?? typeof(object);
            var responseType = Type.GetType(request.ResponseType) ?? typeof(object);
            
            // Request'i deserialize et
            var requestData = JsonSerializer.Deserialize(request.RequestData, requestType);
            
            // Generic metod çağır
            var method = typeof(IHttpService).GetMethod("PostAsync")?.MakeGenericMethod(requestType, responseType);
            var task = (Task)method!.Invoke(_httpService, new object[] { request.Url, requestData!, request.ContentType, request.Headers, request.Timeout })!;
            await task.ConfigureAwait(false);
            
            // Result'ı al
            var resultProperty = task.GetType().GetProperty("Result");
            var response = resultProperty!.GetValue(task);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST test hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    #endregion

    #region Konfigürasyonlu Test Metodları

    /// <summary>
    /// Konfigürasyonlu GET isteği test eder
    /// </summary>
    [HttpPost("test-get-config")]
    public async Task<IActionResult> TestGetConfig([FromBody] TestGetConfigRequest request)
    {
        try
        {
            // Generic tip belirtmek için request'ten al
            var responseType = Type.GetType(request.ResponseType) ?? typeof(object);
            
            // Generic metod çağır
            var method = typeof(IHttpService).GetMethod("GetAsync", new[] { typeof(string), typeof(string), typeof(Dictionary<string, string>), typeof(int?) })?.MakeGenericMethod(responseType);
            var task = (Task)method!.Invoke(_httpService, new object[] { request.ServiceName, request.Endpoint, request.Headers, request.Timeout })!;
            await task.ConfigureAwait(false);
            
            // Result'ı al
            var resultProperty = task.GetType().GetProperty("Result");
            var response = resultProperty!.GetValue(task);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Konfigürasyonlu GET test hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Konfigürasyonlu POST isteği test eder
    /// </summary>
    [HttpPost("test-post-config")]
    public async Task<IActionResult> TestPostConfig([FromBody] TestPostConfigRequest request)
    {
        try
        {
            // Generic tipleri belirle
            var requestType = Type.GetType(request.RequestType) ?? typeof(object);
            var responseType = Type.GetType(request.ResponseType) ?? typeof(object);
            
            // Request'i deserialize et
            var requestData = JsonSerializer.Deserialize(request.RequestData, requestType);
            
            // Generic metod çağır
            var method = typeof(IHttpService).GetMethod("PostAsync", new[] { typeof(string), typeof(string), typeof(object), typeof(string), typeof(Dictionary<string, string>), typeof(int?) })?.MakeGenericMethod(requestType, responseType);
            var task = (Task)method!.Invoke(_httpService, new object[] { request.ServiceName, request.Endpoint, requestData!, request.ContentType, request.Headers, request.Timeout })!;
            await task.ConfigureAwait(false);
            
            // Result'ı al
            var resultProperty = task.GetType().GetProperty("Result");
            var response = resultProperty!.GetValue(task);
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Konfigürasyonlu POST test hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    #endregion

    #region Konfigürasyon Yönetimi

    /// <summary>
    /// Servis konfigürasyonu ekler
    /// </summary>
    [HttpPost("add-config")]
    public IActionResult AddConfiguration([FromBody] AddConfigRequest request)
    {
        try
        {
            _httpService.AddServiceConfiguration(request.ServiceName, request.Config);
            return Ok(new { Message = $"Servis konfigürasyonu eklendi: {request.ServiceName}" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Konfigürasyon ekleme hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Servis konfigürasyonu günceller
    /// </summary>
    [HttpPut("update-config")]
    public IActionResult UpdateConfiguration([FromBody] AddConfigRequest request)
    {
        try
        {
            _httpService.UpdateServiceConfiguration(request.ServiceName, request.Config);
            return Ok(new { Message = $"Servis konfigürasyonu güncellendi: {request.ServiceName}" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Konfigürasyon güncelleme hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Servis konfigürasyonu kaldırır
    /// </summary>
    [HttpDelete("remove-config/{serviceName}")]
    public IActionResult RemoveConfiguration(string serviceName)
    {
        try
        {
            _httpService.RemoveServiceConfiguration(serviceName);
            return Ok(new { Message = $"Servis konfigürasyonu kaldırıldı: {serviceName}" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Konfigürasyon kaldırma hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Servis konfigürasyonu alır
    /// </summary>
    [HttpGet("get-config/{serviceName}")]
    public IActionResult GetConfiguration(string serviceName)
    {
        try
        {
            var config = _httpService.GetServiceConfiguration(serviceName);
            if (config == null)
                return NotFound(new { Error = $"Servis konfigürasyonu bulunamadı: {serviceName}" });

            return Ok(config);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Konfigürasyon alma hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Tüm servis konfigürasyonlarını alır
    /// </summary>
    [HttpGet("get-all-configs")]
    public IActionResult GetAllConfigurations()
    {
        try
        {
            var configs = _httpService.GetAllServiceConfigurations();
            return Ok(configs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Tüm konfigürasyonları alma hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Varsayılan servis adını ayarlar
    /// </summary>
    [HttpPost("set-default-service")]
    public IActionResult SetDefaultService([FromBody] SetDefaultServiceRequest request)
    {
        try
        {
            _httpService.SetDefaultService(request.ServiceName);
            return Ok(new { Message = $"Varsayılan servis ayarlandı: {request.ServiceName}" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Varsayılan servis ayarlama hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Varsayılan servis adını alır
    /// </summary>
    [HttpGet("get-default-service")]
    public IActionResult GetDefaultService()
    {
        try
        {
            var defaultService = _httpService.GetDefaultService();
            return Ok(new { DefaultService = defaultService });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Varsayılan servis alma hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// AppSettings'den yüklenen konfigürasyonları test eder
    /// </summary>
    [HttpGet("test-appsettings-config")]
    public IActionResult TestAppSettingsConfig()
    {
        try
        {
            var configs = _httpService.GetAllServiceConfigurations();
            var defaultService = _httpService.GetDefaultService();
            
            var result = new
            {
                Message = "AppSettings'den yüklenen konfigürasyonlar",
                DefaultService = defaultService,
                TotalServices = configs.Count,
                Services = configs.Select(c => new
                {
                    ServiceName = c.Key,
                    BaseUrl = c.Value.BaseUrl,
                    Timeout = c.Value.DefaultTimeout,
                    ContentType = c.Value.DefaultContentType,
                    HasAuthToken = !string.IsNullOrEmpty(c.Value.AuthToken),
                    HasApiKey = !string.IsNullOrEmpty(c.Value.ApiKey),
                    HasCredentials = !string.IsNullOrEmpty(c.Value.Username),
                    RetryCount = c.Value.RetryCount,
                    RetryDelay = c.Value.RetryDelayMs
                }).ToList()
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AppSettings konfigürasyon test hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// PaymentAPI servisini test eder
    /// </summary>
    [HttpGet("test-payment-api")]
    public async Task<IActionResult> TestPaymentApi()
    {
        try
        {
            // PaymentAPI konfigürasyonunu kontrol et
            var config = _httpService.GetServiceConfiguration("PaymentAPI");
            if (config == null)
                return NotFound(new { Error = "PaymentAPI konfigürasyonu bulunamadı" });

            // Test isteği gönder (gerçek servis olmadığı için mock response döner)
            var response = await _httpService.GetAsync<object>("PaymentAPI", "/health");
            
            return Ok(new
            {
                Message = "PaymentAPI test edildi",
                Config = new
                {
                    BaseUrl = config.BaseUrl,
                    Timeout = config.DefaultTimeout,
                    HasAuthToken = !string.IsNullOrEmpty(config.AuthToken),
                    RetryCount = config.RetryCount
                },
                Response = new
                {
                    StatusCode = response.StatusCode,
                    IsSuccess = response.IsSuccessStatusCode,
                    ElapsedMs = response.ElapsedMilliseconds
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PaymentAPI test hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// ECommerceAPI servisini test eder
    /// </summary>
    [HttpGet("test-ecommerce-api")]
    public async Task<IActionResult> TestECommerceApi()
    {
        try
        {
            // ECommerceAPI konfigürasyonunu kontrol et
            var config = _httpService.GetServiceConfiguration("ECommerceAPI");
            if (config == null)
                return NotFound(new { Error = "ECommerceAPI konfigürasyonu bulunamadı" });

            // Test isteği gönder (gerçek servis olmadığı için mock response döner)
            var response = await _httpService.GetAsync<object>("ECommerceAPI", "/health");
            
            return Ok(new
            {
                Message = "ECommerceAPI test edildi",
                Config = new
                {
                    BaseUrl = config.BaseUrl,
                    Timeout = config.DefaultTimeout,
                    HasApiKey = !string.IsNullOrEmpty(config.ApiKey),
                    HasCredentials = !string.IsNullOrEmpty(config.Username),
                    RetryCount = config.RetryCount
                },
                Response = new
                {
                    StatusCode = response.StatusCode,
                    IsSuccess = response.IsSuccessStatusCode,
                    ElapsedMs = response.ElapsedMilliseconds
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ECommerceAPI test hatası");
            return BadRequest(new { Error = ex.Message });
        }
    }

    #endregion
}

#region Request Models

/// <summary>
/// GET test isteği modeli
/// </summary>
public class TestGetRequest
{
    public string Url { get; set; } = string.Empty;
    public string ResponseType { get; set; } = "System.Object";
    public Dictionary<string, string>? Headers { get; set; }
    public int Timeout { get; set; } = 30;
}

/// <summary>
/// POST test isteği modeli
/// </summary>
public class TestPostRequest
{
    public string Url { get; set; } = string.Empty;
    public string RequestType { get; set; } = "System.Object";
    public string ResponseType { get; set; } = "System.Object";
    public JsonElement RequestData { get; set; }
    public string ContentType { get; set; } = "application/json";
    public Dictionary<string, string>? Headers { get; set; }
    public int Timeout { get; set; } = 30;
}

/// <summary>
/// Konfigürasyonlu GET test isteği modeli
/// </summary>
public class TestGetConfigRequest
{
    public string ServiceName { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string ResponseType { get; set; } = "System.Object";
    public Dictionary<string, string>? Headers { get; set; }
    public int? Timeout { get; set; }
}

/// <summary>
/// Konfigürasyonlu POST test isteği modeli
/// </summary>
public class TestPostConfigRequest
{
    public string ServiceName { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string RequestType { get; set; } = "System.Object";
    public string ResponseType { get; set; } = "System.Object";
    public JsonElement RequestData { get; set; }
    public string? ContentType { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    public int? Timeout { get; set; }
}

/// <summary>
/// Konfigürasyon ekleme isteği modeli
/// </summary>
public class AddConfigRequest
{
    public string ServiceName { get; set; } = string.Empty;
    public HttpServiceConfig Config { get; set; } = new();
}

/// <summary>
/// Varsayılan servis ayarlama isteği modeli
/// </summary>
public class SetDefaultServiceRequest
{
    public string ServiceName { get; set; } = string.Empty;
}

#endregion 