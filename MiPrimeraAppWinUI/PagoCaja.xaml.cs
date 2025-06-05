using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static MiPrimeraAppWinUI.RegistroVenta;
using static QuestPDF.Helpers.Colors;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{
    public sealed partial class PagoCaja : UserControl
    {
        double total = 0.0;
        public PagoCaja()
        {
            this.InitializeComponent();

            lblCantidad.Text = "$" + total.ToString("F2");
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn10_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn20_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn50_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn100_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn200_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn1000_Click(object sender, RoutedEventArgs e)
        {

        }

        private bool _suppressTextChanged = false;
        private long _rawCents = 0; // Mantenemos los centavos internamente
        private void txtCantidad_TextChanged(object sender, TextChangedEventArgs e)
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

            lblCantidad.Text = box.Text;
        }
    }
}
