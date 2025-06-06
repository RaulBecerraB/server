# Comandos de Base de Datos

## Prerrequisitos
- Tener MySQL instalado y ejecutándose
- Crear la base de datos `JwtAuthDb` en MySQL
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

## Notas importantes

1. Los JWT tokens se envían automáticamente en cookies HttpOnly
2. La cookie se configura como Secure=true para HTTPS en producción
3. CORS está configurado para permitir credenciales (cookies)
4. La contraseña se hashea usando BCrypt antes de guardarse
5. El email debe ser único en la base de datos 