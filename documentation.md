# Prueba Técnica - Backend de Facturación

Este proyecto es una API REST desarrollada en **.NET 8** para la gestión de clientes y facturación, siguiendo los principios de **Clean Architecture** y las mejores prácticas de desarrollo.

## 🚀 Tecnologías Utilizadas

- **Lenguaje:** C# / .NET 8
- **ORM:** Entity Framework Core
- **Base de Datos:** PostgreSQL
- **Documentación:** Swagger / OpenAPI
- **Patrones:** Repository Pattern, Dependency Injection, DTOs.

## 🏛️ Arquitectura del Proyecto

El proyecto está organizado siguiendo una estructura de **Clean Architecture** para garantizar el desacoplamiento y la mantenibilidad:

- **Domain:** El núcleo del negocio. Contiene las entidades y las interfaces de los repositorios (abstracciones).
- **Application:** Contiene la lógica de negocio (Servicios), los DTOs para transferencia de datos y las interfaces de servicio.
- **Infrastructure:** Implementación de detalles técnicos como el acceso a datos (DbContext) y los repositorios concretos.
- **Presentation:** Capa de entrada de la aplicación. Contiene los controladores de la API y la configuración principal.

## 📁 Estructura de Carpetas

```text
PruebaTecnicaFacundoTobioBack/
├── Application/
│   ├── DTOs/           # Objetos de entrada/salida de la API
│   ├── Interfaces/     # Contratos de servicios
│   └── Services/       # Lógica de negocio
├── Domain/
│   ├── Entities/       # Modelos de base de datos
│   └── Interfaces/     # Contratos de repositorios (Inversión de Dependencia)
├── Infrastructure/
│   ├── Data/           # DbContext y configuraciones de EF Core
│   └── Repositories/   # Implementación de acceso a datos
├── Presentation/
│   └── Controllers/    # Endpoints de la API
└── Tests/              # Estructura preparada para Unit Testing
```

## 🛠️ Comandos Principales

### Migraciones de Base de Datos
Si necesitas actualizar la base de datos:
```bash
dotnet ef migrations add NombreDeLaMigracion
dotnet ef database update
```

### Ejecutar el Proyecto
```bash
dotnet run
```
La API estará disponible en `http://localhost:5296` (o el puerto configurado) y puedes acceder a Swagger en la raíz `/`.

## 🧠 Decisiones de Diseño

1. **DTOs:** Se utilizan para no exponer las entidades de base de datos directamente, protegiendo campos internos como IDs de sistema o fechas de auditoría.
2. **Repository Pattern:** Permite desacoplar la lógica de negocio del ORM, facilitando el Testing y el cambio de motor de base de datos si fuera necesario.
3. **Inversión de Dependencias:** Los servicios dependen de interfaces de repositorios, no de clases concretas, cumpliendo con el principio SOLID (D).
4. **Validación:** Se utilizan Data Annotations en los DTOs para asegurar la integridad de los datos de entrada.

---
Desarrollado por **Facundo Tobio** para Prueba Técnica.
