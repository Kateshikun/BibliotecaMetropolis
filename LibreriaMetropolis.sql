CREATE DATABASE BibliotecaMetropolis;
GO
USE BibliotecaMetropolis;
GO


-- PAÍS
CREATE TABLE Pais (
    IdPais      INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(80)  NOT NULL,
    CodigoISO   NVARCHAR(3)   NULL  -- opcional (ej. SV, US, CO). Déjalo si el diagrama lo contempla.
);
GO

-- TIPO DE RECURSO
CREATE TABLE TipoRecurso (
    IdTipoRecurso INT IDENTITY(1,1) PRIMARY KEY,
    Nombre        NVARCHAR(50) NOT NULL
);
GO

-- PALABRA CLAVE
CREATE TABLE PalabraClave (
    IdPalabraClave INT IDENTITY(1,1) PRIMARY KEY,
    Nombre         NVARCHAR(100) NOT NULL
);
GO

-- AUTOR
CREATE TABLE Autor (
    IdAutor    INT IDENTITY(1,1) PRIMARY KEY,
    Nombres    NVARCHAR(100) NOT NULL,
    Apellidos  NVARCHAR(100) NOT NULL,
    IdPais     INT NULL,               -- <- FK a País (en lugar de texto)
    Email      NVARCHAR(100) NULL
);
GO

-- EDITORIAL / INSTITUCIÓN
CREATE TABLE Editorial (
    IdEditorial INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(100) NOT NULL,
    IdPais      INT NULL,              -- <- FK a País
    Ciudad      NVARCHAR(50)  NULL
);
GO

-- RECURSO
CREATE TABLE Recurso (
    IdRecurso       INT IDENTITY(1,1) PRIMARY KEY,
    Titulo          NVARCHAR(200) NOT NULL,
    IdTipoRecurso   INT NOT NULL,      -- FK a TipoRecurso
    IdEditorial     INT NOT NULL,      -- FK a Editorial
    AnioPublicacion INT NULL,
    IdPais          INT NULL,          -- <- FK a País (en lugar de texto)
    Ciudad          NVARCHAR(50)  NULL,
    Cantidad        INT NULL,
    Precio          DECIMAL(10,2) NULL
);
GO


-- RecursoAutor (M:N)
CREATE TABLE RecursoAutor (
    IdRecurso INT NOT NULL,
    IdAutor   INT NOT NULL,
    PRIMARY KEY (IdRecurso, IdAutor)
);
GO

-- RecursoPalabraClave (M:N)
CREATE TABLE RecursoPalabraClave (
    IdRecurso      INT NOT NULL,
    IdPalabraClave INT NOT NULL,
    PRIMARY KEY (IdRecurso, IdPalabraClave)
);
GO



ALTER TABLE Autor
  ADD CONSTRAINT FK_Autor_Pais
      FOREIGN KEY (IdPais) REFERENCES Pais(IdPais);
GO

ALTER TABLE Editorial
  ADD CONSTRAINT FK_Editorial_Pais
      FOREIGN KEY (IdPais) REFERENCES Pais(IdPais);
GO


ALTER TABLE Recurso
  ADD CONSTRAINT FK_Recurso_TipoRecurso
      FOREIGN KEY (IdTipoRecurso) REFERENCES TipoRecurso(IdTipoRecurso),
      CONSTRAINT FK_Recurso_Editorial
      FOREIGN KEY (IdEditorial)   REFERENCES Editorial(IdEditorial),
      CONSTRAINT FK_Recurso_Pais
      FOREIGN KEY (IdPais)        REFERENCES Pais(IdPais);
GO


ALTER TABLE RecursoAutor
  ADD CONSTRAINT FK_RecursoAutor_Recurso
      FOREIGN KEY (IdRecurso) REFERENCES Recurso(IdRecurso),
      CONSTRAINT FK_RecursoAutor_Autor
      FOREIGN KEY (IdAutor)   REFERENCES Autor(IdAutor);
GO


ALTER TABLE RecursoPalabraClave
  ADD CONSTRAINT FK_RPK_Recurso
      FOREIGN KEY (IdRecurso)      REFERENCES Recurso(IdRecurso),
      CONSTRAINT FK_RPK_PalabraClave
      FOREIGN KEY (IdPalabraClave) REFERENCES PalabraClave(IdPalabraClave);
GO