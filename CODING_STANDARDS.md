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

### 3.8 Exception Handling
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

### 3.9 Logging
- Merkezi logging yapısı kullanılacak
- Her katmanda uygun log seviyeleri kullanılacak
- Hassas bilgiler loglanmayacak

### 3.10 Security
- JWT authentication kullanılacak
- Role-based authorization uygulanacak
- Password'ler hash'lenerek saklanacak
- API endpoint'leri güvenli olacak

### 3.11 Performance
- Async/await pattern kullanılacak
- Gereksiz database sorgularından kaçınılacak
- Caching mekanizması kullanılacak
- N+1 problemi önlenecek

## 4. Git Standartları
### 4.1 Branch Stratejisi
- main: Production branch
- develop: Development branch
- feature/*: Yeni özellikler için
- bugfix/*: Hata düzeltmeleri için
- release/*: Release hazırlıkları için

### 4.2 Commit Mesajları
- feat: Yeni özellik
- fix: Hata düzeltmesi
- docs: Dokümantasyon değişiklikleri
- style: Kod formatı değişiklikleri
- refactor: Kod refactoring
- test: Test değişiklikleri
- chore: Genel bakım işlemleri

## 5. Dokümantasyon
- XML documentation kullanılacak
- README dosyaları güncel tutulacak
- API dokümantasyonu Swagger ile sağlanacak
- Kod içi yorumlar açıklayıcı olacak 