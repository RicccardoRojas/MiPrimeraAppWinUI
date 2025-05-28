using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesPaleteria;
using Microsoft.Data.Sqlite;

namespace ManejadorPeleteria
{
    public class ManejadorInventarioProductos
    {
        public static List<Producto> ObtenerInvenProduct()
        {
            var productos = new List<Producto>();

            using var connection = new SqliteConnection(Database.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Nombre, Precio FROM Productos";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                productos.Add(new Producto
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Precio = reader.GetDouble(2)
                });
            }

            return productos;
        }
    }
}
