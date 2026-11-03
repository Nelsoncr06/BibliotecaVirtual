using System;
using System.Collections.Generic;
using BibliotecaMVC_BD.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BibliotecaMVC_BD.Datos
{
    // Clase para manejar las operaciones de datos relacionadas con los libros en la base de datos.
    public class LibroDatos
    {
        private readonly string _cadenaConexion;

        public LibroDatos(IConfiguration configuracion) // Constructor que recibe la configuración para inicializar la conexión a la base de datos.
        {
            _cadenaConexion = configuracion.GetConnectionString("BibliotecaDB")
                ?? throw new InvalidOperationException(
                    "No se encontró la cadena de conexión BibliotecaDB.");
        }

        public List<Libro> ObtenerTodos(string? filtro = null) // Método para obtener todos los libros de la base de datos, con un filtro opcional.
        {
            var libros = new List<Libro>();

            const string consulta = @"
        SELECT IdLibro, Codigo, Titulo, Autor, FechaPublicacion
        FROM dbo.Libros
        WHERE @Filtro = N''
           OR CHARINDEX(@Filtro, Codigo) > 0
           OR CHARINDEX(@Filtro, Titulo) > 0
           OR CHARINDEX(@Filtro, Autor) > 0
        ORDER BY Titulo;";

            using (var conexion = new SqlConnection(_cadenaConexion))
            using (var comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.Add("@Filtro", System.Data.SqlDbType.NVarChar, 100) // Parámetro para el filtro de búsqueda.
                    .Value = (filtro ?? string.Empty).Trim();

                conexion.Open(); // Abre la conexión a la base de datos.

                using (var lector = comando.ExecuteReader()) // Ejecuta la consulta y obtiene un lector de datos para recorrer los resultados.
                {
                    while (lector.Read())
                    {
                        var libro = new Libro
                        {
                            IdLibro = lector.GetInt32(0),
                            Codigo = lector.GetString(1),
                            Titulo = lector.GetString(2),
                            Autor = lector.GetString(3),
                            FechaPublicacion = lector.GetDateTime(4)
                        };

                        libros.Add(libro);
                    }
                }
            }

            return libros;
        }

        // Método para registrar un nuevo libro en la base de datos. Recibe un objeto Libro con los datos a insertar.
        public void Registrar(Libro libro) 
        {
            const string consulta = @"
        INSERT INTO dbo.Libros (Codigo, Titulo, Autor, FechaPublicacion)
        VALUES (@Codigo, @Titulo, @Autor, @FechaPublicacion);";

            using (var conexion = new SqlConnection(_cadenaConexion))
            using (var comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.Add("@Codigo", System.Data.SqlDbType.NVarChar, 10)
                    .Value = libro.Codigo.Trim();

                comando.Parameters.Add("@Titulo", System.Data.SqlDbType.NVarChar, 100)
                    .Value = libro.Titulo.Trim();

                comando.Parameters.Add("@Autor", System.Data.SqlDbType.NVarChar, 50)
                    .Value = libro.Autor.Trim();

                comando.Parameters.Add("@FechaPublicacion", System.Data.SqlDbType.Date)
                    .Value = libro.FechaPublicacion!.Value.Date;

                conexion.Open();
                comando.ExecuteNonQuery();
            }
        }
        // Método para contar el total de libros en la base de datos. Devuelve un entero con el número de registros.
        public int ContarLibros()
        {
            const string consulta = "SELECT COUNT(*) FROM dbo.Libros;";

            using (var conexion = new SqlConnection(_cadenaConexion))
            using (var comando = new SqlCommand(consulta, conexion))
            {
                conexion.Open();
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        // Método para obtener un libro por su ID. Devuelve un objeto Libro o null si no se encuentra.
        public Libro? ObtenerPorId(int id)
        {
            const string consulta = @"
        SELECT IdLibro, Codigo, Titulo, Autor, FechaPublicacion
        FROM dbo.Libros
        WHERE IdLibro = @IdLibro;";

            using (var conexion = new SqlConnection(_cadenaConexion))
            using (var comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.Add("@IdLibro", System.Data.SqlDbType.Int)
                    .Value = id;

                conexion.Open();

                using (var lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                    {
                        return null;
                    }

                    return new Libro
                    {
                        IdLibro = lector.GetInt32(0),
                        Codigo = lector.GetString(1),
                        Titulo = lector.GetString(2),
                        Autor = lector.GetString(3),
                        FechaPublicacion = lector.GetDateTime(4)
                    };
                }
            }
        }
        //El método devuelve true si se eliminó un libro y false si no se encontró el identificador.
        public bool Eliminar(int id)
        {
            const string consulta = @"
        DELETE FROM dbo.Libros
        WHERE IdLibro = @IdLibro;";

            using (var conexion = new SqlConnection(_cadenaConexion))
            using (var comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.Add("@IdLibro", System.Data.SqlDbType.Int)
                    .Value = id;

                conexion.Open();

                int filasAfectadas = comando.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
        }

    } //Fin clase libroDatos
}