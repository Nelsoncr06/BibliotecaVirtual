/*
    Script para preparar la tabla y los datos de prueba
    de la aplicación BibliotecaMVC_BD.

    IMPORTANTE:
    Antes de ejecutarlo, selecciona la base de datos de destino.
    En desarrollo local, esa base de datos es BibliotecaDB.

    Este script está pensado para una base de datos nueva.
*/

CREATE TABLE dbo.Libros
(
    IdLibro INT IDENTITY(1,1) NOT NULL,
    Codigo NVARCHAR(10) NOT NULL,
    Titulo NVARCHAR(100) NOT NULL,
    Autor NVARCHAR(50) NOT NULL,
    FechaPublicacion DATE NOT NULL,

    CONSTRAINT PK_Libros PRIMARY KEY (IdLibro),
    CONSTRAINT UQ_Libros_Codigo UNIQUE (Codigo)
);
GO

INSERT INTO dbo.Libros (Codigo, Titulo, Autor, FechaPublicacion)
VALUES
    (N'LIB-001', N'Don Quijote de la Mancha', N'Miguel de Cervantes', '1605-01-16'),
    (N'LIB-002', N'Cien años de soledad', N'Gabriel García Márquez', '1967-06-05'),
    (N'LIB-003', N'El principito', N'Antoine de Saint-Exupéry', '1943-04-06'),
    (N'LIB-004', N'La casa de los espíritus', N'Isabel Allende', '1982-01-01'),
    (N'LIB-005', N'Crónica de una muerte anunciada', N'Gabriel García Márquez', '1981-01-01'),
    (N'LIB-006', N'El amor en los tiempos del cólera', N'Gabriel García Márquez', '1985-01-01'),
    (N'LIB-007', N'Orgullo y prejuicio', N'Jane Austen', '1813-01-28'),
    (N'LIB-008', N'La isla del tesoro', N'Robert Louis Stevenson', '1883-01-01'),
    (N'LIB-009', N'El túnel', N'Ernesto Sabato', '1948-01-01'),
    (N'LIB-010', N'La metamorfosis', N'Franz Kafka', '1915-01-01');
GO