-- Base de datos
CREATE DATABASE GestionExtracurricular;
GO
USE GestionExtracurricular;
GO

-- Tablas de roles y usuarios
CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(250) NULL
);

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
GO

-- Valida instructor
CREATE FUNCTION dbo.fn_ValidarInstructor(@Carnet VARCHAR(10))
RETURNS BIT
AS
BEGIN
    DECLARE @EsValido BIT = 0;
    IF EXISTS (
        SELECT 1 
        FROM Usuarios u
        JOIN Roles r ON u.IdRol = r.IdRol
        WHERE u.Carnet = @Carnet 
          AND u.Activo = 1 
          AND r.NombreRol = 'Instructor'
    )
    BEGIN
        SET @EsValido = 1;
    END
    RETURN @EsValido;
END;
GO

-- Asociaciones
CREATE TABLE Asociaciones (
    IdAsociacion INT IDENTITY(1,1) PRIMARY KEY,
    Acronimo VARCHAR(20) NOT NULL,
    Nombre VARCHAR(200) NOT NULL,
    AnioFundacion INT,
    Descripcion NVARCHAR(MAX),
    Formulario NVARCHAR(MAX),
    ImgUrl NVARCHAR(MAX)
);

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

-- Cursos
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

-- Deportes
CREATE TABLE SeleccionesDeportivas (
    IdSeleccion INT IDENTITY(1,1) PRIMARY KEY,
    Disciplina VARCHAR(50) NOT NULL, -- Disciplina
    Rama VARCHAR(20) NOT NULL,       -- Rama
    NombreEquipo VARCHAR(100) NOT NULL,
    ImgUrl NVARCHAR(MAX)
);

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

-- Arte y cultura
CREATE TABLE CategoriasDAC (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    NombreCategoria VARCHAR(100) NOT NULL, -- Categoria
    Descripcion NVARCHAR(500)
);

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
GO

-- Triggers
GO

-- Trigger de inserción
CREATE TRIGGER TR_InscripcionCursos_Insert
ON InscripcionCursos
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Inserta registros
    DECLARE @Carnet VARCHAR(10), @IdCurso INT, @Nota DECIMAL(4,2), @FechaInscripcion DATETIME, @EstadoInscripcion VARCHAR(20);
    
    DECLARE cursor_inscripciones CURSOR LOCAL FOR 
    SELECT Carnet, IdCurso, Nota, FechaInscripcion, EstadoInscripcion FROM inserted;
    
    OPEN cursor_inscripciones;
    FETCH NEXT FROM cursor_inscripciones INTO @Carnet, @IdCurso, @Nota, @FechaInscripcion, @EstadoInscripcion;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @CupoMaximo INT;
        DECLARE @InscritosActivos INT;
        
        -- Obtiene cupo
        SELECT @CupoMaximo = CupoMaximo FROM Cursos WHERE IdCurso = @IdCurso;
        
        -- Obtiene inscritos
        SELECT @InscritosActivos = COUNT(*) FROM InscripcionCursos 
        WHERE IdCurso = @IdCurso AND EstadoInscripcion = 'Confirmada';
        
        -- Define estado
        DECLARE @EstadoFinal VARCHAR(20) = COALESCE(@EstadoInscripcion, 'Confirmada');
        
        IF @EstadoFinal = 'Confirmada' AND @InscritosActivos >= @CupoMaximo
        BEGIN
            SET @EstadoFinal = 'En Espera';
        END
        
        -- Inserta
        INSERT INTO InscripcionCursos (Carnet, IdCurso, Nota, FechaInscripcion, EstadoInscripcion)
        VALUES (@Carnet, @IdCurso, @Nota, COALESCE(@FechaInscripcion, GETDATE()), @EstadoFinal);
        
        FETCH NEXT FROM cursor_inscripciones INTO @Carnet, @IdCurso, @Nota, @FechaInscripcion, @EstadoInscripcion;
    END
    
    CLOSE cursor_inscripciones;
    DEALLOCATE cursor_inscripciones;
END;
GO

