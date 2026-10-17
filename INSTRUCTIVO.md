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

---

## 🏛️ 6. Arquitectura, Patrones de Diseño y Principios SOLID

El proyecto implementa una arquitectura por capas desacoplada (`Models`, `Data`, `Services`, `Views`), diseñada bajo los estándares de **Programación Orientada a Objetos Avanzada** y los **Principios SOLID**.

### 📐 Patrones de Diseño Implementados y su Justificación

1. **Patrón Repository (Repositorio con Interfaces):**
   - **Clases e Interfaces:** [`IRepository<T>`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/IRepository.cs), [`ICursoRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/ICursoRepository.cs), [`IDeporteRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/IDeporteRepository.cs), [`IAsociacionRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/IAsociacionRepository.cs), [`IArteRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/IArteRepository.cs), [`IUsuarioRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/IUsuarioRepository.cs), [`BaseRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/BaseRepository.cs).
   - **Justificación Técnica:** Desacopla por completo la capa de presentación (formularios WinForms) de la tecnología de persistencia (SQL Server / ADO.NET). Permite modificar las consultas SQL, cambiar el motor de base de datos o realizar pruebas unitarias con repositorios simulados (*mocks*) sin necesidad de alterar una sola línea de código en la interfaz gráfica.

2. **Patrón Singleton (Instancia Única Concurrente):**
   - **Clase:** [`DatabaseConnection`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/DatabaseConnection.cs) (accedida mediante `DatabaseConnection.Instance`).
   - **Justificación Técnica:** La cadena de conexión y la configuración del acceso al servidor de base de datos deben ser centralizadas y únicas en toda la aplicación. Implementar Singleton con `Lazy<DatabaseConnection>` y constructor privado evita la creación redundante de objetos de configuración en memoria, garantiza seguridad multihilo (*thread-safety*) y previene fugas de recursos.

3. **Patrón Factory (Factoría Creacional de Componentes):**
   - **Clase:** [`TarjetaFactory`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/TarjetaFactory.cs).
   - **Justificación Técnica:** Centraliza la creación y mapeo de controles visuales de tarjeta (`ActividadInscritaTarjeta` y `GrupoExtensionTarjeta`). Al recibir cualquier objeto polimórfico derivado de `InscripcionBase` o `ActividadDAC`, la factoría construye y configura la tarjeta de manera uniforme, eliminando la duplicación de código en los formularios [`Portal`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/Portal.cs) y [`GruposExtension`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/GruposExtension.cs).

---

### 🧩 Cumplimiento de Principios SOLID

