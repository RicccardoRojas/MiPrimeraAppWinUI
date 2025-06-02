using Microsoft.Data.Sqlite;
using EntidadPeleteria;
using AccesosDatos;
using System.Collections.ObjectModel;
namespace ManejadoresPaleteria
{
    public class ManejadorInventarioProducto
    {
        #region Productos
        public  ObservableCollection<Productos> ObtenerProductos(string filtro)
        {
            try
            {
                var productos = new ObservableCollection<Productos>();

                using var connection = new SqliteConnection(AccesoBD.ConnectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM VistaProductos WHERE Nombre LIKE $nombre";
                command.Parameters.AddWithValue("$nombre", filtro);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    productos.Add(new Productos
                    {
                        Id = reader.GetInt32(0),
                        RutaIcono = reader.GetString(1),
                        Producto = reader.GetString(2).ToUpper(),
                        Cantidad = reader.GetInt32(3),
                        Caducidad = reader.GetString(4),
                        Precio = reader.GetDouble(5),
                        Sabor = reader.GetString(6),
                    });
                }

                return productos;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public List<Productos> ObtenerProductosCompra(int id)
        {
            try
            {
                var productos = new List<Productos>();

                using var connection = new SqliteConnection(AccesoBD.ConnectionString);
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM InventarioProductos WHERE id = $id;";
                command.Parameters.AddWithValue("id", id);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    productos.Add(new Productos
                    {
                        Id = reader.GetInt32(0),
                        RutaIcono = reader.GetString(1),
                        Producto = reader.GetString(2).ToUpper(),
                        Cantidad = reader.GetInt32(3),
                        Caducidad = reader.GetString(4),
                        Precio = reader.GetDouble(5),
                        IDTIPSabor = reader.GetInt32(6),
                    });
                }

                return productos;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public  void InsertarProductos(Productos producto)
        {
            try
            {
                using var connection = new SqliteConnection(AccesoBD.ConnectionString);
                connection.Open();

                var command = connection.CreateCommand();

                if(producto.Id == -1)
                {
                    command.CommandText = @"INSERT INTO InventarioProductos (Icono,Nombre,Cantidad,Caducidad,Precio,FKIDTipoSabor)
                    VALUES ($icono,$nombre,$cantidad,$caducidad,$precio,$fkidtiposabor);";

                    command.Parameters.AddWithValue("$icono", producto.RutaIcono);
                    command.Parameters.AddWithValue("$nombre", producto.Producto);
                    command.Parameters.AddWithValue("$cantidad", producto.Cantidad);
                    command.Parameters.AddWithValue("$caducidad", producto.Caducidad);
                    command.Parameters.AddWithValue("$precio", producto.Precio);
                    command.Parameters.AddWithValue("$fkidtiposabor", producto.IDTIPSabor);

                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"UPDATE InventarioProductos SET Icono = $icono, Nombre = $nombre, Cantidad = $cantidad, Caducidad = $caducidad, Precio = $precio, FKIDTipoSabor = $fkidtiposabor WHERE ID = $id;";
                    command.Parameters.AddWithValue("$icono", producto.RutaIcono);
                    command.Parameters.AddWithValue("$nombre", producto.Producto);
                    command.Parameters.AddWithValue("$cantidad", producto.Cantidad);
                    command.Parameters.AddWithValue("$caducidad", producto.Caducidad);
                    command.Parameters.AddWithValue("$precio", producto.Precio);
                    command.Parameters.AddWithValue("$fkidtiposabor", producto.IDTIPSabor);
                    command.Parameters.AddWithValue("$id", producto.Id);
                    command.ExecuteNonQuery();
                }

                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool EliminarProductos(int id)
        {
            try
            {
                using var connection = new SqliteConnection(AccesoBD.ConnectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM InventarioProductos WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #endregion

        #region TiposSabores
        public List<TiposSabores> ObtenerTipoSabores()
        {
            try
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
            catch (Exception)
            {

                throw;
            }
            
        }
        public void InsertarTipoSabores(TiposSabores producto)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
            
        }
        #endregion

        #region Sabores
        public ObservableCollection<Sabores> ObtenerSabores(int fkidtiposabor)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
            
        }
        public List<Sabores> ObtenerSaboresLista(int fkidtiposabor)
        {
            try
            {
                var sabores = new List<Sabores>();

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
            catch (Exception)
            {
                throw;
            }

        }

        public void InserEditSabores(Sabores producto)
        {
            try
            {
                using var connection = new SqliteConnection(AccesoBD.ConnectionString);
                connection.Open();

                var command = connection.CreateCommand();

                if (producto.ID == -1)
                {
                    command.CommandText = @"INSERT INTO Sabores (Sabor,FKIDTipoSabor) VALUES ($sabor,$fkid);";
                    command.Parameters.AddWithValue("$sabor", producto.Sabor);
                    command.Parameters.AddWithValue("$fkid", producto.FKIDTipoSabor);

                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = @"UPDATE Sabores SET Sabor = $sabor WHERE ID = $id;";
                    command.Parameters.AddWithValue("$sabor", producto.Sabor);
                    command.Parameters.AddWithValue("$id", producto.ID);
                    command.ExecuteNonQuery();
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public void EliminarSabores(int id)
        {
            try
            {
                using var connection = new SqliteConnection(AccesoBD.ConnectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Sabores WHERE ID = @id";
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
    }
}
