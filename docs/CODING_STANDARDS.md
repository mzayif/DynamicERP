# DynamicERP Kodlama Standartları

## 1. Hata Yönetimi

### 1.1. Beklenen Hatalar (Business Rules)
- `Result` sınıfı kullanılmalı
- Örnek durumlar:
  - Kayıt bulunamadı
  - Kayıt zaten var
  - İş kuralı ihlalleri
  - Validasyon hataları
  - Yetkilendirme kontrolleri

### 1.2. Beklenmeyen Hatalar (System Errors)
- `Exception` fırlatılmalı
- Örnek durumlar:
  - Veritabanı bağlantı hataları
  - Dosya sistemi hataları
  - Network hataları
  - Memory hataları
  - Sistem hataları

## 2. Loglama

### 2.1. Otomatik Loglama
- Tüm HTTP request ve response'lar `RequestResponseLoggingMiddleware` tarafından otomatik loglanır
- Manuel loglama yapılmamalı
- Sadece özel durumlarda (örn: kritik işlemler) manuel loglama yapılabilir

## 3. CQRS (Command Query Responsibility Segregation)

### 3.1. Genel Kurallar
- Her servis ve controller CQRS pattern'ine uygun olmalı
- Command'lar veri değiştirir
- Query'ler veri okur
- Her işlem için ayrı command/query handler oluşturulmalı

### 3.2. Handler Dosya Düzeni
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

### 3.3. Command Örnekleri
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

### 3.4. Query Örnekleri
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

## 4. Request/Response Modelleri

### 4.1. Request Modelleri
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

### 4.2. Response Modelleri
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

## 5. API Response Formatı

### 5.1. Başarılı Response
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

### 5.2. Hata Response
```json
{
    "isSuccess": false,
    "errorCode": "USER_NOT_FOUND",
    "message": "Kullanıcı bulunamadı",
    "validationErrors": null
}
```

## 6. Güvenlik

### 6.1. Authentication
- JWT token kullanılmalı
- Token'lar güvenli bir şekilde saklanmalı
- Refresh token mekanizması kullanılmalı

### 6.2. Authorization
- Role-based authorization kullanılmalı
- Her endpoint için gerekli yetkiler belirtilmeli
- Yetkisiz erişimler engellenmeli

## 7. Performans

### 7.1. Veritabanı
- Gereksiz sorgular yapılmamalı
- İndeksler doğru kullanılmalı
- N+1 problemi önlenmeli
- Bulk işlemler için uygun metodlar kullanılmalı

### 7.2. Caching
- Sık kullanılan veriler cache'lenmeli
- Cache invalidation stratejisi belirlenmeli
- Distributed caching kullanılmalı

## 8. Kod Kalitesi

### 8.1. SOLID Prensipleri
- Single Responsibility Principle
- Open/Closed Principle
- Liskov Substitution Principle
- Interface Segregation Principle
- Dependency Inversion Principle

### 8.2. Clean Code
- Anlamlı değişken ve metod isimleri
- DRY (Don't Repeat Yourself) prensibi
- KISS (Keep It Simple, Stupid) prensibi
- Yorum satırları yerine self-documenting code

## 9. Test

### 9.1. Unit Test
- Her servis için unit test yazılmalı
- Mock'lar doğru kullanılmalı
- Test coverage %80'in üzerinde olmalı

### 9.2. Integration Test
- Kritik iş akışları için integration test yazılmalı
- Test veritabanı kullanılmalı
- Testler izole edilmeli

## 10. Dokümantasyon

### 10.1. API Dokümantasyonu
- Swagger kullanılmalı
- Her endpoint için açıklama yazılmalı
- Request/Response örnekleri verilmeli

### 10.2. Kod Dokümantasyonu
- XML comments kullanılmalı
- Karmaşık işlemler için açıklama yazılmalı
- README dosyaları güncel tutulmalı 