-- Trigger de actualización y borrado
CREATE TRIGGER TR_InscripcionCursos_UpdateDelete
ON InscripcionCursos
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verifica cupo
    -- Caso 1
    -- Caso 2
    IF EXISTS (
        SELECT 1 
        FROM deleted d
        LEFT JOIN inserted i ON d.IdInscripcion = i.IdInscripcion
        WHERE d.EstadoInscripcion = 'Confirmada' 
          AND (i.IdInscripcion IS NULL OR i.EstadoInscripcion = 'Cancelada')
    )
    BEGIN
        -- Cursos afectados
        DECLARE @IdCursoAfectado INT;
        
        DECLARE cursor_cursos_afectados CURSOR LOCAL FOR
        SELECT DISTINCT d.IdCurso 
        FROM deleted d
        LEFT JOIN inserted i ON d.IdInscripcion = i.IdInscripcion
        WHERE d.EstadoInscripcion = 'Confirmada' 
          AND (i.IdInscripcion IS NULL OR i.EstadoInscripcion = 'Cancelada');
        
        OPEN cursor_cursos_afectados;
        FETCH NEXT FROM cursor_cursos_afectados INTO @IdCursoAfectado;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            DECLARE @CupoMaximo INT;
            DECLARE @InscritosActivos INT;
            
            SELECT @CupoMaximo = CupoMaximo FROM Cursos WHERE IdCurso = @IdCursoAfectado;
            SELECT @InscritosActivos = COUNT(*) FROM InscripcionCursos 
            WHERE IdCurso = @IdCursoAfectado AND EstadoInscripcion = 'Confirmada';
            
            -- Si hay cupos
            WHILE @InscritosActivos < @CupoMaximo
            BEGIN
                DECLARE @SiguienteInscripcionId INT = NULL;
                
                -- Obtiene el ID
                SELECT TOP 1 @SiguienteInscripcionId = IdInscripcion
                FROM InscripcionCursos
                WHERE IdCurso = @IdCursoAfectado AND EstadoInscripcion = 'En Espera'
                ORDER BY FechaInscripcion ASC;
                
                -- Si es nulo, sale
                IF @SiguienteInscripcionId IS NULL
                    BREAK;
                
                -- Actualiza
                UPDATE InscripcionCursos
                SET EstadoInscripcion = 'Confirmada'
                WHERE IdInscripcion = @SiguienteInscripcionId;
                
                -- Recalcula
                SELECT @InscritosActivos = COUNT(*) FROM InscripcionCursos 
                WHERE IdCurso = @IdCursoAfectado AND EstadoInscripcion = 'Confirmada';
            END
            
            FETCH NEXT FROM cursor_cursos_afectados INTO @IdCursoAfectado;
        END
        
        CLOSE cursor_cursos_afectados;
        DEALLOCATE cursor_cursos_afectados;
    END
END;
GO

-- Vistas
GO

-- Vista inscripciones cursos
CREATE VIEW uv_DetalleInscripcionesCursos AS
SELECT 
    i.IdInscripcion,
    u.Carnet,
    u.NombreCompleto AS NombreEstudiante,
    u.Carrera,
    u.Campus,
    c.IdCurso,
    c.NombreCurso,
    c.FechaInicio,
    c.FechaFin,
    c.Horario,
    c.CupoMaximo,
    i.Nota,
    i.FechaInscripcion,
    i.EstadoInscripcion,
    inst.NombreCompleto AS NombreInstructor
FROM InscripcionCursos i
JOIN Usuarios u ON i.Carnet = u.Carnet
JOIN Cursos c ON i.IdCurso = c.IdCurso
LEFT JOIN Usuarios inst ON c.CarnetInstructor = inst.Carnet;
GO

-- Vista membresias
CREATE VIEW uv_DetalleMembresiasAsociaciones AS
SELECT 
    m.IdMembresia,
    u.Carnet,
    u.NombreCompleto AS NombreEstudiante,
    u.Carrera,
    a.IdAsociacion,
    a.Acronimo,
    a.Nombre AS NombreAsociacion,
    m.Rol,
    m.FechaSolicitud,
    m.EstadoValidacion
