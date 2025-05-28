using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesPaleteria
{
    public class Sabores
    {
        public int ID { get; set; }
        public string Sabor { get; set; }
        public int FKIDTipoSabor { get; set; }
    }
}
