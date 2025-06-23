# DynamicERP API

Modern, güvenli ve ölçeklenebilir ERP sistemi API'si.

## 🚀 Özellikler

### 🔐 Güvenlik
- **JWT Token Authentication**
- **Password Hashing (BCrypt)** - Güvenli şifre hash'leme
- **Password Strength Validation** - Güçlü şifre zorunluluğu
- **CORS Protection** - Cross-origin güvenliği
- **Global Exception Handling** - Merkezi hata yönetimi

### 🏗️ Mimari
- **Clean Architecture** - Temiz ve sürdürülebilir kod yapısı
- **CQRS Pattern** - Command Query Responsibility Segregation
- **Repository Pattern** - Veri erişim katmanı
- **Unit of Work** - Transaction yönetimi

### 📊 Veritabanı
- **Entity Framework Core** - Code-First yaklaşımı
- **SQL Server** - Güçlü veritabanı desteği
- **Migration Sistemi** - Veritabanı versiyon yönetimi
- **Multi-Tenant Ready** - Çoklu müşteri desteği

### 🔧 Teknolojiler
- **.NET 9** - En güncel .NET framework
- **MediatR** - Mediator pattern implementasyonu
- **FluentValidation** - Güçlü validation sistemi
- **Mapster** - Hızlı object mapping
- **Swagger/OpenAPI** - API dokümantasyonu

## 🛠️ Kurulum

### Gereksinimler
- .NET 9 SDK
- SQL Server 2019+
- Visual Studio 2022 veya VS Code

### Adımlar
1. Repository'yi klonlayın
2. Connection string'i `appsettings.json`'da güncelleyin
3. Migration'ları çalıştırın: `dotnet ef database update`
4. API'yi başlatın: `dotnet run`

## 🔑 Varsayılan Kullanıcı

```
Email: test@testcompany.com
Şifre: Test123!
```

## 📝 API Endpoints

### Authentication
- `POST /api/auth/login` - Kullanıcı girişi
- `POST /api/auth/refresh-token` - Token yenileme
- `POST /api/auth/revoke-token` - Token iptal etme

### Users
- `GET /api/users` - Tüm kullanıcıları listele
- `GET /api/users/{id}` - Kullanıcı detayı
- `POST /api/users` - Yeni kullanıcı oluştur
- `PUT /api/users/{id}` - Kullanıcı güncelle
- `DELETE /api/users/{id}` - Kullanıcı sil

## 🔒 Güvenlik Özellikleri

### Password Hashing
- BCrypt algoritması kullanılıyor
- Work factor: 12 (güvenlik seviyesi)
- Salt otomatik olarak ekleniyor

### Password Validation
- Minimum 6 karakter
- En az 1 büyük harf
- En az 1 küçük harf
- En az 1 rakam
- En az 1 özel karakter
- Yaygın şifre kontrolü

### JWT Token
- 30 dakika geçerlilik süresi
- Secure token validation
- Token rotation desteği

## 🌍 Ortam Konfigürasyonu

### Development
- Detaylı hata mesajları
- CORS: Tüm origin'lere izin
- Logging: Debug seviyesi

### Production
- Güvenli hata mesajları
- CORS: Sadece belirli domain'lere izin
- Logging: Error seviyesi

## 📚 Dokümantasyon

- [Coding Standards](CODING_STANDARDS.md)
- [RoadMap](ROADMAP.md)

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun
3. Değişikliklerinizi commit edin
4. Pull request gönderin

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. 