FROM MembresiaAsociaciones m
JOIN Usuarios u ON m.Carnet = u.Carnet
JOIN Asociaciones a ON m.IdAsociacion = a.IdAsociacion;
GO

-- Vista inscripciones dac
CREATE VIEW uv_DetalleInscripcionesDAC AS
SELECT 
    i.IdInscripcionDAC,
    u.Carnet,
    u.NombreCompleto AS NombreEstudiante,
    u.Carrera,
    a.IdActividad,
    a.NombreActividad,
    c.NombreCategoria,
    i.FechaInscripcion,
    i.EstadoInscripcion,
    inst.NombreCompleto AS NombreInstructor
FROM InscripcionesDAC i
JOIN Usuarios u ON i.Carnet = u.Carnet
JOIN ActividadesDAC a ON i.IdActividad = a.IdActividad
JOIN CategoriasDAC c ON a.IdCategoria = c.IdCategoria
LEFT JOIN Usuarios inst ON a.CarnetInstructor = inst.Carnet;
GO

-- Vista deportes
CREATE VIEW uv_DetalleParticipacionDeportiva AS
SELECT 
    p.IdParticipacion,
    u.Carnet,
    u.NombreCompleto AS NombreEstudiante,
    u.Carrera,
    s.IdSeleccion,
    s.Disciplina,
    s.Rama,
    s.NombreEquipo,
    p.Modalidad,
    p.FechaIngreso
FROM ParticipacionDeportiva p
JOIN Usuarios u ON p.Carnet = u.Carnet
JOIN SeleccionesDeportivas s ON p.IdSeleccion = s.IdSeleccion;
GO

-- Procedimientos
GO

-- Inscribe a curso
CREATE PROCEDURE dbo.sp_InscribirEstudianteCurso
    @Carnet VARCHAR(10),
    @IdCurso INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verifica estudiante
        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Carnet = @Carnet)
        BEGIN
            ;THROW 50001, 'El estudiante con el carnet especificado no está registrado en el sistema.', 1;
        END
        
        -- Verifica curso
        IF NOT EXISTS (SELECT 1 FROM Cursos WHERE IdCurso = @IdCurso)
        BEGIN
            ;THROW 50002, 'El curso especificado no existe.', 1;
        END
        
        -- Verifica inscripción
        DECLARE @EstadoActual VARCHAR(20) = NULL;
        DECLARE @IdInscripcion INT = NULL;
        
        SELECT @IdInscripcion = IdInscripcion, @EstadoActual = EstadoInscripcion 
        FROM InscripcionCursos 
        WHERE Carnet = @Carnet AND IdCurso = @IdCurso;
        
        IF @EstadoActual IS NOT NULL
        BEGIN
            -- Si está activo, error
            IF @EstadoActual IN ('Confirmada', 'En Espera')
            BEGIN
                DECLARE @MsgError VARCHAR(150) = 'El estudiante ya cuenta con una inscripción activa en este curso. Estado actual: ' + @EstadoActual;
                ;THROW 50003, @MsgError, 1;
            END
            
            -- Si está cancelado, re-inscribe
            IF @EstadoActual = 'Cancelada'
            BEGIN
                DECLARE @CupoMaximo INT;
                DECLARE @InscritosActivos INT;
                
                SELECT @CupoMaximo = CupoMaximo FROM Cursos WHERE IdCurso = @IdCurso;
                SELECT @InscritosActivos = COUNT(*) FROM InscripcionCursos 
                WHERE IdCurso = @IdCurso AND EstadoInscripcion = 'Confirmada';
                
                DECLARE @NuevoEstado VARCHAR(20) = 'Confirmada';
                
                -- Si no hay cupo, espera
                IF @InscritosActivos >= @CupoMaximo
                BEGIN
                    SET @NuevoEstado = 'En Espera';
                END
                
                UPDATE InscripcionCursos
                SET EstadoInscripcion = @NuevoEstado,
                    FechaInscripcion = GETDATE(),
                    Nota = NULL
                WHERE IdInscripcion = @IdInscripcion;
            END
        END
        ELSE
        BEGIN
            -- Inserta
            INSERT INTO InscripcionCursos (Carnet, IdCurso)
            VALUES (@Carnet, @IdCurso);
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Cancela curso
CREATE PROCEDURE dbo.sp_CancelarInscripcionCurso
    @Carnet VARCHAR(10),
    @IdCurso INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verifica inscripción
        IF NOT EXISTS (
            SELECT 1 
            FROM InscripcionCursos 
            WHERE Carnet = @Carnet AND IdCurso = @IdCurso AND EstadoInscripcion IN ('Confirmada', 'En Espera')
        )
        BEGIN
            ;THROW 50004, 'No existe una inscripción activa para el estudiante en el curso especificado.', 1;
        END
        
        -- Actualiza
        UPDATE InscripcionCursos
        SET EstadoInscripcion = 'Cancelada'
        WHERE Carnet = @Carnet AND IdCurso = @IdCurso;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Registra usuario
