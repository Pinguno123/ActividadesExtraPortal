-- Consultas SQL para creación de tablas extraídas de proyecto.sql

-- 1. Tabla: Roles
CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(250) NULL
);

-- 2. Tabla: Usuarios
CREATE TABLE Usuarios (
    Carnet VARCHAR(10) PRIMARY KEY,
    NombreCompleto VARCHAR(100) NOT NULL,
    Carrera VARCHAR(150) NOT NULL,
    Campus VARCHAR(100) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    CorreoElectronico VARCHAR(50) NOT NULL,
    IdRol INT NOT NULL,
    FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)
);

-- 3. Tabla: Asociaciones
CREATE TABLE Asociaciones (
    IdAsociacion INT IDENTITY(1,1) PRIMARY KEY,
    Acronimo VARCHAR(20) NOT NULL,
    Nombre VARCHAR(200) NOT NULL,
    AnioFundacion INT,
    Descripcion NVARCHAR(MAX),
    Formulario NVARCHAR(MAX),
    ImgUrl NVARCHAR(MAX)
);

-- 4. Tabla: MembresiaAsociaciones
CREATE TABLE MembresiaAsociaciones (
    IdMembresia INT IDENTITY(1,1) PRIMARY KEY,
    Carnet VARCHAR(10) NOT NULL,
    IdAsociacion INT NOT NULL,
    Rol VARCHAR(50) DEFAULT 'Miembro', -- Rol
    FechaSolicitud DATE DEFAULT GETDATE(),
    EstadoValidacion VARCHAR(20) DEFAULT 'Pendiente', -- Estado
    CONSTRAINT CK_RolMembresia CHECK (Rol IN ('Miembro', 'Directiva', 'Presidente', 'Tesorero', 'Secretario')),
    CONSTRAINT CK_EstadoValidacion CHECK (EstadoValidacion IN ('Pendiente', 'Aprobada', 'Rechazada')),
    CONSTRAINT UQ_Estudiante_Asociacion UNIQUE (Carnet, IdAsociacion),
    FOREIGN KEY (Carnet) REFERENCES Usuarios(Carnet),
    FOREIGN KEY (IdAsociacion) REFERENCES Asociaciones(IdAsociacion)
);

-- 5. Tabla: Cursos
CREATE TABLE Cursos (
    IdCurso INT IDENTITY(1,1) PRIMARY KEY, -- ID autoincrementable
    NombreCurso VARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(500),
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    Horario VARCHAR(100) NOT NULL,
    CupoMaximo INT NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Activo',
    CarnetInstructor VARCHAR(10) NULL,
    CONSTRAINT CK_FechasCurso CHECK (FechaFin >= FechaInicio),
    CONSTRAINT CK_Curso_InstructorValido CHECK (CarnetInstructor IS NULL OR dbo.fn_ValidarInstructor(CarnetInstructor) = 1),
    CONSTRAINT CK_EstadoCurso CHECK (Estado IN ('Activo', 'Inactivo', 'Finalizado')),
    FOREIGN KEY (CarnetInstructor) REFERENCES Usuarios(Carnet)
);

-- 6. Tabla: InscripcionCursos
CREATE TABLE InscripcionCursos (
    IdInscripcion INT IDENTITY(1,1) PRIMARY KEY,
    Carnet VARCHAR(10) NOT NULL,
    IdCurso INT NOT NULL,
    Nota DECIMAL(4,2) NULL, -- Nota
    FechaInscripcion DATETIME DEFAULT GETDATE(),
    EstadoInscripcion VARCHAR(20) DEFAULT 'Confirmada',
    CONSTRAINT CK_EstadoInscripcion CHECK (EstadoInscripcion IN ('Confirmada', 'En Espera', 'Cancelada')),
    CONSTRAINT UQ_Estudiante_Curso UNIQUE (Carnet, IdCurso),
    FOREIGN KEY (Carnet) REFERENCES Usuarios(Carnet),
    FOREIGN KEY (IdCurso) REFERENCES Cursos(IdCurso)
);

-- 7. Tabla: SeleccionesDeportivas
CREATE TABLE SeleccionesDeportivas (
    IdSeleccion INT IDENTITY(1,1) PRIMARY KEY,
    Disciplina VARCHAR(50) NOT NULL, -- Disciplina
    Rama VARCHAR(20) NOT NULL,       -- Rama
    NombreEquipo VARCHAR(100) NOT NULL,
    ImgUrl NVARCHAR(MAX)
);

-- 8. Tabla: ParticipacionDeportiva
CREATE TABLE ParticipacionDeportiva (
    IdParticipacion INT IDENTITY(1,1) PRIMARY KEY,
    Carnet VARCHAR(10) NOT NULL,
    IdSeleccion INT NOT NULL,
    Modalidad VARCHAR(100), -- Modalidad
    FechaIngreso DATE DEFAULT GETDATE(),
    CONSTRAINT UQ_Estudiante_Seleccion UNIQUE (Carnet, IdSeleccion),
    FOREIGN KEY (Carnet) REFERENCES Usuarios(Carnet),
    FOREIGN KEY (IdSeleccion) REFERENCES SeleccionesDeportivas(IdSeleccion)
);

-- 9. Tabla: CategoriasDAC
CREATE TABLE CategoriasDAC (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    NombreCategoria VARCHAR(100) NOT NULL, -- Categoria
    Descripcion NVARCHAR(500)
);

-- 10. Tabla: ActividadesDAC
CREATE TABLE ActividadesDAC (
    IdActividad INT IDENTITY(1,1) PRIMARY KEY,
    IdCategoria INT NOT NULL,
    NombreActividad VARCHAR(150) NOT NULL, -- Actividad
    Descripcion NVARCHAR(MAX),
    Estado VARCHAR(20) DEFAULT 'Activo',
    CarnetInstructor VARCHAR(10) NULL,
    CONSTRAINT CK_Actividad_InstructorValido CHECK (CarnetInstructor IS NULL OR dbo.fn_ValidarInstructor(CarnetInstructor) = 1),
    CONSTRAINT CK_EstadoActividadDAC CHECK (Estado IN ('Activo', 'Inactivo')),
    FOREIGN KEY (IdCategoria) REFERENCES CategoriasDAC(IdCategoria),
    FOREIGN KEY (CarnetInstructor) REFERENCES Usuarios(Carnet)
);

-- 11. Tabla: InscripcionesDAC
CREATE TABLE InscripcionesDAC (
    IdInscripcionDAC INT IDENTITY(1,1) PRIMARY KEY,
    Carnet VARCHAR(10) NOT NULL,
    IdActividad INT NOT NULL,
    FechaInscripcion DATETIME DEFAULT GETDATE(),
    EstadoInscripcion VARCHAR(20) DEFAULT 'Activa', -- Estado
    CONSTRAINT CK_EstadoInscripcionDAC CHECK (EstadoInscripcion IN ('Activa', 'Finalizada', 'Retirada')),
    CONSTRAINT UQ_Estudiante_ActividadDAC UNIQUE (Carnet, IdActividad),
    FOREIGN KEY (Carnet) REFERENCES Usuarios(Carnet),
    FOREIGN KEY (IdActividad) REFERENCES ActividadesDAC(IdActividad)
);
