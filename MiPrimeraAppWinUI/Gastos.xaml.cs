using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Input;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{
   
    public sealed partial class Gastos : Page
    {
        public ObservableCollection<Gasto> GastosList { get; set; } = new ObservableCollection<Gasto>();
        public Gastos()
        {
            this.InitializeComponent();

            txtEncabeAddGastos.Text = "AGREGAR/EDITAR\nGASTO";
            txtPrecio.Text = "$0.00";

            GastosList = GenerarGastos(100);
            DGGastos.ItemsSource = GastosList;
        }

        public class Gasto
        {
            public int ID { get; set; }
            public string Fecha { get; set; }
            public string Nombre { get; set; }
            public string Precio { get; set; }
            public string Observaciones { get; set; }
        }

        private ObservableCollection<Gasto> GenerarGastos (int cantidad)
        {
            var lista = new ObservableCollection<Gasto>();
            var random = new Random();

            for (int i = 1; i <= cantidad; i++)
            {
                var Caducidad = DateTime.Now.AddDays(random.Next(30, 365));
                string fechaFormateada = Caducidad.ToString("d 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                lista.Add(new Gasto { 
                    ID = i + 1, 
                    Fecha = fechaFormateada,
                    Nombre = "Gasto " + (i + 1),
                    Precio = $"${Math.Round(random.NextDouble() * (10000 - 5) + 5, 2):N2}",
                    Observaciones = "Observaciones " + (i + 1)
                }
                );
            }
            return lista;
        }

        private void Botones_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void Botones_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;
        }


        private bool _suppressTextChanged = false;
        private long _rawCents = 0; // Mantenemos los centavos internamente

        private void txtPrecio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressTextChanged) return;

            var box = (TextBox)sender;
            string input = box.Text;

            // Filtrar solo dígitos
            string digits = new string(input.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(digits))
            {
                _rawCents = 0;
            }
            else
            {
                // Limitar longitud si quieres (ej. hasta $99,999.99 = 7 dígitos)
                if (digits.Length > 9) digits = digits.Substring(0, 9);

                // Convertir string a centavos
                if (long.TryParse(digits, out long cents))
                {
                    _rawCents = cents;
                }
            }

            // Mostrar como moneda
            decimal value = _rawCents / 100m;

            _suppressTextChanged = true;
            box.Text = value.ToString("C2"); // Usa $ y comas
            box.SelectionStart = box.Text.Length;
            _suppressTextChanged = false;
        }
        

        private void txtPrecio_Paste(object sender, TextControlPasteEventArgs e)
        {
            e.Handled = true;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current is App app && app.MainWindow is MainWindow mainWindow)
            {
                mainWindow.NavegarAPagina("inicio");
            }
        }

    }
}