CREATE PROCEDURE dbo.sp_RegistrarUsuario
    @Carnet VARCHAR(10),
    @NombreCompleto VARCHAR(100),
    @Carrera VARCHAR(150),
    @Campus VARCHAR(100),
    @CorreoElectronico VARCHAR(50),
    @NombreRol VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verifica rol
        DECLARE @IdRol INT = NULL;
        SELECT @IdRol = IdRol FROM Roles WHERE NombreRol = @NombreRol;
        
        IF @IdRol IS NULL
        BEGIN
            DECLARE @MsgError VARCHAR(150) = 'El rol especificado (' + COALESCE(@NombreRol, 'NULL') + ') no existe en el sistema.';
            ;THROW 50005, @MsgError, 1;
        END
        
        -- Verifica carnet
        IF EXISTS (SELECT 1 FROM Usuarios WHERE Carnet = @Carnet)
        BEGIN
            ;THROW 50006, 'El carnet ingresado ya se encuentra registrado.', 1;
        END
        
        -- Inserta
        INSERT INTO Usuarios (Carnet, NombreCompleto, Carrera, Campus, Activo, CorreoElectronico, IdRol)
        VALUES (@Carnet, @NombreCompleto, @Carrera, @Campus, 1, @CorreoElectronico, @IdRol);
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Vistas de catálogo
GO

-- Instructores activos
CREATE VIEW uv_ListarInstructores AS
SELECT 
    u.Carnet,
    u.NombreCompleto AS NombreInstructor,
    u.Carrera,
    u.Campus,
    u.Activo,
    u.CorreoElectronico
FROM Usuarios u
JOIN Roles r ON u.IdRol = r.IdRol
WHERE r.NombreRol = 'Instructor';
GO

-- Cursos con cupos
CREATE VIEW uv_ListarCursosConCupos AS
SELECT 
    c.IdCurso,
    c.NombreCurso,
    c.Descripcion,
    c.FechaInicio,
    c.FechaFin,
    c.Horario,
    c.CupoMaximo,
    c.Estado,
    inst.NombreCompleto AS NombreInstructor,
    COUNT(CASE WHEN i.EstadoInscripcion = 'Confirmada' THEN 1 END) AS CuposOcupados,
    c.CupoMaximo - COUNT(CASE WHEN i.EstadoInscripcion = 'Confirmada' THEN 1 END) AS CuposDisponibles
FROM Cursos c
LEFT JOIN Usuarios inst ON c.CarnetInstructor = inst.Carnet
LEFT JOIN InscripcionCursos i ON c.IdCurso = i.IdCurso
GROUP BY c.IdCurso, c.NombreCurso, c.Descripcion, c.FechaInicio, c.FechaFin, c.Horario, c.CupoMaximo, c.Estado, inst.NombreCompleto;
GO

-- Procedimientos adicionales
GO

