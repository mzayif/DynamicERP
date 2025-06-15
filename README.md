# DynamicERP Projesi

## Proje Yapısı

### 1. Entity Yapısı
- **BaseEntity**: Temel entity özellikleri
  - Id (Guid)
  - CreatedDate
  - CreatedBy
  - UpdatedDate
  - UpdatedBy

- **BaseFullEntity**: BaseEntity'den türeyen ve IsActive özelliği eklenmiş entity
  - Tüm BaseEntity özellikleri
  - IsActive (bool)

- **Entity'ler**:
  - User
  - Tenant
  - ExternalProvider

### 2. Repository Pattern
- **IGenericRepository<TEntity, TKey>**
  - GetAllAsync()
  - GetByIdAsync()
  - AddAsync()
  - UpdateAsync()
  - DeleteAsync()
  - ExistsAsync()
  - FindAsync()
  - FirstOrDefaultAsync()
  - CountAsync()
  - ActivateAsync()
  - CancelAsync()

- **Özel Repository'ler**:
  - IUserRepository
  - ITenantRepository
  - IExternalProviderRepository

### 3. Service Layer
- **Servis Interface'leri**:
  - IUserService
  - ITenantService
  - IExternalProviderService

- **Servis Implementasyonları**:
  - UserService
  - TenantService
  - ExternalProviderService

### 4. Entity Configuration
- **Configuration Sınıfları**:
  - UserConfiguration
  - TenantConfiguration
  - ExternalProviderConfiguration

- **Validation Kuralları**:
  - ValidationRules.cs ile merkezi validation
  - Entity bazlı özel kurallar

### 5. Dependency Injection
- Repository ve Service'ler için DI kayıtları
- Scoped lifetime kullanımı
- Program.cs'de servis kayıtları

### 6. Validation Rules
- Merkezi validation kuralları
- Entity bazlı özel kurallar
- Configuration'larda kullanım

### 7. CQRS Pattern
- **Command Interface'leri**:
  - ICommand
  - ICommand<TResult>

- **Query Interface'leri**:
  - IQuery<TResult>

- **Handler Interface'leri**:
  - ICommandHandler<TCommand>
  - ICommandHandler<TCommand, TResult>
  - IQueryHandler<TQuery, TResult>

### 8. Unit of Work
- IUnitOfWork interface'i
- UnitOfWork implementasyonu
- Transaction yönetimi

### 9. Proje Yapısı
- Clean Architecture
- Katmanlı mimari
- Interface ve implementasyon ayrımı

### 10. Temizlik ve Düzenleme
- Eski interface'lerin kaldırılması
- Yeni yapıya geçiş
- Kod organizasyonu

## Teknolojiler
- .NET 8
- Entity Framework Core
- SQL Server
- CQRS Pattern
- Repository Pattern
- Unit of Work Pattern

## Katmanlar
1. **Core**: Entity'ler, Interface'ler, Validation Rules
2. **Infrastructure**: Repository'ler, Service'ler, Configuration'lar
3. **API**: Controller'lar, Middleware'ler 