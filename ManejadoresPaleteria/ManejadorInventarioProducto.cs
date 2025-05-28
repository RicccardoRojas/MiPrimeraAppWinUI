using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using EntidadPeleteria;
using AccesosDatos;
using System.Collections.ObjectModel;
namespace ManejadoresPaleteria
{
    public class ManejadorInventarioProducto
    {
        #region Productos
        public  List<Productos> ObtenerProductos()
        {
            var productos = new List<Productos>();

            using var connection = new SqliteConnection(AccesoBD.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM InventarioProductos";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                productos.Add(new Productos
                {
                    Id = reader.GetInt32(0),
                    RutaIcono = reader.GetString(1),
                    Producto = reader.GetString(2),
                    Cantidad = reader.GetInt32(3),
                    Caducidad = reader.GetString(4),
                    IDTIPSabor = reader.GetInt32(5),
                    Precio = reader.GetString(6),
                });
            }

            return productos;
        }
        public  void InsertarProductos(Productos producto)
        {
            using var connection = new SqliteConnection(AccesoBD.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO InventarioProductos (Icono,Producto,Cantidad,Caducidad,Precio,FKIDTipoSabor)
                VALUES ($icono,$producto,$cantidad,$precio,$fkidtiposabor);
            ";

            command.Parameters.AddWithValue("$icono", producto.RutaIcono);
            command.Parameters.AddWithValue("$producto", producto.Producto);
            command.Parameters.AddWithValue("$cantidad", producto.Cantidad);
            command.Parameters.AddWithValue("$precio", producto.Precio);
            command.Parameters.AddWithValue("$fkidtiposabor", producto.IDTIPSabor);

            command.ExecuteNonQuery();
        }
        #endregion

        #region TiposSabores
        public List<TiposSabores> ObtenerTipoSabores()
        {
            var sabores = new List<TiposSabores>();

            using var connection = new SqliteConnection(AccesoBD.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT ID, TipoSabores FROM TipoSabor";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                sabores.Add(new TiposSabores
                {
                    Id = reader.GetInt32(0),
                    Sabor = reader.GetString(1)
                });
            }

            return sabores;
        }
        public void InsertarTipoSabores(TiposSabores producto)
        {
            using var connection = new SqliteConnection(AccesoBD.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO TipoSabor (TipoSabores)
                VALUES ($tiposabor);
            ";

            command.Parameters.AddWithValue("$tiposabor", producto.Sabor);

            command.ExecuteNonQuery();
        }
        #endregion

        #region Sabores
        public ObservableCollection<Sabores> ObtenerSabores(int fkidtiposabor)
        {
            var sabores = new ObservableCollection<Sabores>();

            using var connection = new SqliteConnection(AccesoBD.ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT ID, Sabor FROM Sabores WHERE FKIDTipoSabor = @fkid";
            command.Parameters.AddWithValue("@fkid", fkidtiposabor);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                sabores.Add(new Sabores
                {
                    ID = reader.GetInt32(0),
                    Sabor = reader.GetString(1)
                });
            }

            return sabores;
        }

        #endregion
    }
}