-- Crea rol
CREATE PROCEDURE dbo.sp_CrearRol
    @NombreRol VARCHAR(50),
    @Descripcion VARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM Roles WHERE NombreRol = @NombreRol)
            BEGIN
                ;THROW 50007, 'El rol ya existe en el sistema.', 1;
            END
            
        INSERT INTO Roles (NombreRol, Descripcion) 
        VALUES (@NombreRol, @Descripcion);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Actualiza usuario
CREATE PROCEDURE dbo.sp_ActualizarUsuario
    @Carnet VARCHAR(10),
    @NombreCompleto VARCHAR(100),
    @Carrera VARCHAR(150),
    @Campus VARCHAR(100),
    @Activo BIT,
    @CorreoElectronico VARCHAR(50),
    @NombreRol VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Carnet = @Carnet)
            BEGIN
                ;THROW 50008, 'El usuario con el carnet especificado no existe.', 1;
            END
            
        DECLARE @IdRol INT = NULL;
        SELECT @IdRol = IdRol FROM Roles WHERE NombreRol = @NombreRol;
        IF @IdRol IS NULL
            BEGIN
                ;THROW 50009, 'El rol especificado no existe.', 1;
            END
            
        UPDATE Usuarios
        SET NombreCompleto = @NombreCompleto, 
            Carrera = @Carrera, 
            Campus = @Campus,
            Activo = @Activo, 
            CorreoElectronico = @CorreoElectronico, 
            IdRol = @IdRol
        WHERE Carnet = @Carnet;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Crea asociación
CREATE PROCEDURE dbo.sp_CrearAsociacion
    @Acronimo VARCHAR(20),
    @Nombre VARCHAR(200),
    @AnioFundacion INT,
    @Descripcion NVARCHAR(MAX),
    @Formulario NVARCHAR(MAX),
    @ImgUrl NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM Asociaciones WHERE Acronimo = @Acronimo)
            BEGIN
                ;THROW 50010, 'Ya existe una asociación con ese acrónimo.', 1;
            END
            
        INSERT INTO Asociaciones (Acronimo, Nombre, AnioFundacion, Descripcion, Formulario, ImgUrl)
        VALUES (@Acronimo, @Nombre, @AnioFundacion, @Descripcion, @Formulario, @ImgUrl);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Crea curso
CREATE PROCEDURE dbo.sp_CrearCurso
    @NombreCurso VARCHAR(150),
    @Descripcion NVARCHAR(500),
    @FechaInicio DATE,
    @FechaFin DATE,
    @Horario VARCHAR(100),
    @CupoMaximo INT,
    @CarnetInstructor VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            
        INSERT INTO Cursos (NombreCurso, Descripcion, FechaInicio, FechaFin, Horario, CupoMaximo, Estado, CarnetInstructor)
        VALUES (@NombreCurso, @Descripcion, @FechaInicio, @FechaFin, @Horario, @CupoMaximo, 'Activo', @CarnetInstructor);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Actualiza curso
CREATE PROCEDURE dbo.sp_ActualizarCurso
    @IdCurso INT,
    @NombreCurso VARCHAR(150),
    @Descripcion NVARCHAR(500),
    @FechaInicio DATE,
    @FechaFin DATE,
    @Horario VARCHAR(100),
    @CupoMaximo INT,
    @Estado VARCHAR(20),
    @CarnetInstructor VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM Cursos WHERE IdCurso = @IdCurso)
            BEGIN
                ;THROW 50012, 'El curso especificado no existe.', 1;
            END
            
        UPDATE Cursos
        SET NombreCurso = @NombreCurso, 
            Descripcion = @Descripcion, 
            FechaInicio = @FechaInicio,
            FechaFin = @FechaFin, 
            Horario = @Horario, 
            CupoMaximo = @CupoMaximo, 
            Estado = @Estado,
            CarnetInstructor = @CarnetInstructor
        WHERE IdCurso = @IdCurso;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Crea selección
