using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ManejadoresPaleteria;
using EntidadPeleteria;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{

    public sealed partial class FormularioPage : Page
    {
        ManejadorInventarioProducto MI;
        double total = 0.0;
        string rutacarrito = "";
        public FormularioPage()
        {
            this.InitializeComponent();
            MI = new ManejadorInventarioProducto();
            txtTotal.Text = "TOTAL: \n$" + total.ToString("F2");

            Actualizar("%");
        }
        void RellenarSabores(int fkid)
        {
            //Consulta Sabores 
            cmbSabores.ItemsSource = MI.ObtenerSaboresLista(fkid);

            //Tipos de Productos Formulario (Mostrar)
            cmbSabores.DisplayMemberPath = "Sabor";
            cmbSabores.SelectedValuePath = "Id";

            cmbSabores.SelectedIndex = -1; 
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
            var saborSeleccionado = cmbSabores.SelectedItem as Sabores;

            if (string.IsNullOrEmpty(txtTipoHelado.Text) || string.IsNullOrEmpty(saborSeleccionado.Sabor) || string.IsNullOrEmpty(txtCantidad.Text))
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

                AgregarItemAlCarrito(txtTipoHelado.Text, saborSeleccionado.Sabor, txtPrecioUnitario.Text, int.Parse(txtCantidad.Text), 
                    subtotal, rutacarrito);

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

            nuevoItem.FilaEditada += (nombre, precio, descripcion, cantidad) =>
            {
                // Aquí haces lo que necesites con los datos editados
                txtTipoHelado.Text = nombre;
                txtPrecioUnitario.Text = precio.ToString("F2");
                cmbSabores.SelectedItem = cmbSabores.Items
                    .Cast<object>()
                    .FirstOrDefault(item => ((Sabores)item).Sabor == descripcion);
                txtCantidad.Text = cantidad.ToString();
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

        private void GridViewProductos_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Asegúrate de tener 3 columnas visibles, con margen
            const int columnas = 3;
            const double margenTotal = 15; // margen exterior + padding entre columnas

            if (gridViewProductos.ItemsPanelRoot is ItemsWrapGrid panel)
            {
                double anchoDisponible = gridViewProductos.ActualWidth - margenTotal;

                if (anchoDisponible > 0)
                {
                    double anchoPorColumna = anchoDisponible / columnas;
                    panel.ItemWidth = anchoPorColumna;
                }
            }
        }

        private void txtBuscarProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            Actualizar(txtBuscarProducto.Text.Trim() == "" ? "%" : $"%{txtBuscarProducto.Text.Trim()}%");
        }

        void Actualizar(string Filtro)
        {
            gridViewProductos.ItemsSource = MI.ObtenerProductos(Filtro);
        }

        private async void BotonProductos_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (sender is FrameworkElement element && element.DataContext is Productos filaSeleccionada)
                {
                    List<Productos> productos = MI.ObtenerProductosCompra(filaSeleccionada.Id);
                    foreach (var produc in productos)
                    {
                        txtTipoHelado.Text = produc.Producto;
                        RellenarSabores(produc.IDTIPSabor);
                        txtPrecioUnitario.Text = produc.Precio.ToString("C2");
                        rutacarrito = produc.RutaIcono;
                    }
                }
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog
                {
                    Title = $"Error al Consultar",
                    Content = ex.Message,
                    CloseButtonText = "Cancelar",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot
                };
                dialog.ShowAsync();
            }
        }
    }
}
