# EncuestaBackend

Un backend robusto desarrollado en C# y .NET diseñado para la creación, gestión y análisis de encuestas. Este proyecto implementa una arquitectura en capas (N-Tier) para separar las responsabilidades y asegurar la escalabilidad y el mantenimiento del código.

## 🏗️ Arquitectura y Estructura del Proyecto

La solución (`EncuestaBackend.sln`) está dividida en las siguientes capas lógicas:

- **Entidades**: Contiene los modelos de dominio, clases POCO que representan los objetos centrales del negocio (ej. Encuestas, Preguntas, Respuestas).
- **Data (Acceso a Datos)**: Se encarga de la comunicación con la base de datos. Aquí se configuran los contextos de base de datos, repositorios y consultas.
- **Logica (Lógica de Negocio)**: Contiene los servicios, reglas de negocio y validaciones. Actúa como intermediario entre la capa de presentación y la capa de acceso a datos.
- **Presentacion (API)**: Es el punto de entrada de la aplicación. Expone los endpoints RESTful (Controladores) con los que interactúan los clientes (aplicaciones web, móviles, etc.).

## 🚀 Tecnologías y Herramientas
- **Lenguaje**: C#
- **Framework**: .NET
- **Arquitectura**: N-Capas (N-Tier)
- **Entorno Recomendado**: Visual Studio 2022 o Visual Studio Code

## 📋 Requisitos Previos
Antes de ejecutar el proyecto, asegúrate de tener instalado:
- [.NET SDK](https://dotnet.microsoft.com/download)
- Un IDE compatible (Visual Studio, JetBrains Rider o VS Code).
- Servidor de base de datos (según la configuración del proyecto, típicamente SQL Server).

## 🛠️ Cómo ejecutar el proyecto localmente

1. **Clona el repositorio**:
   ```bash
   git clone https://github.com/eudyyuniorramires/EncuestaBackend.git
   ```
2. **Navega al directorio del proyecto**:
   ```bash
   cd EncuestaBackend
   ```
3. **Abre la solución**:
   Abre el archivo `EncuestaBackend.sln` en tu IDE.
4. **Restaura los paquetes NuGet**:
   ```bash
   dotnet restore
   ```
5. **Configura la Base de Datos**:
   Verifica el archivo de configuración (ej. `appsettings.json` en el proyecto de Presentación) para asegurar que la cadena de conexión apunte a tu base de datos local. Aplica las migraciones si el proyecto usa Entity Framework.
6. **Ejecuta la aplicación**:
   Establece el proyecto **Presentacion** como proyecto de inicio y presiona `F5`, o usa la terminal:
   ```bash
   dotnet run --project Presentacion
   ```

## 🤝 Contribuciones
Las contribuciones, problemas (issues) y solicitudes de extracción (pull requests) son bienvenidas. Siéntete libre de revisar y proponer mejoras en la estructura o el código.