| Principio | Evidencia en el Código Fuente |
| :--- | :--- |
| **S - Single Responsibility (Responsabilidad Única)** | Cada capa tiene una función delimitada: `Models` almacena entidades de dominio; `Data` encapsula sentencias SQL y stored procedures (`ArteRepository`, `IArteRepository`); `Services` gestiona servicios técnicos externos (Selenium y HTTP a ImgBB) retornando objetos DTO; `Views` se encarga exclusivamente de la renderización visual (`Arte`, `GruposExtension`, `ProgramasFormacion`, `VoluntareadoCultural`, `GrupoExtensionTarjeta`). |
| **O - Open/Closed (Abierto/Cerrado)** | Mediante las clases base abstractas [`ActividadExtracurricularBase`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Models/ActividadExtracurricularBase.cs) e [`InscripcionBase`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Models/InscripcionBase.cs), y la plantilla visual [`PlantillaAACD`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/PlantillaAACD.cs), es posible extender el sistema añadiendo nuevos tipos de actividades o submódulos implementando métodos abstractos/virtuales sin necesidad de modificar el código existente de las clases base. |
| **L - Liskov Substitution (Sustitución de Liskov)** | Las clases hijas [`InscripcionCurso`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Models/InscripcionCurso.cs), [`InscripcionDAC`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Models/InscripcionDAC.cs), [`MembresiaAsociacion`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Models/MembresiaAsociacion.cs) y [`ParticipacionDeportiva`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Models/ParticipacionDeportiva.cs) pueden sustituir a su clase base [`InscripcionBase`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Models/InscripcionBase.cs) en cualquier parte del sistema (como en [`TarjetaFactory.CrearTarjeta`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/TarjetaFactory.cs#L17)); del mismo modo, las vistas especializadas ([`GruposExtension`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/GruposExtension.cs), [`ProgramasFormacion`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/ProgramasFormacion.cs), [`VoluntareadoCultural`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/VoluntareadoCultural.cs)) sustituyen a [`PlantillaAACD`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/PlantillaAACD.cs). |
| **I - Interface Segregation (Segregación de Interfaces)** | En lugar de una interfaz general con decenas de métodos no requeridos por todos los clientes, se crearon interfaces específicas por dominio: [`ICursoRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/ICursoRepository.cs), [`IDeporteRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/IDeporteRepository.cs), [`IAsociacionRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/IAsociacionRepository.cs), [`IArteRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/Interfaces/IArteRepository.cs), [`IImageService`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Services/Interfaces/IImageService.cs), etc. |
| **D - Dependency Inversion (Inversión de Dependencias)** | Los módulos de alto nivel (vistas como [`AdminDashboard`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/AdminDashboard.cs), [`Deportes`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/Deportes.cs), [`Asociaciones`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/Asociaciones.cs), [`Arte`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/Arte.cs), [`GruposExtension`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/GruposExtension.cs), [`ProgramasFormacion`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/ProgramasFormacion.cs) y [`VoluntareadoCultural`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/VoluntareadoCultural.cs)) dependen de abstracciones (interfaces como `IArteRepository`) recibidas mediante sus constructores y no de implementaciones concretas fijas. |

---

### 🧬 Programación Orientada a Objetos Avanzada (POO)

1. **Jerarquías de Clases y Abstracción:**
   - Dominio unificado mediante [`ActividadExtracurricularBase`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Models/ActividadExtracurricularBase.cs) y [`InscripcionBase`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Models/InscripcionBase.cs).
   - Acceso a datos unificado mediante [`BaseRepository`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/BaseRepository.cs).
   - Capa de presentación unificada mediante la plantilla visual [`PlantillaAACD : Form`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/PlantillaAACD.cs).

2. **Polimorfismo con Métodos `virtual`, `abstract` y `override`:**
   - **Modelos:** Métodos abstractos `ObtenerTipoActividad()`, `ObtenerTitulo()`, `ObtenerDetalle()` implementados con `override` en cada modelo derivado; métodos virtuales `ObtenerResumen()`, `EstaDisponible()` y `EsActiva()` con implementaciones base especializadas.
   - **Vistas:** Métodos del ciclo de vida `ConfigurarModulo()` y `CargarDatos()` definidos como `virtual` en [`PlantillaAACD.cs`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/PlantillaAACD.cs) y sobreescritos con `override` en los formularios hijos ([`Deportes`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/Deportes.cs), [`Asociaciones`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/Asociaciones.cs), [`Cursos`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/Cursos.cs), [`Arte`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/Arte.cs), [`GruposExtension`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/GruposExtension.cs), [`ProgramasFormacion`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/ProgramasFormacion.cs), [`VoluntareadoCultural`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Views/VoluntareadoCultural.cs)).

3. **Modificadores de Acceso y Encapsulamiento:**
   - Uso riguroso de `private readonly` para dependencias inmutables.
   - Uso de `protected` en clases base para permitir especialización controlada sin exponer miembros a clientes externos.
   - Constructores privados en el Singleton [`DatabaseConnection`](file:///c:/Users/dougl/OneDrive/Documentos/GitHub/ActividadesExtraPortal/Data/DatabaseConnection.cs) para restringir el ciclo de vida de la instancia.

