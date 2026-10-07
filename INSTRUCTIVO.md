# Instructivo de Ejecución y Compilación

Este documento detalla los pasos necesarios para configurar, abrir, compilar y ejecutar el proyecto **ActividadesExtraPortal** en **Visual Studio 2022**.

---

## 🛠️ Requisitos Previos

Antes de comenzar, asegúrate de tener instalado y configurado lo siguiente en tu sistema:

1. **Visual Studio 2022** (Versión 17.10 o superior recomendada):
   - Asegúrate de incluir la carga de trabajo **Desarrollo de escritorio de .NET** (la cual incluye el soporte para Windows Forms).
2. **SDK de .NET 10.0** (o la versión correspondiente definida en el archivo del proyecto [`ActividadesExtraPortal.csproj`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/ActividadesExtraPortal.csproj)).
3. **SQL Server** (LocalDB, SQL Express o una instancia completa de SQL Server Developer/Enterprise).
4. **Google Chrome**: Requerido por Selenium WebDriver para realizar el raspado de datos (scraping) del portal estudiantil.

---

## 🗄️ 1. Configuración de la Base de Datos

El sistema almacena la información de los estudiantes, asociaciones, selecciones deportivas y cursos en una base de datos de SQL Server llamada `GestionExtracurricular`.

### Pasos para montar la base de datos:
1. Abre **SQL Server Management Studio (SSMS)** o **Azure Data Studio**.
2. Conéctate a tu servidor local de base de datos (por ejemplo, `localhost`, `.` o `(localdb)\MSSQLLocalDB`).
3. Abre el archivo de script SQL de creación de base de datos e inserciones ubicado en:
   - [`Resources/proyecto.sql`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Resources/proyecto.sql)
4. Ejecuta todo el script (**F5**). Esto creará:
   - La base de datos `GestionExtracurricular`.
   - Las tablas correspondientes (Roles, Usuarios, Asociaciones, Membresías, etc.).
   - Triggers de validación de cupos e inscripciones.
   - Vistas de reportes.
   - Procedimientos almacenados para inscripciones y registros.

> [!IMPORTANT]
> **Verificación de la Cadena de Conexión:**
> Por defecto, el proyecto busca la base de datos en `localhost` usando autenticación de Windows.
> Si usas otra instancia de base de datos (como SQL Express o LocalDB), debes abrir el archivo [`Data/DatabaseConnection.cs`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/DatabaseConnection.cs#L9) y modificar la línea 9 con tu cadena de conexión correcta:
> ```csharp
> private readonly string connectionString = @"Server=TU_SERVIDOR;Database=GestionExtracurricular;Trusted_Connection=True;TrustServerCertificate=True;";
> ```

---

## 📂 2. Cómo abrir el proyecto en Visual Studio

Existen dos opciones sencillas para abrir el proyecto en Visual Studio 2022:

### Opción A (Recomendada): Usar el archivo de solución moderna `.slnx`
1. Abre **Visual Studio 2022**.
2. En la ventana de inicio, haz clic en **Abrir un proyecto o una solución**.
3. Navega a la carpeta del proyecto y selecciona el archivo [`ActividadesExtraPortal.slnx`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/ActividadesExtraPortal.slnx).
4. Haz clic en **Abrir**.

### Opción B: Abrir directamente el archivo del proyecto `.csproj`
1. En la ventana de inicio de Visual Studio, haz clic en **Abrir un proyecto o una solución**.
2. Selecciona el archivo [`ActividadesExtraPortal.csproj`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/ActividadesExtraPortal.csproj).
3. Haz clic en **Abrir**.

---

## 📦 3. Restaurar dependencias de NuGet

El proyecto utiliza paquetes NuGet de terceros para la interfaz de usuario de material design, base de datos y Selenium:
* `MaterialSkin.2` (Diseño moderno e interactivo para formularios de Windows).
* `Selenium.WebDriver` (Automatización del navegador para la captura de credenciales/datos).
* `Microsoft.Data.SqlClient` (Conector de acceso a datos para SQL Server).

Visual Studio restaurará estos paquetes de manera automática al abrir la solución o al compilar por primera vez. Si requieres hacerlo manualmente:
1. En el **Explorador de soluciones**, haz clic derecho sobre la solución/proyecto.
2. Selecciona **Restaurar paquetes NuGet**.

---

## 🏗️ 4. Compilación del proyecto

Para compilar el código y generar el archivo ejecutable (`.exe`):

1. En el menú superior de Visual Studio, ve a **Compilar** y selecciona **Compilar solución** (o presiona la combinación de teclas `Ctrl + Shift + B`).
2. Verifica en la ventana de **Salida (Output)** que la compilación haya finalizado con éxito:
   ```text
   ========== Compilar: 1 correctos, 0 incorrectos, 0 omitidos ==========
   ```

---

## 🚀 5. Ejecución del programa

Para ejecutar la aplicación en modo de depuración o de forma directa:

1. Presiona la tecla **F5** o haz clic en el botón con el ícono de play verde **Iniciar (Start / ActividadesExtraPortal)** en la barra de herramientas superior.
2. **Flujo automático de ejecución de la aplicación:**
   - La aplicación iniciará y lanzará una ventana automatizada de **Google Chrome** mediante Selenium que navegará al Portal Académico de la Universidad Don Bosco (`https://admacad.udb.edu.sv/PortalWeb/`).
   - Introduce tus credenciales en el navegador Chrome de manera normal.
   - Una vez inicies sesión exitosamente en el portal, el programa extraerá automáticamente los datos de tu perfil (Nombre, Carnet, Carrera, Campus y Foto de perfil).
   - El navegador Selenium se cerrará automáticamente.
   - Los datos recuperados se registrarán o actualizarán de forma transparente en la base de datos local `GestionExtracurricular`.
   - Se abrirá la ventana principal de la aplicación con un diseño de **Material Design** cargando dinámicamente tu información personalizada en los apartados de Cursos, Selecciones Deportivas, Asociaciones y Actividades de Arte y Cultura.

> [!NOTE]
> Si la base de datos local no está disponible o la conexión falla, la aplicación registrará un error en la consola de advertencia, pero te permitirá navegar a través de la interfaz del portal de manera demostrativa.
