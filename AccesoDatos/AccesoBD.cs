using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos
{
    public class AccesoBD
    {
        public static string DbPath => @"C:\Users\riccc\OneDrive\Documentos\Base de Datos Paleteria\BDPeleteria.db";
        public static string ConnectionString => $"Data Source={DbPath}";
    }
}
