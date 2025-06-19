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
        double total = 0.0, valorinicial;
        public PagoCaja(double valor)
        {
            this.InitializeComponent();

            valorinicial = valor;
            lblCantidad.Text = "$" + total.ToString("F2");
            lblTotal.Text = "$" + valor.ToString("F2");
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Suma(1);
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            Suma(2);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            Suma(5);
        }

        private void btn10_Click(object sender, RoutedEventArgs e)
        {
            Suma(10);
        }

        private void btn20_Click(object sender, RoutedEventArgs e)
        {
            Suma(20);
        }

        private void btn50_Click(object sender, RoutedEventArgs e)
        {
            Suma(50);
        }

        private void btn100_Click(object sender, RoutedEventArgs e)
        {
            Suma(100);
        }

        private void btn200_Click(object sender, RoutedEventArgs e)
        {
            Suma(200);
        }

        private void btn1000_Click(object sender, RoutedEventArgs e)
        {
            Suma(1000);
        }

        private void btn500_Click(object sender, RoutedEventArgs e)
        {
            Suma(500);
        }

        void Suma(double valor)
        {
            total += valor; // actualizas el acumulado total de lo entregado
            lblCantidad.Text = "$" + total.ToString("F2");
            Resta(total); // pasas el total entregado para calcular el cambio
        }


        void Resta(double entregado)
        {
            double cambio = entregado - valorinicial;
            if (cambio < 0) cambio = 0;
            lblcambio.Text = "$" + cambio.ToString("F2");
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
                lblcambio.Text = "$" + valorinicial.ToString("F2");
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
            Resta((double)value);
        }

        public (double,double,double) ObtenerCambio()
        {
            double total = lblTotal.Text.Replace("$", "").Replace(",", "").Trim() == "" ? 0.0 : Convert.ToDouble(lblTotal.Text.Replace("$", "").Replace(",", "").Trim());
            double pago = lblCantidad.Text.Replace("$", "").Replace(",", "").Trim() == "" ? 0.0 : Convert.ToDouble(lblCantidad.Text.Replace("$", "").Replace(",", "").Trim());
            double cambio = lblcambio.Text.Replace("$", "").Replace(",", "").Trim() == "" ? 0.0 : Convert.ToDouble(lblcambio.Text.Replace("$", "").Replace(",", "").Trim());

            return (total, pago, cambio);   
        }
    }
}
