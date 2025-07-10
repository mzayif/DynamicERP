# HTTP Servis Komple Rehberi

Bu dokümantasyon, DynamicERP uygulamasında kullanılan HTTP servisinin tüm özelliklerini, kullanımını ve konfigürasyonunu kapsamlı bir şekilde açıklar.

## 📋 İçindekiler

1. [Genel Bakış](#genel-bakış)
2. [Özellikler](#özellikler)
3. [Kurulum ve Konfigürasyon](#kurulum-ve-konfigürasyon)
4. [Temel Kullanım](#temel-kullanım)
5. [Konfigürasyonlu Kullanım](#konfigürasyonlu-kullanım)
6. [WCF Servisleri](#wcf-servisleri)
7. [AppSettings Entegrasyonu](#appsettings-entegrasyonu)
8. [Test Endpoint'leri](#test-endpointleri)
9. [Best Practices](#best-practices)
10. [Troubleshooting](#troubleshooting)

## 🎯 Genel Bakış

HTTP servisi, uygulama içerisinden dış servislere yapılabilecek bütün REST ve SOAP isteklerini yönetmek için tasarlanmış generic bir servistir. Bu servis sayesinde:

- ✅ **Tip Güvenliği**: Generic TRequest/TResponse ile tip güvenliği
- ✅ **Çoklu Servis Desteği**: Birden fazla servis konfigürasyonu
- ✅ **REST ve SOAP**: Her iki protokol için uygun
- ✅ **Konfigürasyon Yönetimi**: AppSettings'den otomatik yükleme
- ✅ **Hata Yönetimi**: Detaylı hata bilgileri ve retry politikaları
- ✅ **Performans Ölçümü**: Response time tracking
- ✅ **Logging**: Detaylı loglama seçenekleri

## 🚀 Özellikler

### Temel Özellikler
- **Generic Tip Desteği**: TRequest/TResponse ile tip güvenliği
- **HTTP Metodları**: GET, POST, PUT, DELETE, PATCH, HEAD, OPTIONS
- **Content-Type Desteği**: JSON, XML, Form Data
- **Header Yönetimi**: Esnek header konfigürasyonu
- **Timeout Yönetimi**: Servis bazında timeout ayarları
- **Hata Yönetimi**: Detaylı hata bilgileri ve mesajları

### Gelişmiş Özellikler
- **Retry Policy**: FixedDelay, ExponentialBackoff, Jitter
- **Circuit Breaker**: Hata durumunda otomatik devre kesici
- **Rate Limiting**: İstek hızı sınırlama
- **Caching**: Otomatik cache yönetimi
- **Compression**: Request/Response sıkıştırma
- **Health Check**: Servis sağlık kontrolü
- **File Upload/Download**: Dosya transfer desteği
- **Proxy Support**: Proxy desteği
- **SSL Configuration**: SSL sertifika yönetimi

### Konfigürasyon Özellikleri
- **Çoklu Servis Desteği**: Birden fazla servis konfigürasyonu
- **Base URL Yönetimi**: Her servis için ayrı base URL
- **Otomatik Header Ekleme**: Token, API Key, Basic Auth
- **Varsayılan Değerler**: Timeout, Content-Type
- **Dinamik Konfigürasyon**: Runtime'da ekleme/güncelleme
- **Varsayılan Servis**: Otomatik servis seçimi

## 🔧 Kurulum ve Konfigürasyon

### 1. Service Registration

```csharp
// Program.cs veya Startup.cs
services.AddHttpClient<IHttpService, HttpService>();
services.AddScoped<IHttpService, HttpService>();
services.AddHttpClient<IWcfService, WcfService>();
services.AddScoped<IWcfService, WcfService>();
```

### 2. Options Pattern Konfigürasyonu

#### ServiceRegistration.cs
```csharp
public static IServiceCollection AddDynamicErpServices(this IServiceCollection services, IConfiguration configuration)
{
    // ... diğer servisler ...

    // HTTP servis konfigürasyonları için Options Pattern
    services.Configure<HttpServiceOptions>(configuration.GetSection("HttpServiceConfigurations"));
    
    // HTTP servisi ekle
    services.AddHttpClient<IHttpService, HttpService>();
    services.AddScoped<IHttpService, HttpService>();

    return services;
}
```

#### HttpService.cs
```csharp
public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly ILoggerService _logger;
    private readonly Dictionary<string, HttpServiceConfig> _serviceConfigurations;
    private string _defaultService = string.Empty;

    public HttpService(
        HttpClient httpClient, 
        ILoggerService logger,
        IOptions<HttpServiceOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _serviceConfigurations = new Dictionary<string, HttpServiceConfig>();

        // Options Pattern ile konfigürasyonları yükle
        LoadConfigurations(options.Value);
    }

    private void LoadConfigurations(HttpServiceOptions options)
    {
        try
        {
            _logger.LogInformation("HTTP servis konfigürasyonları yükleniyor...");

            // Her servis konfigürasyonunu ekle
            foreach (var serviceConfig in options.Services)
            {
                _serviceConfigurations[serviceConfig.Key] = serviceConfig.Value;
                _logger.LogInformation($"Servis konfigürasyonu yüklendi: {serviceConfig.Key} - {serviceConfig.Value.BaseUrl}");
            }

            // Varsayılan servisi ayarla
            if (!string.IsNullOrEmpty(options.DefaultService))
            {
                _defaultService = options.DefaultService;
                _logger.LogInformation($"Varsayılan servis ayarlandı: {options.DefaultService}");
            }

            _logger.LogInformation($"HTTP servis konfigürasyonları başarıyla yüklendi. Toplam {options.Services.Count} servis.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP servis konfigürasyonları yüklenirken hata");
        }
    }

    // ... diğer metodlar ...
}
```

### 2. AppSettings Konfigürasyonu

#### appsettings.json (Production)
```json
{
  "HttpServiceConfigurations": {
    "DefaultService": "PaymentAPI",
    "Services": {
      "PaymentAPI": {
        "ServiceName": "PaymentAPI",
        "BaseUrl": "https://api.payment.com",
        "DefaultTimeout": 30,
        "DefaultContentType": "application/json",
        "AuthToken": "Bearer payment-token-123",
        "DefaultHeaders": {
          "Accept": "application/json",
          "User-Agent": "ERP-System/1.0",
          "X-Client-ID": "ERP001"
        },
        "RetryCount": 3,
        "RetryDelayMs": 1000,
        "SkipSslValidation": false,
        "UseProxy": false
      },
      "ECommerceAPI": {
        "ServiceName": "ECommerceAPI",
        "BaseUrl": "https://api.ecommerce.com",
        "DefaultTimeout": 45,
        "DefaultContentType": "application/json",
        "ApiKey": "ecommerce-api-key-456",
        "Username": "ecommerce_user",
        "Password": "ecommerce_pass_789",
        "DefaultHeaders": {
          "Accept": "application/json",
          "User-Agent": "ERP-System/1.0",
          "X-Client-Version": "1.0.0",
          "X-Request-Source": "ERP-System"
        },
        "RetryCount": 2,
        "RetryDelayMs": 2000,
        "SkipSslValidation": false,
        "UseProxy": false
      }
    }
  }
}
```

#### appsettings.Development.json (Development)
```json
{
  "HttpServiceConfigurations": {
    "DefaultService": "PaymentAPI",
    "Services": {
      "PaymentAPI": {
        "ServiceName": "PaymentAPI",
        "BaseUrl": "https://dev-api.payment.com",
        "DefaultTimeout": 30,
        "DefaultContentType": "application/json",
        "AuthToken": "Bearer dev-payment-token-123",
        "DefaultHeaders": {
          "Accept": "application/json",
          "User-Agent": "ERP-System/1.0-Dev",
          "X-Client-ID": "ERP001",
          "X-Environment": "Development"
        },
        "RetryCount": 3,
        "RetryDelayMs": 1000,
        "SkipSslValidation": true,
        "UseProxy": false
      },
      "ECommerceAPI": {
        "ServiceName": "ECommerceAPI",
        "BaseUrl": "https://dev-api.ecommerce.com",
        "DefaultTimeout": 45,
        "DefaultContentType": "application/json",
        "ApiKey": "dev-ecommerce-api-key-456",
        "Username": "dev_ecommerce_user",
        "Password": "dev_ecommerce_pass_789",
        "DefaultHeaders": {
          "Accept": "application/json",
          "User-Agent": "ERP-System/1.0-Dev",
          "X-Client-Version": "1.0.0",
          "X-Request-Source": "ERP-System",
          "X-Environment": "Development"
        },
        "RetryCount": 2,
        "RetryDelayMs": 2000,
        "SkipSslValidation": true,
        "UseProxy": false
      }
    }
  }
}
```

## 📖 Temel Kullanım

### 1. GET İsteği

```csharp
// Basit GET isteği
var response = await _httpService.GetAsync<User>("https://api.example.com/users/1");

if (response.IsSuccessStatusCode && response.Data != null)
{
    var user = response.Data;
    Console.WriteLine($"Kullanıcı: {user.Name}");
}
```

### 2. POST İsteği

```csharp
// POST isteği ile veri gönderme
var createUserRequest = new CreateUserRequest 
{ 
    Name = "John Doe", 
    Email = "john@example.com" 
};

var response = await _httpService.PostAsync<CreateUserRequest, User>(
    "https://api.example.com/users", 
    createUserRequest
);

if (response.IsSuccessStatusCode && response.Data != null)
{
    var createdUser = response.Data;
    Console.WriteLine($"Oluşturulan kullanıcı ID: {createdUser.Id}");
}
```

### 3. PUT İsteği

```csharp
// PUT isteği ile veri güncelleme
var updateUserRequest = new UpdateUserRequest 
{ 
    Name = "John Updated", 
    Email = "john.updated@example.com" 
};

var response = await _httpService.PutAsync<UpdateUserRequest, User>(
    "https://api.example.com/users/1", 
    updateUserRequest
);
```

### 4. DELETE İsteği

```csharp
// DELETE isteği
var response = await _httpService.DeleteAsync<object>("https://api.example.com/users/1");

if (response.IsSuccessStatusCode)
{
    Console.WriteLine("Kullanıcı başarıyla silindi");
}
```

### 5. SOAP İsteği

```csharp
// SOAP isteği
var soapRequest = new GetUserRequest { UserId = 1 };
var response = await _httpService.SendSoapRequestAsync<GetUserRequest, GetUserResponse>(
    "https://api.example.com/soap",
    "GetUser",
    soapRequest
);
```

### 6. Hata Yönetimi

```csharp
try
{
    var response = await _httpService.GetAsync<User>("https://api.example.com/users/1");
    
    if (response.IsSuccessStatusCode)
    {
        if (response.HasDeserializationError)
        {
            // Deserializasyon hatası
            _logger.LogWarning($"Deserializasyon hatası: {response.DeserializationErrorMessage}");
            // Ham içeriği kullan
            var rawData = response.RawContent;
        }
        else if (response.Data != null)
        {
            // Başarılı
            var user = response.Data;
        }
    }
    else
    {
        // HTTP hatası
        _logger.LogError($"HTTP Hatası: {response.StatusCode} - {response.ErrorMessage}");
    }
}
catch (Exception ex)
{
    // Genel hata
    _logger.LogError($"İstek hatası: {ex.Message}");
}
```

## ⚙️ Konfigürasyonlu Kullanım

### 1. Servis Konfigürasyonu Ekleme

```csharp
var config = new HttpServiceConfig
{
    ServiceName = "PaymentAPI",
    BaseUrl = "https://api.payment.com",
    DefaultTimeout = 30,
    DefaultContentType = "application/json",
    AuthToken = "Bearer token123",
    RetryCount = 3,
    RetryPolicyType = RetryPolicyType.ExponentialBackoff,
    CircuitBreakerFailureThreshold = 5,
    RateLimitPerMinute = 100,
    UseCaching = true,
    CacheDurationSeconds = 300,
    LogLevel = LogLevel.Information,
    HealthCheckEndpoint = "/health"
};

_httpService.AddServiceConfiguration("PaymentAPI", config);
```

### 2. Konfigürasyonlu İstek Gönderme

```csharp
// GET isteği
var response = await _httpService.GetAsync<User>("PaymentAPI", "/users/1");

// POST isteği
var createUserRequest = new CreateUserRequest { Name = "John", Email = "john@example.com" };
var response = await _httpService.PostAsync<CreateUserRequest, User>("PaymentAPI", "/users", createUserRequest);

// Dosya upload
var response = await _httpService.UploadFileAsync<UploadResponse>("PaymentAPI", "/upload", "C:\\file.pdf");

// Dosya download
var response = await _httpService.DownloadFileAsync("PaymentAPI", "/download/file.pdf");
```

### 3. Health Check

```csharp
// Servis sağlığını kontrol et
var isHealthy = await _httpService.HealthCheckAsync("PaymentAPI");

// Detaylı sağlık durumu
var healthStatus = await _httpService.GetServiceHealthAsync("PaymentAPI");
Console.WriteLine($"Servis: {healthStatus.ServiceName}");
Console.WriteLine($"Sağlıklı: {healthStatus.IsHealthy}");
Console.WriteLine($"Yanıt Süresi: {healthStatus.ResponseTimeMs}ms");
Console.WriteLine($"Başarı Oranı: {healthStatus.SuccessRate}%");
```

### 4. Circuit Breaker Yönetimi

```csharp
// Circuit breaker durumunu al
var state = _httpService.GetCircuitBreakerState("PaymentAPI");
Console.WriteLine($"Circuit Breaker Durumu: {state}");

// Manuel olarak aç/kapat
_httpService.SetCircuitBreakerState("PaymentAPI", CircuitBreakerState.Open);
```

### 5. Rate Limit Bilgisi

```csharp
var rateLimitInfo = _httpService.GetRateLimitInfo("PaymentAPI");
Console.WriteLine($"Limit: {rateLimitInfo.LimitPerMinute}/dakika");
Console.WriteLine($"Mevcut: {rateLimitInfo.CurrentRequests}");
Console.WriteLine($"Kalan: {rateLimitInfo.RemainingRequests}");
Console.WriteLine($"Aşıldı: {rateLimitInfo.IsExceeded}");
```

### 6. Cache Yönetimi

```csharp
// Cache'i temizle
_httpService.ClearCache("PaymentAPI");
```

## 🔌 WCF Servisleri

### 1. WCF Servis Konfigürasyonu

```csharp
// WCF servisi için konfigürasyon
var wcfConfig = new HttpServiceConfig
{
    ServiceName = "WcfService",
    BaseUrl = "https://wcf.example.com",
    DefaultTimeout = 60,
    DefaultContentType = "text/xml; charset=utf-8"
};

_httpService.AddServiceConfiguration("WcfService", wcfConfig);
```

### 2. WCF İstekleri

```csharp
// BasicHttpBinding ile WCF isteği
var request = new GetUserRequest { UserId = 1 };
var response = await _wcfService.SendWcfRequestAsync<GetUserRequest, GetUserResponse>(
    "https://wcf.example.com/UserService.svc",
    "GetUser",
    request,
    WcfBindingType.BasicHttp
);

// WSHttpBinding ile güvenli WCF isteği
var response = await _wcfService.SendWcfSecureRequestAsync<GetUserRequest, GetUserResponse>(
    "https://wcf.example.com/UserService.svc",
    "GetUser",
    request,
    "username",
    "password"
);

// NetTcpBinding ile TCP isteği
var response = await _wcfService.SendWcfTcpRequestAsync<GetUserRequest, GetUserResponse>(
    "net.tcp://wcf.example.com:808/UserService",
    "GetUser",
    request
);
```

### 3. WSDL Dokümantasyonu

```csharp
// WSDL dokümantasyonunu al
var wsdl = await _wcfService.GetWcfWsdlAsync("https://wcf.example.com/UserService.svc");
Console.WriteLine($"WSDL: {wsdl}");

// Endpoint'leri keşfet
var endpoints = await _wcfService.DiscoverWcfEndpointsAsync("https://wcf.example.com/UserService.svc");
foreach (var endpoint in endpoints)
{
    Console.WriteLine($"Endpoint: {endpoint}");
}
```

## 📋 AppSettings Entegrasyonu

### 1. Options Pattern ile Otomatik Yükleme

Uygulama başlangıcında `appsettings.json` dosyasından konfigürasyonlar Options Pattern ile otomatik olarak yüklenir:

```csharp
// ServiceRegistration.cs
services.Configure<HttpServiceOptions>(configuration.GetSection("HttpServiceConfigurations"));
```

### 2. Constructor'da Yükleme

```csharp
// HttpService.cs
public HttpService(
    HttpClient httpClient, 
    ILoggerService logger,
    IOptions<HttpServiceOptions> options)
{
    _httpClient = httpClient;
    _logger = logger;
    _serviceConfigurations = new Dictionary<string, HttpServiceConfig>();

    // Options Pattern ile konfigürasyonları yükle
    LoadConfigurations(options.Value);
}

private void LoadConfigurations(HttpServiceOptions options)
{
    try
    {
        _logger.LogInformation("HTTP servis konfigürasyonları yükleniyor...");

        // Her servis konfigürasyonunu ekle
        foreach (var serviceConfig in options.Services)
        {
            _serviceConfigurations[serviceConfig.Key] = serviceConfig.Value;
            _logger.LogInformation($"Servis konfigürasyonu yüklendi: {serviceConfig.Key} - {serviceConfig.Value.BaseUrl}");
        }

        // Varsayılan servisi ayarla
        if (!string.IsNullOrEmpty(options.DefaultService))
        {
            _defaultService = options.DefaultService;
            _logger.LogInformation($"Varsayılan servis ayarlandı: {options.DefaultService}");
        }

        _logger.LogInformation($"HTTP servis konfigürasyonları başarıyla yüklendi. Toplam {options.Services.Count} servis.");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "HTTP servis konfigürasyonları yüklenirken hata");
    }
}
```

## 🧪 Test Endpoint'leri

### 1. Temel Test Endpoint'leri

#### GET Test
```http
POST /api/HttpService/test-get
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "url": "https://jsonplaceholder.typicode.com/users/1",
    "responseType": "YourNamespace.User",
    "headers": {
        "Accept": "application/json"
    },
    "timeout": 30
}
```

#### POST Test
```http
POST /api/HttpService/test-post
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "url": "https://jsonplaceholder.typicode.com/users",
    "requestType": "YourNamespace.CreateUserRequest",
    "responseType": "YourNamespace.CreateUserResponse",
    "requestData": {
        "name": "John Doe",
        "email": "john@example.com"
    },
    "contentType": "application/json",
    "headers": {
        "Accept": "application/json"
    },
    "timeout": 30
}
```

### 2. Konfigürasyonlu Test Endpoint'leri

#### Konfigürasyonlu GET Test
```http
POST /api/HttpService/test-get-config
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "serviceName": "PaymentAPI",
    "endpoint": "/users/1",
    "responseType": "YourNamespace.User",
    "headers": {
        "Accept": "application/json"
    },
    "timeout": 30
}
```

#### Konfigürasyonlu POST Test
```http
POST /api/HttpService/test-post-config
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "serviceName": "PaymentAPI",
    "endpoint": "/users",
    "requestType": "YourNamespace.CreateUserRequest",
    "responseType": "YourNamespace.CreateUserResponse",
    "requestData": {
        "name": "John Doe",
        "email": "john@example.com"
    },
    "contentType": "application/json",
    "headers": {
        "Accept": "application/json"
    },
    "timeout": 30
}
```

### 3. AppSettings Test Endpoint'leri

#### Tüm Konfigürasyonları Test Et
```http
GET /api/HttpService/test-appsettings-config
Authorization: Bearer {your-jwt-token}
```

**Beklenen Yanıt:**
```json
{
  "message": "AppSettings'den yüklenen konfigürasyonlar",
  "defaultService": "PaymentAPI",
  "totalServices": 2,
  "services": [
    {
      "serviceName": "PaymentAPI",
      "baseUrl": "https://dev-api.payment.com",
      "timeout": 30,
      "contentType": "application/json",
      "hasAuthToken": true,
      "hasApiKey": false,
      "hasCredentials": false,
      "retryCount": 3,
      "retryDelay": 1000
    },
    {
      "serviceName": "ECommerceAPI",
      "baseUrl": "https://dev-api.ecommerce.com",
      "timeout": 45,
      "contentType": "application/json",
      "hasAuthToken": false,
      "hasApiKey": true,
      "hasCredentials": true,
      "retryCount": 2,
      "retryDelay": 2000
    }
  ]
}
```

#### PaymentAPI Konfigürasyonunu Test Et
```http
GET /api/HttpService/test-payment-api
Authorization: Bearer {your-jwt-token}
```

#### ECommerceAPI Konfigürasyonunu Test Et
```http
GET /api/HttpService/test-ecommerce-api
Authorization: Bearer {your-jwt-token}
```

### 4. Konfigürasyon Yönetimi Endpoint'leri

#### Konfigürasyon Ekleme
```http
POST /api/HttpService/add-config
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "serviceName": "NewAPI",
    "config": {
        "serviceName": "NewAPI",
        "baseUrl": "https://api.new.com",
        "defaultTimeout": 30,
        "defaultContentType": "application/json",
        "authToken": "Bearer new-token",
        "defaultHeaders": {
            "Accept": "application/json",
            "User-Agent": "ERP-System/1.0"
        },
        "retryCount": 3,
        "retryDelayMs": 1000
    }
}
```

#### Konfigürasyon Güncelleme
```http
PUT /api/HttpService/update-config
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "serviceName": "PaymentAPI",
    "config": {
        "serviceName": "PaymentAPI",
        "baseUrl": "https://new-api.payment.com",
        "defaultTimeout": 60,
        "authToken": "new-token-456"
    }
}
```

#### Konfigürasyon Kaldırma
```http
DELETE /api/HttpService/remove-config/NewAPI
Authorization: Bearer {your-jwt-token}
```

#### Konfigürasyon Alma
```http
GET /api/HttpService/get-config/PaymentAPI
Authorization: Bearer {your-jwt-token}
```

#### Tüm Konfigürasyonları Listele
```http
GET /api/HttpService/get-all-configs
Authorization: Bearer {your-jwt-token}
```

#### Varsayılan Servis Ayarlama
```http
POST /api/HttpService/set-default-service
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "serviceName": "PaymentAPI"
}
```

#### Varsayılan Servis Alma
```http
GET /api/HttpService/get-default-service
Authorization: Bearer {your-jwt-token}
```

### 5. WCF Test Endpoint'leri

#### WCF İstek Test
```http
POST /api/WcfService/test-wcf-request
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "url": "https://wcf.example.com/UserService.svc",
    "soapAction": "GetUser",
    "requestType": "YourNamespace.GetUserRequest",
    "responseType": "YourNamespace.GetUserResponse",
    "requestData": {
        "userId": 1
    },
    "bindingType": "BasicHttp",
    "headers": {
        "Accept": "text/xml"
    },
    "timeout": 30
}
```

#### WSDL Alma Test
```http
POST /api/WcfService/test-get-wsdl
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "url": "https://wcf.example.com/UserService.svc",
    "timeout": 30
}
```

#### Endpoint Keşif Test
```http
POST /api/WcfService/test-discover-endpoints
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
    "url": "https://wcf.example.com/UserService.svc",
    "timeout": 30
}
```

## 🎯 Best Practices

### 1. Retry Politikası Seçimi
- **FixedDelay**: Basit senaryolar için
- **ExponentialBackoff**: Ağır yük altındaki servisler için
- **ExponentialBackoffWithJitter**: Yüksek eşzamanlılık için

### 2. Circuit Breaker Ayarları
- **FailureThreshold**: 5-10 arası (çok düşük olmasın)
- **OpenDuration**: 30-60 saniye (çok uzun olmasın)
- **SuccessThreshold**: 2-3 arası

### 3. Rate Limiting
- API limitlerine göre ayarlayın
- Burst trafiği için buffer bırakın
- Monitoring ile takip edin

### 4. Caching
- GET istekleri için kullanın
- Süreleri servis özelliklerine göre ayarlayın
- Cache invalidation stratejisi belirleyin

### 5. Logging
- Production'da LogRequestBody/LogResponseBody = false
- PII verilerini loglamayın
- Log seviyesini ortama göre ayarlayın

### 6. Health Check
- Kritik servisler için health check endpoint'i tanımlayın
- Düzenli aralıklarla kontrol edin
- Alerting sistemi kurun

### 7. Security
- Hassas bilgileri environment variable'larda saklayın
- SSL sertifika doğrulamasını production'da açık tutun
- API key'leri düzenli olarak rotate edin

### 8. Error Handling
- Her zaman response kontrolü yapın
- Deserializasyon hatalarını handle edin
- Timeout değerlerini servis özelliklerine göre ayarlayın

## 🔧 Troubleshooting

### 1. Konfigürasyon Yükleme Sorunları

**Problem**: Konfigürasyonlar yüklenmiyor
```bash
# Log'ları kontrol edin
tail -f logs/application.log | grep "HTTP servis konfigürasyonları"

# Beklenen log çıktısı:
# info: HTTP servis konfigürasyonları yükleniyor...
# info: Servis konfigürasyonu eklendi: PaymentAPI - https://dev-api.payment.com
# info: Varsayılan servis ayarlandı: PaymentAPI
```

**Çözüm**:
- `appsettings.json` dosyasının doğru formatta olduğunu kontrol edin
- JSON syntax hatalarını düzeltin
- Environment variable'ların doğru set edildiğini kontrol edin

### 2. SSL Sertifika Sorunları

**Problem**: SSL sertifika doğrulama hatası
```json
{
  "error": "The SSL connection could not be established"
}
```

**Çözüm**:
```json
{
  "SkipSslValidation": true  // Development ortamında
}
```

### 3. Timeout Sorunları

**Problem**: İstekler timeout oluyor
```json
{
  "error": "The operation has timed out"
}
```

**Çözüm**:
```json
{
  "DefaultTimeout": 60  // Timeout süresini artırın
}
```

### 4. Authentication Sorunları

**Problem**: 401 Unauthorized hatası
```json
{
  "error": "Unauthorized"
}
```

**Çözüm**:
- Token'ın geçerli olduğunu kontrol edin
- API key'in doğru olduğunu kontrol edin
- Username/password'ün doğru olduğunu kontrol edin

### 5. Circuit Breaker Sorunları

**Problem**: Circuit breaker sürekli açık kalıyor
```json
{
  "circuitBreakerState": "Open"
}
```

**Çözüm**:
```json
{
  "CircuitBreakerOpenDurationSeconds": 30,  // Süreyi azaltın
  "CircuitBreakerSuccessThreshold": 1       // Eşiği düşürün
}
```

### 6. Rate Limit Sorunları

**Problem**: Rate limit aşıldı
```json
{
  "error": "Rate limit exceeded"
}
```

**Çözüm**:
```json
{
  "RateLimitPerMinute": 200  // Limiti artırın
}
```

## 📊 Monitoring ve Alerting

### 1. Metrikler
- Response time
- Success rate
- Circuit breaker durumu
- Rate limit kullanımı
- Cache hit/miss oranı

### 2. Alerting
- Circuit breaker açıldığında
- Success rate düştüğünde
- Response time arttığında
- Rate limit aşıldığında

### 3. Dashboard
- Servis sağlık durumu
- Performans metrikleri
- Hata oranları
- Kullanım istatistikleri

## 🔄 Konfigürasyon Güncelleme

Runtime'da konfigürasyon güncellemek için:

```http
PUT /api/HttpService/update-config
Authorization: Bearer {your-jwt-token}
Content-Type: application/json

{
  "serviceName": "PaymentAPI",
  "config": {
    "serviceName": "PaymentAPI",
    "baseUrl": "https://new-api.payment.com",
    "defaultTimeout": 60,
    "authToken": "new-token-456"
  }
}
```

Bu komple rehber sayesinde HTTP servisinin tüm özelliklerini etkin bir şekilde kullanabilirsiniz! 🎉 