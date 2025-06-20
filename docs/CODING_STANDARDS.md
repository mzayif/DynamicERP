# DynamicERP Coding Standards

## 1. Proje Yapısı
- Clean Architecture prensipleri kullanılacak
- Katmanlar arası bağımlılıklar içten dışa doğru olacak
- Her katman kendi sorumluluğunu yerine getirecek

## 2. Katmanlar
### 2.1 Core Katmanı
- Domain modelleri
- Interface'ler
- Enum'lar
- Sabitler
- Yardımcı sınıflar
- Result yapıları
- Request/Response modelleri

### 2.2 Application Katmanı
- CQRS pattern kullanımı
- MediatR ile command/query işlemleri
- Service implementasyonları
- Validation kuralları
- Mapping konfigürasyonları

### 2.3 Infrastructure Katmanı
- Repository implementasyonları
- DbContext
- Migration'lar
- Harici servis entegrasyonları

### 2.4 API Katmanı
- Controller'lar
- Middleware'ler
- API konfigürasyonları
- Swagger dökümantasyonu

## 3. Kod Standartları
### 3.1 Genel Kurallar
- Türkçe karakter kullanılmayacak
- Anlamlı isimlendirmeler yapılacak
- SOLID prensipleri uygulanacak
- DRY (Don't Repeat Yourself) prensibi uygulanacak
- KISS (Keep It Simple, Stupid) prensibi uygulanacak

### 3.2 Naming Conventions
- Interface'ler "I" prefix'i ile başlayacak
- Abstract sınıflar "Base" prefix'i ile başlayacak
- Enum'lar tekil olacak
- Property'ler PascalCase olacak
- Method'lar PascalCase olacak
- Private field'lar "_camelCase" olacak
- Local değişkenler camelCase olacak

### 3.3 CQRS Pattern
- Command'lar için:
  - `Create{Entity}Command`
  - `Update{Entity}Command`
  - `Delete{Entity}Command`
- Query'ler için:
  - `Get{Entity}ByIdQuery`
  - `Get{Entity}By{Property}Query`
  - `GetAll{Entity}Query`
- Handler'lar için:
  - `{Command/Query}Handler`

#### 3.3.1 CQRS Genel Kurallar
- Her servis ve controller CQRS pattern'ine uygun olmalı
- Command'lar veri değiştirir
- Query'ler veri okur
- Her işlem için ayrı command/query handler oluşturulmalı

#### 3.3.2 Handler Dosya Düzeni
- Her CQRS işlemi için tek bir dosya oluşturulmalı
- Dosya ismi: `[IslemAdi]Handler.cs` (ör: `CreateUserHandler.cs`)
- Dosya içeriği sırası:
  1. Request ve/veya Response class'ı (ör: `CreateUserRequest`, `CreateUserResponse`)
  2. (Varsa) Validator class'ı (ör: `CreateUserRequestValidator`)
  3. Handler class'ı (ör: `CreateUserHandler`)

Örnek dosya yapısı:
```csharp
// CreateUserHandler.cs

// 1. Request
public class CreateUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    // ...
}

// 2. Validator
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).MinimumLength(6);
        // ...
    }
}

// 3. Handler
public class CreateUserHandler : ICommandHandler<CreateUserRequest, Result>
{
    private readonly IUserRepository _userRepository;
    
    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        // İşlem mantığı
    }
}
```

#### 3.3.3 Command Örnekleri
```csharp
// Create Command - Sadece Result döner
public class CreateUserCommand : ICommand<Result>
{
    public string Email { get; set; }
    public string Password { get; set; }
    // ...
}

// Update Command - Sadece Result döner
public class UpdateUserCommand : ICommand<Result>
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    // ...
}

// Delete Command - Sadece Result döner
public class DeleteUserCommand : ICommand<Result>
{
    public Guid Id { get; set; }
}

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result>
{
    // ...
}
```

#### 3.3.4 Query Örnekleri
```csharp
// Query'ler data döner
public class GetUserByIdQuery : IQuery<Result<UserDto>>
{
    public Guid Id { get; set; }
}

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Result<UserDto>>
{
    // ...
}
```

### 3.4 Validation
- FluentValidation kullanılacak
- Her request için ayrı validator sınıfı oluşturulacak
- Validation kuralları merkezi olarak yönetilecek

### 3.5 Mapping
- Mapster kullanılacak
- Property isimleri aynı olduğu sürece otomatik mapping yapılacak
- Özel mapping gerektiren durumlar için TypeAdapterConfig kullanılacak

### 3.6 Result Yapısı
- Başarılı işlemler için:
  - `Result.Success()`
  - `DataResult<T>.Success(data)`
- Başarısız işlemler için:
  - `Result.Failure(message)`
  - `DataResult<T>.Failure(message)`

### 3.7 Merkezi Mesaj Yapısı
- Tüm mesajlar `MessageCodes` sınıfında tanımlanacak
- Mesaj kodları kategorilere ayrılacak:
  - Common: Genel mesajlar
  - Auth: Kimlik doğrulama mesajları
  - User: Kullanıcı ile ilgili mesajlar
  - Validation: Doğrulama mesajları
- Mesajlar çoklu dil desteği ile kullanılacak
- Mesaj kullanımı:
  ```csharp
  Messages.GetMessage(MessageCodes.Common.NotFound, "Kullanıcı")
  Messages.GetMessage(MessageCodes.Auth.InvalidCredentials)
  ```

## 4. Hata Yönetimi

### 4.1. Beklenen Hatalar (Business Rules)
- `Result` sınıfı kullanılmalı
- Örnek durumlar:
  - Kayıt bulunamadı
  - Kayıt zaten var
  - İş kuralı ihlalleri
  - Validasyon hataları
  - Yetkilendirme kontrolleri

### 4.2. Beklenmeyen Hatalar (System Errors)
- `Exception` fırlatılmalı
- Örnek durumlar:
  - Veritabanı bağlantı hataları
  - Dosya sistemi hataları
  - Network hataları
  - Memory hataları
  - Sistem hataları

### 4.3 Exception Handling
- Custom exception'lar kullanılacak
- Exception'lar merkezi olarak yakalanacak
- Kullanıcıya anlamlı hata mesajları dönülecek
- Exception fırlatma kuralları:
  - Domain katmanında exception fırlatılabilir
  - Application katmanında exception fırlatılmamalı, Result dönülmeli
  - Infrastructure katmanında sadece teknik hatalarda exception fırlatılmalı
  - API katmanında exception'lar middleware'de yakalanmalı
- Exception hiyerarşisi:
  ```csharp
  public class DynamicERPException : Exception
  public class NotFoundException : DynamicERPException
  public class ValidationException : DynamicERPException
  public class BusinessException : DynamicERPException
  public class TechnicalException : DynamicERPException
  ```

## 5. Request/Response Modelleri

### 5.1. Request Modelleri
- Birden fazla parametre gerektiren işlemler için `...Request` modeli oluşturulmalı
- Request modelleri validation attribute'ları içermeli
- Örnek:
```csharp
public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}
```

### 5.2. Response Modelleri
- Entity'ler asla direkt olarak dönülmemeli
- Her response için `...Response` veya `...Dto` modeli oluşturulmalı
- Query işlemlerinde response modelleri `DataResponse<T>` içinde dönülmeli
- CUD (Create, Update, Delete) işlemlerinde sadece `Result` dönülmeli, data dönülmemeli
- Örnek:
```csharp
// Query Response
public class UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    // ...
}

// CUD işlemleri için sadece Result dönülür
public class CreateUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    // ...
}
```

## 6. API Response Formatı

### 6.1. Başarılı Response
```json
// Query Response
{
    "isSuccess": true,
    "data": {
        "id": "guid",
        "email": "user@example.com",
        "fullName": "John Doe"
    },
    "message": "İşlem başarılı"
}

// CUD Response
{
    "isSuccess": true,
    "message": "İşlem başarılı"
}
```

### 6.2. Hata Response
```json
{
    "isSuccess": false,
    "errorCode": "USER_NOT_FOUND",
    "message": "Kullanıcı bulunamadı",
    "validationErrors": null
}
```

## 7. Loglama

### 7.1. Otomatik Loglama
- Tüm HTTP request ve response'lar `RequestResponseLoggingMiddleware` tarafından otomatik loglanır
- Manuel loglama yapılmamalı
- Sadece özel durumlarda (örn: kritik işlemler) manuel loglama yapılabilir

### 7.2. Genel Loglama Kuralları
- Merkezi logging yapısı kullanılacak
- Her katmanda uygun log seviyeleri kullanılacak
- Hassas bilgiler loglanmayacak

## 8. Güvenlik

### 8.1. Authentication
- JWT token kullanılmalı
- Token'lar güvenli bir şekilde saklanmalı
- Refresh token mekanizması kullanılmalı

### 8.2. Authorization
- Role-based authorization kullanılmalı
- Her endpoint için gerekli yetkiler belirtilmeli
- Yetkisiz erişimler engellenmeli

### 8.3. Genel Güvenlik Kuralları
- Password'ler hash'lenerek saklanacak
- API endpoint'leri güvenli olacak

## 9. Performans

### 9.1. Veritabanı
- Gereksiz sorgular yapılmamalı
- İndeksler doğru kullanılmalı
- N+1 problemi önlenmeli
- Bulk işlemler için uygun metodlar kullanılmalı

### 9.2. Caching
- Sık kullanılan veriler cache'lenmeli
- Cache invalidation stratejisi belirlenmeli
- Distributed caching kullanılmalı

### 9.3. Genel Performans Kuralları
- Async/await pattern kullanılacak
- Gereksiz database sorgularından kaçınılacak
- Caching mekanizması kullanılacak

## 10. Kod Kalitesi

### 10.1. SOLID Prensipleri
- Single Responsibility Principle
- Open/Closed Principle
- Liskov Substitution Principle
- Interface Segregation Principle
- Dependency Inversion Principle

### 10.2. Clean Code
- Anlamlı değişken ve metod isimleri
- DRY (Don't Repeat Yourself) prensibi
- KISS (Keep It Simple, Stupid) prensibi
- Yorum satırları yerine self-documenting code

## 11. Test

### 11.1. Unit Test
- Her servis için unit test yazılmalı
- Mock'lar doğru kullanılmalı
- Test coverage %80'in üzerinde olmalı

### 11.2. Integration Test
- Kritik iş akışları için integration test yazılmalı
- Test veritabanı kullanılmalı
- Testler izole edilmeli

## 12. Git Standartları
### 12.1 Branch Stratejisi
- main: Production branch
- develop: Development branch
- feature/*: Yeni özellikler için
- bugfix/*: Hata düzeltmeleri için
- release/*: Release hazırlıkları için

### 12.2 Commit Mesajları
- feat: Yeni özellik
- fix: Hata düzeltmesi
- docs: Dokümantasyon değişiklikleri
- style: Kod formatı değişiklikleri
- refactor: Kod refactoring
- test: Test değişiklikleri
- chore: Genel bakım işlemleri

## 13. Dokümantasyon

### 13.1. API Dokümantasyonu
- Swagger kullanılmalı
- Her endpoint için açıklama yazılmalı
- Request/Response örnekleri verilmeli

### 13.2. Kod Dokümantasyonu
- XML comments kullanılmalı
- Karmaşık işlemler için açıklama yazılmalı
- README dosyaları güncel tutulmalı
- Kod içi yorumlar açıklayıcı olacak 