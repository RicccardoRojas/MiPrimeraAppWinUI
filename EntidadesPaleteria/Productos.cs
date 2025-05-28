using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace EntidadesPaleteria
{
    public class Productos
    {
        public int Id { get; set; }
        public string RutaIcono { get; set; }
        public BitmapImage Icono { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public string Caducidad { get; set; }
        public int IDTIPSabor { get; set; }
        public string Sabor { get; set; }
        public string Precio { get; set; }
    }
}
