-- ************************************************************
-- 1. CREACIÓN DE LA BASE DE DATOS Y SELECCIÓN
-- ************************************************************

CREATE DATABASE BibliotecaMetropolis;
GO

USE BibliotecaMetropolis;
GO


-- ************************************************************
-- 2. CREACIÓN DE TABLAS PRINCIPALES
-- Se usa IDENTITY(1,1) para el autoincremento.
-- ************************************************************

CREATE TABLE Pais (
    IdPais INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(80) NOT NULL
);
GO

CREATE TABLE TipoRecurso (
    idTipoR INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(50) NOT NULL,
    descripcion NVARCHAR(255) NULL
);
GO

CREATE TABLE Editorial (
    IdEdit INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(500) NULL
);
GO

CREATE TABLE Autor (
    IdAutor INT IDENTITY(1,1) PRIMARY KEY,
    nombres NVARCHAR(100) NOT NULL,
    apellidos NVARCHAR(100) NOT NULL
);
GO


CREATE TABLE Recurso (
    -- Se usa IdRecurso para consistencia con los modelos C#
    IdRecurso INT IDENTITY(1,1) PRIMARY KEY, 
    titulo NVARCHAR(200) NOT NULL,
    annopublic INT NULL,
    edicion NVARCHAR(50) NULL,
    descripcion NVARCHAR(1000) NULL,
    palabrasbusqueda NVARCHAR(500) NULL,
    
    -- Claves Foráneas (Se definen en la sección 3)
    IdPais INT NULL,
    idTipoR INT NOT NULL,
    IdEdit INT NOT NULL
);
GO

-- ************************************************************
-- 3. CREACIÓN DE TABLAS DE RELACIÓN (M:N)
-- ************************************************************

CREATE TABLE PalabraClave (
    IdPalabraClave INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);
GO

-- Se usa RecursoAutor para coincidir con la entidad de unión C#
CREATE TABLE RecursoAutor (
    IdRecurso INT NOT NULL,
    IdAutor INT NOT NULL,
    EsPrincipal BIT NOT NULL DEFAULT 0, -- BIT es el tipo de dato booleano en SQL Server
    PRIMARY KEY (IdRecurso, IdAutor)
);
GO

CREATE TABLE RecursoPalabraClave (
    IdRecurso INT NOT NULL,
    IdPalabraClave INT NOT NULL,
    PRIMARY KEY (IdRecurso, IdPalabraClave)
);
GO

-- ************************************************************
-- 4. CLAVES FORÁNEAS (RELACIONES)
-- NOTA: Se recomienda activar CASCADE DELETE en producción para tablas M:N.
-- ************************************************************

-- Relaciones para la tabla Recurso
ALTER TABLE Recurso
    ADD CONSTRAINT FK_Recurso_Pais
        FOREIGN KEY (IdPais) REFERENCES Pais(IdPais),
    CONSTRAINT FK_Recurso_TipoRecurso
        FOREIGN KEY (idTipoR) REFERENCES TipoRecurso(idTipoR),
    CONSTRAINT FK_Recurso_Editorial
        FOREIGN KEY (IdEdit) REFERENCES Editorial(IdEdit);
GO

-- Relaciones para la tabla RecursoAutor
ALTER TABLE RecursoAutor
    ADD CONSTRAINT FK_RecursoAutor_Recurso
        FOREIGN KEY (IdRecurso) REFERENCES Recurso(IdRecurso),
    CONSTRAINT FK_RecursoAutor_Autor
        FOREIGN KEY (IdAutor) REFERENCES Autor(IdAutor);
GO

-- Relaciones para la tabla RecursoPalabraClave
ALTER TABLE RecursoPalabraClave
    ADD CONSTRAINT FK_RPK_Recurso
        FOREIGN KEY (IdRecurso) REFERENCES Recurso(IdRecurso),
    CONSTRAINT FK_RPK_PalabraClave
        FOREIGN KEY (IdPalabraClave) REFERENCES PalabraClave(IdPalabraClave);
GO

-- ************************************************************
-- 5. REGISTROS INICIALES (INSERTS)
-- ************************************************************

-- 5.1. Datos de PAIS
SET IDENTITY_INSERT Pais ON;
INSERT INTO Pais (IdPais, nombre) VALUES
(1, N'El Salvador'),
(2, N'México'),
(3, N'España'),
(4, N'Colombia');
SET IDENTITY_INSERT Pais OFF;
GO

-- 5.2. Datos de TIPO RECURSO
SET IDENTITY_INSERT TipoRecurso ON;
INSERT INTO TipoRecurso (idTipoR, nombre, descripcion) VALUES
(1, N'Libro', N'Material de tapa dura o blanda con ISBN.'),
(2, N'Revista', N'Publicación periódica.'),
(3, N'Tesis', N'Documento de investigación académica.'),
(4, N'Audiovisual', N'CD, DVD, o contenido digital.'),
(5, N'Recurso Digital', N'eBook, artículo online con DOI.');
SET IDENTITY_INSERT TipoRecurso OFF;
GO

-- 5.3. Datos de EDITORIAL
SET IDENTITY_INSERT Editorial ON;
INSERT INTO Editorial (IdEdit, Nombre, descripcion) VALUES
(1, N'Planeta', N'Editorial líder en ficción y no ficción.'),
(2, N'Oxford University Press', N'Editorial académica internacional.'),
(3, N'Pearson', N'Especializada en libros de texto y educación superior.'),
(4, N'Siglo XXI Editores', N'Reconocida en ciencias sociales y humanidades.'),
(5, N'Anagrama', N'Editorial española de literatura contemporánea.');
SET IDENTITY_INSERT Editorial OFF;
GO

-- 5.4. Datos de AUTOR
SET IDENTITY_INSERT Autor ON;
INSERT INTO Autor (IdAutor, nombres, apellidos) VALUES
(1, N'Ludy', N'Chino Tlavieso')
SET IDENTITY_INSERT Autor OFF;
GO