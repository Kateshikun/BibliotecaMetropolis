CREATE DATABASE BibliotecaMetropolis;
GO

USE BibliotecaMetropolis;
GO


-- TABLAS PRINCIPALES


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
    IdRec INT IDENTITY(1,1) PRIMARY KEY,
    titulo NVARCHAR(200) NOT NULL,
    annopublic INT NULL,
    edicion NVARCHAR(50) NULL,
    descripcion NVARCHAR(1000) NULL,
    palabrasbusqueda NVARCHAR(500) NULL,
    -- Claves Foráneas
    IdPais INT NULL,
    idTipoR INT NOT NULL,
    IdEdit INT NOT NULL
);
GO

-- TABLAS DE RELACIÓN (M:N) 
-- Estas tablas sirven para relacionar otras tablas

CREATE TABLE PalabraClave ( --Tabla para realizar las busquedas mas facilmente
    IdPalabraClave INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE AutoresRecursos (
    IdRec INT NOT NULL,
    IdAutor INT NOT NULL,
    EsPrincipal BIT NOT NULL DEFAULT 0,
    PRIMARY KEY (IdRec, IdAutor)
);
GO

CREATE TABLE RecursoPalabraClave (
    IdRecurso INT NOT NULL,
    IdPalabraClave INT NOT NULL,
    PRIMARY KEY (IdRecurso, IdPalabraClave)
);
GO

-- CLAVES FORÁNEAS (RELACIONES)

ALTER TABLE Recurso
  ADD CONSTRAINT FK_Recurso_Pais
      FOREIGN KEY (IdPais) REFERENCES Pais(IdPais),
      CONSTRAINT FK_Recurso_TipoRecurso
      FOREIGN KEY (idTipoR) REFERENCES TipoRecurso(idTipoR),
      CONSTRAINT FK_Recurso_Editorial
      FOREIGN KEY (IdEdit) REFERENCES Editorial(IdEdit);
GO

ALTER TABLE AutoresRecursos
  ADD CONSTRAINT FK_AutoresRecursos_Recurso
      FOREIGN KEY (IdRec) REFERENCES Recurso(IdRec),
      CONSTRAINT FK_AutoresRecursos_Autor
      FOREIGN KEY (IdAutor) REFERENCES Autor(IdAutor);
GO

ALTER TABLE RecursoPalabraClave
  ADD CONSTRAINT FK_RPK_Recurso
      FOREIGN KEY (IdRecurso) REFERENCES Recurso(IdRec),
      CONSTRAINT FK_RPK_PalabraClave
      FOREIGN KEY (IdPalabraClave) REFERENCES PalabraClave(IdPalabraClave);
GO
