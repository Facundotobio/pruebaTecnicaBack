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
└── Tests/              # Suite de Pruebas Unitarias
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
La API estará disponible en `http://localhost:5296` y puedes acceder a Swagger en la raíz `/`.

## 🧪 Testing

El proyecto incluye una suite completa de pruebas unitarias para asegurar la calidad del código y el cumplimiento de las reglas de negocio.

### Stack de Testing
- **xUnit**: Framework principal de pruebas.
- **Moq**: Librería para el aislamiento de dependencias mediante Mocks.
- **FluentAssertions**: Para aserciones más legibles y semánticas.

### Pruebas Implementadas
- **CustomerService**: 
    - Creación y edición de clientes.
    - Validación de borrado seguro (impide borrar clientes con facturas asociadas).
- **InvoiceService**: 
    - **Validación de existencia de cliente**: Asegura que no se puedan crear facturas para clientes inexistentes (Integridad de Negocio).
    - **Cálculo automático de totales**: Verifica que la suma de los items coincida exactamente con el total de la factura.
    - Mapeo de estados y persistencia.

```bash
dotnet test
```
Las pruebas cubren:
- Reglas de negocio de Clientes (borrado lógico, validación de facturas).
- Cálculos automáticos de Facturas.

### Seguridad y Autenticación (JWT)
El sistema utiliza **JSON Web Tokens (JWT)** para proteger los recursos.

#### Flujo de Autenticación:
1. **Login**: POST a `/api/Auth/login` enviando solo el `Email`.
2. **Validación**: El servidor verifica que el cliente esté registrado y con `Estado = Activo`.
3. **Token**: Se devuelve un token JWT válido por 8 horas.

#### Cómo usar el Token:
Para acceder a los endpoints de `Customer` o `Invoice`, se debe incluir el header de autorización:
`Authorization: Bearer <token>`

#### Configuración (appsettings.json):
```json
"Jwt": {
  "Key": "Su_Clave_Secreta",
  "Issuer": "PruebaTecnicaAPI",
  "Audience": "PruebaTecnicaFront"
}
```

## 🧠 Decisiones de Diseño

1. **DTOs:** Se utilizan para no exponer las entidades de base de datos directamente, protegiendo campos internos como IDs de sistema o fechas de auditoría.
2. **Repository Pattern:** Permite desacoplar la lógica de negocio del ORM, facilitando el Testing y el cambio de motor de base de datos si fuera necesario.
3. **Inversión de Dependencias:** Los servicios dependen de interfaces de repositorios, no de clases concretas, cumpliendo con el principio SOLID (D).
4. **Validación:** Se utilizan Data Annotations en los DTOs para asegurar la integridad de los datos de entrada.

---
Desarrollado por **Facundo Tobio** para Prueba Técnica.
