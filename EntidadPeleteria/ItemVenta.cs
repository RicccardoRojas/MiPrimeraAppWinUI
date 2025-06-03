namespace EntidadPeleteria
{
    public class ItemVenta
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double Subtotal => PrecioUnitario * Cantidad;
    }

}
