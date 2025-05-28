namespace AccesosDatos
{
    public class AccesoBD
    {
        public static string DbPath => @"C:\Users\riccc\OneDrive\Documentos\Base de Datos Paleteria\BDPeleteria.db";
        public static string ConnectionString => $"Data Source={DbPath}";
    }
}
