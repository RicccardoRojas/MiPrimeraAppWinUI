using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadPeleteria
{
    public class RegistroVenta
    {
        public int ID { get; set; }
        public DateTime Fecha { get; set; }
        public string Productos { get; set; }
        public double Monto { get; set; }
        public string Pago { get; set; }
    }
}