CREATE PROCEDURE dbo.sp_CrearSeleccionDeportiva
    @Disciplina VARCHAR(50),
    @Rama VARCHAR(20),
    @NombreEquipo VARCHAR(100),
    @ImgUrl NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM SeleccionesDeportivas WHERE NombreEquipo = @NombreEquipo)
            BEGIN
                ;THROW 50013, 'El nombre de equipo deportivo ya se encuentra registrado.', 1;
            END
            
        INSERT INTO SeleccionesDeportivas (Disciplina, Rama, NombreEquipo, ImgUrl)
        VALUES (@Disciplina, @Rama, @NombreEquipo, @ImgUrl);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Crea categoría
CREATE PROCEDURE dbo.sp_CrearCategoriaDAC
    @NombreCategoria VARCHAR(100),
    @Descripcion NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM CategoriasDAC WHERE NombreCategoria = @NombreCategoria)
            BEGIN
                ;THROW 50014, 'Ya existe una categoría artística con ese nombre.', 1;
            END
            
        INSERT INTO CategoriasDAC (NombreCategoria, Descripcion)
        VALUES (@NombreCategoria, @Descripcion);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Crea actividad
CREATE PROCEDURE dbo.sp_CrearActividadDAC
    @IdCategoria INT,
    @NombreActividad VARCHAR(150),
    @Descripcion NVARCHAR(MAX),
    @CarnetInstructor VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM CategoriasDAC WHERE IdCategoria = @IdCategoria)
            BEGIN
                ;THROW 50015, 'La categoría DAC especificada no existe.', 1;
            END
            
        INSERT INTO ActividadesDAC (IdCategoria, NombreActividad, Descripcion, Estado, CarnetInstructor)
        VALUES (@IdCategoria, @NombreActividad, @Descripcion, 'Activo', @CarnetInstructor);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Inscribe asociación
CREATE PROCEDURE dbo.sp_InscribirMembresiaAsociacion
    @Carnet VARCHAR(10),
    @IdAsociacion INT,
    @Rol VARCHAR(50) = 'Miembro'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Carnet = @Carnet)
            BEGIN
                ;THROW 50016, 'El carnet ingresado no existe en el sistema.', 1;
            END
        IF NOT EXISTS (SELECT 1 FROM Asociaciones WHERE IdAsociacion = @IdAsociacion)
            BEGIN
                ;THROW 50017, 'La asociación especificada no existe.', 1;
            END
            
        DECLARE @EstadoActual VARCHAR(20) = NULL;
        DECLARE @IdMembresia INT = NULL;
        SELECT @IdMembresia = IdMembresia, @EstadoActual = EstadoValidacion 
        FROM MembresiaAsociaciones WHERE Carnet = @Carnet AND IdAsociacion = @IdAsociacion;
        
        IF @EstadoActual IS NOT NULL
        BEGIN
            IF @EstadoActual IN ('Pendiente', 'Aprobada')
                BEGIN
                    ;THROW 50018, 'El estudiante ya cuenta con una solicitud activa o membresía en esta asociación.', 1;
                END
            IF @EstadoActual = 'Rechazada'
            BEGIN
                UPDATE MembresiaAsociaciones
                SET EstadoValidacion = 'Pendiente', Rol = COALESCE(@Rol, 'Miembro'), FechaSolicitud = GETDATE()
                WHERE IdMembresia = @IdMembresia;
            END
        END
        ELSE
        BEGIN
            INSERT INTO MembresiaAsociaciones (Carnet, IdAsociacion, Rol, EstadoValidacion)
            VALUES (@Carnet, @IdAsociacion, COALESCE(@Rol, 'Miembro'), 'Pendiente');
        END
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Cancela asociación
CREATE PROCEDURE dbo.sp_CancelarMembresiaAsociacion
    @Carnet VARCHAR(10),
    @IdAsociacion INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM MembresiaAsociaciones WHERE Carnet = @Carnet AND IdAsociacion = @IdAsociacion AND EstadoValidacion IN ('Pendiente', 'Aprobada'))
            BEGIN
                ;THROW 50019, 'No existe una membresía activa o pendiente para este estudiante en la asociación.', 1;
            END
            
        UPDATE MembresiaAsociaciones
        SET EstadoValidacion = 'Rechazada'
        WHERE Carnet = @Carnet AND IdAsociacion = @IdAsociacion;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Inscribe selección
