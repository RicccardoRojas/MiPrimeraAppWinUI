using AccesosDatos;
using EntidadPeleteria;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManejadoresPaleteria
{
    public class ManejadorRegistroVenta
    {
        #region Registro de Venta

        public ObservableCollection<RegistroVenta> ObtenerRegistroVenta(string filtro)
        {
            try
            {
                var registro = new ObservableCollection<RegistroVenta>();

                using var connection = new SqliteConnection(AccesoBD.ConnectionString);
                connection.Open();

                var command = connection.CreateCommand();

                switch (filtro)
                {
                    case "DIARIO":
                        string fechaActual = DateTime.Now.ToString("dd-MM-yyyy");
                        command.CommandText = "SELECT * FROM RegistroVenta WHERE Fecha = @fechaActual";
                        command.Parameters.AddWithValue("@fechaActual", fechaActual);
                        break;
                    case "SEMANAL":
                        command.CommandText = "SELECT * FROM RegistroVenta WHERE Fecha BETWEEN date('now', 'weekday 0', '-6 days') AND date('now', 'weekday 0')";
                        break;
                    case "MENSUAL":
                        command.CommandText = "SELECT * FROM RegistroVenta WHERE Fecha >= date('now', 'start of month')";
                        break;
                    default:
                        command.CommandText = "SELECT * FROM RegistroVenta WHERE Fecha = @fecha";
                        command.Parameters.AddWithValue("@fecha", filtro);
                        break;
                }

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    registro.Add(new RegistroVenta
                    {
                        ID = reader.GetInt32(0),
                        Productos = reader.GetString(1),
                        Monto = reader.GetDouble(2),
                        Pago = reader.GetString(3).ToUpper(),
                    });
                }

                return registro;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void InsertarRegsitroVentas(RegistroVenta registroVenta)
        {
            try
            {
                using var connection = new SqliteConnection(AccesoBD.ConnectionString);
                connection.Open();

                var command = connection.CreateCommand();

                
                command.CommandText = @"INSERT INTO RegistroVenta (Fecha,Productos,Monto,Pago)
                VALUES ($fecha,$productos,$monto,$pago);";

                command.Parameters.AddWithValue("$fecha", registroVenta.Fecha);
                command.Parameters.AddWithValue("$productos", registroVenta.Productos);
                command.Parameters.AddWithValue("$monto", registroVenta.Monto);
                command.Parameters.AddWithValue("$pago", registroVenta.Pago);

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
