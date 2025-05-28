namespace EntidadPeleteria
{
    public class Sabores
    {
        public int ID { get; set; }
        public string Sabor { get; set; }
        public int FKIDTipoSabor { get; set; }
    }
}
