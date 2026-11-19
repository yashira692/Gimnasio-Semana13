-- =============================================
-- Base de datos: GimnasioDB  (SEMANA13 + SEMANA14: usuarios y JWT)
-- Tema: Gimnasio - Planes de membresia y Socios (1:N)
-- =============================================
IF DB_ID('GimnasioDB') IS NOT NULL
BEGIN
    ALTER DATABASE GimnasioDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE GimnasioDB;
END
GO
CREATE DATABASE GimnasioDB;
GO
USE GimnasioDB;
GO

CREATE TABLE [Plan] (
    PlanId         INT IDENTITY(1,1) CONSTRAINT PK_Plan PRIMARY KEY,
    Nombre         NVARCHAR(50)  NOT NULL CONSTRAINT UQ_Plan_Nombre UNIQUE,
    Descripcion    NVARCHAR(200) NULL,
    PrecioMensual  DECIMAL(10,2) NOT NULL,
    DuracionMeses  INT           NOT NULL,
    Activo         BIT           NOT NULL
);
GO

CREATE TABLE Socio (
    SocioId           INT IDENTITY(1,1) CONSTRAINT PK_Socio PRIMARY KEY,
    Nombres           NVARCHAR(60)  NOT NULL,
    Apellidos         NVARCHAR(60)  NOT NULL,
    Dni               CHAR(8)       NOT NULL CONSTRAINT UQ_Socio_Dni UNIQUE,
    Email             NVARCHAR(100) NULL,
    FechaInscripcion  DATE          NOT NULL,
    Activo            BIT           NOT NULL,
    PlanId            INT           NOT NULL
        CONSTRAINT FK_Socio_Plan FOREIGN KEY REFERENCES [Plan](PlanId)
);
GO

INSERT INTO [Plan] (Nombre, Descripcion, PrecioMensual, DuracionMeses, Activo) VALUES
(N'Básico',   N'Acceso a sala de máquinas en horario regular', 79.90, 1, 1),
(N'Premium',  N'Sala de máquinas + clases grupales',          129.90, 3, 1),
(N'Black',    N'Acceso total 24/7, clases y nutricionista',   199.90, 12, 1),
(N'Estudiante', N'Plan antiguo con descuento (ya no se vende)', 59.90, 1, 0);
GO

INSERT INTO Socio (Nombres, Apellidos, Dni, Email, FechaInscripcion, Activo, PlanId) VALUES
(N'Lucía',    N'Ramírez Torres',  '71234567', 'lucia.ramirez@gmail.com',  '2026-08-01', 1, 1),
(N'Diego',    N'Huamán Quispe',   '72345678', 'diego.huaman@gmail.com',   '2026-08-15', 1, 2),
(N'Valeria',  N'Paredes Soto',    '73456789', 'valeria.paredes@gmail.com','2026-09-03', 1, 2),
(N'Andrés',   N'Castillo Rojas',  '74567890', NULL,                       '2026-09-20', 0, 1),
(N'Camila',   N'Flores Medina',   '75678901', 'camila.flores@gmail.com',  '2026-10-05', 1, 3),
(N'Joaquín',  N'Vargas León',     '76789012', 'joaquin.vargas@gmail.com', '2026-10-28', 1, 3);
GO

-- =============================================
-- SEMANA14: usuarios del sistema (autenticación JWT)
-- Roles propios del gimnasio: Recepcionista y Gerente
-- La clave se guarda como hash BCrypt (nunca en texto)
-- =============================================
CREATE TABLE Usuario (
    UsuarioId      INT IDENTITY(1,1) CONSTRAINT PK_Usuario PRIMARY KEY,
    Correo         NVARCHAR(100) NOT NULL CONSTRAINT UQ_Usuario_Correo UNIQUE,
    ClaveHash      VARCHAR(100)  NOT NULL,
    Rol            VARCHAR(20)   NOT NULL
        CONSTRAINT CK_Usuario_Rol CHECK (Rol IN ('Recepcionista', 'Gerente')),
    FechaRegistro  DATETIME2     NOT NULL
);
GO

-- Usuarios de prueba (uno por rol). Claves de prueba:
--   gerente@gimnasio.pe    -> Gerente2026*
--   recepcion@gimnasio.pe  -> Recepcion2026*
-- Hash BCrypt (work factor 11). Toda cuenta registrada por la API nace como
-- Recepcionista; para tener un Gerente se cambia el rol con un UPDATE.
INSERT INTO Usuario (Correo, ClaveHash, Rol, FechaRegistro) VALUES
('gerente@gimnasio.pe',   '$2a$11$0yqCl4f/1J6FfkjsoCokROOcvEdKHrswO7gNnHCsYU9IIY1rFLweq', 'Recepcionista', SYSUTCDATETIME()),
('recepcion@gimnasio.pe', '$2a$11$HOjTme6xkaM6hshfNPa.l.yB8EamNM5W51BGFPpY74btG9cw4LylG', 'Recepcionista', SYSUTCDATETIME());
GO

UPDATE Usuario SET Rol = 'Gerente' WHERE Correo = 'gerente@gimnasio.pe';
GO

SELECT * FROM [Plan];
SELECT * FROM Socio;
SELECT UsuarioId, Correo, Rol FROM Usuario;
GO
