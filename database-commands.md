# Sistema de Autenticación JWT - Arquitectura Modular

## Estructura del Proyecto

### 📁 **Arquitectura Modular**
```
server/
├── Constants/              # Constantes de la aplicación
│   └── AuthConstants.cs    # Constantes de autenticación
├── Controllers/            # Controladores API
│   └── AuthController.cs   # Endpoints de autenticación
├── Data/                   # Contexto de base de datos
│   └── ApplicationDbContext.cs
├── Extensions/             # Métodos de extensión
│   └── ServiceCollectionExtensions.cs
├── Helpers/                # Clases auxiliares
│   └── CookieHelper.cs     # Manejo de cookies JWT
├── Middleware/             # Middleware personalizado
│   └── JwtCookieMiddleware.cs
├── Models/                 # Modelos de datos
│   ├── User.cs
│   └── DTOs/               # Data Transfer Objects
│       ├── AuthResultDto.cs
│       ├── LoginDto.cs
│       ├── RegisterDto.cs
│       └── UserResponseDto.cs
└── Services/               # Servicios de negocio
    ├── Interfaces/         # Interfaces de servicios
    │   ├── IAuthService.cs
    │   ├── IJwtService.cs
    │   └── IPasswordService.cs
    ├── AuthService.cs      # Lógica de autenticación
    ├── JwtService.cs       # Manejo de JWT tokens
    └── PasswordService.cs  # Hashing de contraseñas
```

## Prerrequisitos
- Tener MySQL instalado y ejecutándose
- Crear la base de datos `jwt-example` en MySQL
- Ajustar la cadena de conexión en `appsettings.json` con tus credenciales de MySQL

## Comandos Entity Framework

### 1. Restaurar paquetes NuGet
```bash
dotnet restore
```

### 2. Crear migración inicial
```bash
dotnet ef migrations add InitialCreate
```

### 3. Aplicar migración a la base de datos
```bash
dotnet ef database update
```

### 4. Ejecutar la aplicación
```bash
dotnet run
```

## Endpoints disponibles

### Registro de usuario
```
POST /api/auth/register
Content-Type: application/json

{
  "email": "usuario@ejemplo.com",
  "password": "password123",
  "firstName": "Juan",
  "lastName": "Pérez"
}
```

### Login
```
POST /api/auth/login
Content-Type: application/json

{
  "email": "usuario@ejemplo.com",
  "password": "password123"
}
```

### Obtener usuario actual (requiere autenticación)
```
GET /api/auth/me
```

### Logout
```
POST /api/auth/logout
```

## Características de la Arquitectura

### ✅ **Principios SOLID**
- **Single Responsibility**: Cada clase tiene una responsabilidad específica
- **Open/Closed**: Extensible mediante interfaces
- **Liskov Substitution**: Implementaciones intercambiables
- **Interface Segregation**: Interfaces específicas y cohesivas
- **Dependency Inversion**: Dependencias mediante abstracciones

### ✅ **Patrones Implementados**
- **Repository Pattern**: A través del DbContext
- **Service Layer Pattern**: Servicios de negocio separados
- **Dependency Injection**: Inyección de dependencias nativa de .NET
- **Extension Methods**: Para configuración modular
- **Constants Pattern**: Evita magic strings

### ✅ **Beneficios**
- **Mantenibilidad**: Código organizado y fácil de mantener
- **Testabilidad**: Servicios mockeables mediante interfaces
- **Escalabilidad**: Fácil agregar nuevas funcionalidades
- **Reutilización**: Componentes reutilizables
- **Separación de responsabilidades**: Cada capa tiene su propósito

### ✅ **Seguridad**
- JWT tokens en cookies HttpOnly
- Hashing de contraseñas con BCrypt
- Validación de tokens automática
- CORS configurado para credenciales
- Configuración segura de cookies

## Notas importantes

1. Los JWT tokens se envían automáticamente en cookies HttpOnly
2. La cookie se configura como Secure=true para HTTPS en producción
3. CORS está configurado para permitir credenciales (cookies)
4. La contraseña se hashea usando BCrypt antes de guardarse
5. El email debe ser único en la base de datos
6. Todas las constantes están centralizadas en `AuthConstants.cs`
7. Los servicios están registrados mediante inyección de dependencias
8. La configuración está modularizada en métodos de extensión 