CREATE PROCEDURE dbo.sp_InscribirParticipacionDeportiva
    @Carnet VARCHAR(10),
    @IdSeleccion INT,
    @Modalidad VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Carnet = @Carnet)
            BEGIN
                ;THROW 50020, 'El carnet del estudiante no existe.', 1;
            END
        IF NOT EXISTS (SELECT 1 FROM SeleccionesDeportivas WHERE IdSeleccion = @IdSeleccion)
            BEGIN
                ;THROW 50021, 'La selección deportiva especificada no existe.', 1;
            END
        IF EXISTS (SELECT 1 FROM ParticipacionDeportiva WHERE Carnet = @Carnet AND IdSeleccion = @IdSeleccion)
            BEGIN
                ;THROW 50022, 'El estudiante ya participa en esta selección deportiva.', 1;
            END
            
        INSERT INTO ParticipacionDeportiva (Carnet, IdSeleccion, Modalidad)
        VALUES (@Carnet, @IdSeleccion, @Modalidad);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Cancela selección
CREATE PROCEDURE dbo.sp_CancelarParticipacionDeportiva
    @Carnet VARCHAR(10),
    @IdSeleccion INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM ParticipacionDeportiva WHERE Carnet = @Carnet AND IdSeleccion = @IdSeleccion)
            BEGIN
                ;THROW 50023, 'El estudiante no tiene registrada participación en esta selección deportiva.', 1;
            END
            
        DELETE FROM ParticipacionDeportiva
        WHERE Carnet = @Carnet AND IdSeleccion = @IdSeleccion;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Inscribe actividad
CREATE PROCEDURE dbo.sp_InscribirActividadDAC
    @Carnet VARCHAR(10),
    @IdActividad INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE Carnet = @Carnet)
            BEGIN
                ;THROW 50024, 'El carnet del estudiante no existe.', 1;
            END
        IF NOT EXISTS (SELECT 1 FROM ActividadesDAC WHERE IdActividad = @IdActividad)
            BEGIN
                ;THROW 50025, 'La actividad artística (DAC) especificada no existe.', 1;
            END
            
        DECLARE @EstadoActual VARCHAR(20) = NULL;
        DECLARE @IdInscripcionDAC INT = NULL;
        SELECT @IdInscripcionDAC = IdInscripcionDAC, @EstadoActual = EstadoInscripcion
        FROM InscripcionesDAC WHERE Carnet = @Carnet AND IdActividad = @IdActividad;
        
        IF @EstadoActual IS NOT NULL
        BEGIN
            IF @EstadoActual IN ('Activa', 'Finalizada')
                BEGIN
                    ;THROW 50026, 'El estudiante ya cuenta con una inscripción activa o finalizada para esta actividad.', 1;
                END
            IF @EstadoActual = 'Retirada'
            BEGIN
                UPDATE InscripcionesDAC
                SET EstadoInscripcion = 'Activa', FechaInscripcion = GETDATE()
                WHERE IdInscripcionDAC = @IdInscripcionDAC;
            END
        END
        ELSE
        BEGIN
            INSERT INTO InscripcionesDAC (Carnet, IdActividad)
            VALUES (@Carnet, @IdActividad);
        END
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO

-- Cancela actividad
CREATE PROCEDURE dbo.sp_CancelarInscripcionDAC
    @Carnet VARCHAR(10),
    @IdActividad INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM InscripcionesDAC WHERE Carnet = @Carnet AND IdActividad = @IdActividad AND EstadoInscripcion = 'Activa')
            BEGIN
                ;THROW 50027, 'No existe una inscripción activa para este estudiante en la actividad artística.', 1;
            END
            
        UPDATE InscripcionesDAC
        SET EstadoInscripcion = 'Retirada'
        WHERE Carnet = @Carnet AND IdActividad = @IdActividad;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        ;THROW;
    END CATCH
END;
GO