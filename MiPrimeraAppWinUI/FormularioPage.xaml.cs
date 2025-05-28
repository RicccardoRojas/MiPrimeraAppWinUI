using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{

    public sealed partial class FormularioPage : Page
    {
        double total = 0.0;
        public FormularioPage()
        {
            this.InitializeComponent();
            txtTotal.Text = "TOTAL: \n$" + total.ToString("F2");
        }

        private void btnCantidad_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;
        }

        private void btnCantidad_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void Botones_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;
        }

        private void Botones_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void btnMas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                txtCantidad.Text = "1";
            }
            else
            {
                txtCantidad.Text = (int.Parse(txtCantidad.Text) + 1).ToString();
            }
            
        }

        private void btnMenos_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCantidad.Text) && int.Parse(txtCantidad.Text) >= 1 )
            {
                txtCantidad.Text = (int.Parse(txtCantidad.Text) - 1).ToString();
            }
        }

        bool _suppressTextChanged = false;

        private void txtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressTextChanged) return;

            var box = (TextBox)sender;
            string original = box.Text;

            string filtered = new string(original.Where(char.IsDigit).ToArray());

            if (filtered != original)
            {
                int selStart = box.SelectionStart;
                int diff = original.Length - filtered.Length;
                int newPos = Math.Max(0, selStart - diff);

                _suppressTextChanged = true;
                box.Text = filtered;
                box.SelectionStart = Math.Min(filtered.Length, newPos);
                _suppressTextChanged = false;
            }
        }
        
        private async void btnVerificar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTipoHelado.Text) || string.IsNullOrEmpty((cmbSabores.SelectedItem as ComboBoxItem)?.Content?.ToString()) || string.IsNullOrEmpty(txtCantidad.Text))
            {
                var dialog = new ContentDialog
                {
                    Title = "Campos obligatorios",
                    Content = "Por favor, completa todos los campos antes de continuar.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot // Necesario para que funcione en WinUI 3
                };

                await dialog.ShowAsync();
                return;
            }
            else
            {
                double precio = double.Parse(txtPrecioUnitario.Text.Replace("$", "").Trim());
                double subtotal = Math.Round(precio * int.Parse(txtCantidad.Text), 2);

                AgregarItemAlCarrito(txtTipoHelado.Text, (cmbSabores.SelectedItem as ComboBoxItem)?.Content?.ToString(),txtPrecioUnitario.Text, int.Parse(txtCantidad.Text), 
                    subtotal, "/Assets/helado.png");

                txtTotal.Text = $"TOTAL: \n${(total += subtotal).ToString("F2")}" ;
            }
                
        }

        private void AgregarItemAlCarrito(string Nombre, string Sabor, string precio, int cantidad, double subtotal, string imagen)
        {
            var nuevoItem = new ListaCarrito
            {
                Nombre = Nombre,
                Descripcion = Sabor.ToUpper(),
                Precio = precio,
                Cantidad = cantidad.ToString(),
                Total = $"${subtotal.ToString("F2")}",
                Imagen = imagen
            };

            nuevoItem.FilaEliminada += (valor) =>
            {
                total -= valor;
                txtTotal.Text = $"TOTAL:\n${total.ToString("F2")}";
            };

            PanelCarrito.Children.Add(nuevoItem);
            
        }

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current is App app && app.MainWindow is MainWindow mainWindow)
            {
                mainWindow.NavegarAPagina("inicio");
            }
        }

        private void btnPagoEfectivo_Click(object sender, RoutedEventArgs e)
        {
            btnPagoTarjeta.Background = new SolidColorBrush(Colors.LightGreen);
            btnPagoEfectivo.Background = new SolidColorBrush(Colors.Blue);
        }

        private void btnPagoTarjeta_Click(object sender, RoutedEventArgs e)
        {
            btnPagoTarjeta.Background = new SolidColorBrush(Colors.Blue);
            btnPagoEfectivo.Background = new SolidColorBrush(Colors.LightGreen);
        }

    }
}
