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
using HarfBuzzSharp;
using static MiPrimeraAppWinUI.RegistroVenta;

namespace MiPrimeraAppWinUI
{
    public sealed partial class FormularioPage : Page
    {
        ManejadorInventarioProducto MI;
        private ListaCarrito itemEditando = null;
        double total = 0.0, subtotalAnterior = 0;
        string rutacarrito = "";
        int fkidtiposabor = 0;
        bool Editar = false, efectivo = false, tarjeta = false;
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

        private void ConfirmarEdicion()
        {
            if (itemEditando != null)
            {
                itemEditando.Nombre = txtTipoHelado.Text;
                itemEditando.Descripcion = ((Sabores)cmbSabores.SelectedItem).Sabor.ToUpper();
                itemEditando.Precio = txtPrecioUnitario.Text;
                itemEditando.Cantidad = txtCantidad.Text;

                string precioTexto = txtPrecioUnitario.Text.Replace("$", "").Trim();

                if (double.TryParse(precioTexto, out double precio) &&
                    int.TryParse(txtCantidad.Text, out int cantidad))
                {
                    double nuevoSubtotal = precio * cantidad;
                    itemEditando.Total = $"${nuevoSubtotal:F2}";
                }


                itemEditando.RestaurarColor();
                itemEditando = null; // Limpia la referencia
            }
        }

        private void LimpiarRegistro()
        {
            txtTipoHelado.Text = string.Empty;
            cmbSabores.ItemsSource = null;
            txtPrecioUnitario.Text = string.Empty;
            txtCantidad.Text = "1";
            Editar = false; // Resetea el estado de edición
            subtotalAnterior = 0; // Resetea el subtotal anterior
            itemEditando = null; // Limpia la referencia al item editando
        }

        private async void btnVerificar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
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
                else if (Editar)
                {
                    ConfirmarEdicion();

                    double precio = double.Parse(txtPrecioUnitario.Text.Replace("$", "").Trim());
                    int cantidad = int.Parse(txtCantidad.Text);
                    double subtotalNuevo = Math.Round(precio * cantidad, 2);
                    total = total - subtotalAnterior + subtotalNuevo;

                    txtTotal.Text = $"TOTAL:\n${total.ToString("F2")}";

                    LimpiarRegistro();
                }
                else if (!Editar)
                {
                    double precio = double.Parse(txtPrecioUnitario.Text.Replace("$", "").Trim());
                    double subtotal = Math.Round(precio * int.Parse(txtCantidad.Text), 2);

                    AgregarItemAlCarrito(txtTipoHelado.Text, saborSeleccionado.Sabor, txtPrecioUnitario.Text, int.Parse(txtCantidad.Text),
                        subtotal, rutacarrito);

                    txtTotal.Text = $"TOTAL: \n${(total += subtotal).ToString("F2")}";

                    LimpiarRegistro();
                }
            }
            catch (Exception)
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
            
                
        }

        private void AgregarItemAlCarrito(string Nombre, string Sabor, string precio, int cantidad, double subtotal, string imagen)
        {
            var nuevoItem = new ListaCarrito
            {
                Nombre = Nombre,
                Descripcion = Sabor.ToUpper(),
                TAGID = fkidtiposabor,
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

            nuevoItem.FilaEditada += (nombre, precio, descripcion, cantidad, tagId) =>
            {
                Editar = true;
                // Restaurar el color del item anterior (si hay)
                if (itemEditando != null && itemEditando != nuevoItem)
                {
                    itemEditando.RestaurarColor();
                }

                // Guardar este como el nuevo item editando
                itemEditando = nuevoItem;

                // Mostrar los datos en controles principales
                txtTipoHelado.Text = nombre;
                txtPrecioUnitario.Text = precio.ToString("C2");
                RellenarSabores(tagId);

                var saborSeleccionado = cmbSabores.Items.Cast<Sabores>()
                    .FirstOrDefault(s => s.Sabor.Equals(descripcion, StringComparison.OrdinalIgnoreCase));
                if (saborSeleccionado != null)
                {
                    cmbSabores.SelectedItem = saborSeleccionado;
                }

                txtCantidad.Text = cantidad.ToString();

                subtotalAnterior = precio * cantidad;
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
            efectivo = true;
            tarjeta = false;
            btnPagoTarjeta.Background = new SolidColorBrush(Colors.LightGreen);
            btnPagoEfectivo.Background = new SolidColorBrush(Colors.Blue);
        }

        private void btnPagoTarjeta_Click(object sender, RoutedEventArgs e)
        {
            tarjeta = true;
            efectivo  = false;
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

        private async void btnPagar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PanelCarrito.Children == null || (!efectivo && !tarjeta))
                {
                    var dialog = new ContentDialog
                    {
                        Title = $"Error al Pagar",
                        Content = "Inserta Productos al Carrito o Selecciona Forma de Pago",
                        CloseButtonText = "Cancelar",
                        DefaultButton = ContentDialogButton.Primary,
                        XamlRoot = this.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
                else
                {
                    var dialog = new PagoCaja(total);

                    var dialogConfirmacion = new ContentDialog
                    {
                        Title = "Confirmar Pago",
                        Content = dialog,
                        PrimaryButtonText = "Pagar",
                        CloseButtonText = "Cancelar",
                        DefaultButton = ContentDialogButton.Primary,
                        FullSizeDesired = true,
                        XamlRoot = this.XamlRoot,
                    };

                    dialogConfirmacion.Resources["ContentDialogMaxWidth"] = 1000;
                    dialogConfirmacion.Resources["ContentDialogMinWidth"] = 800;

                    dialogConfirmacion.Resources["ContentDialogMinHeight"] = 350;
                    dialogConfirmacion.Resources["ContentDialogMaxHeight"] = 500;

                    dialogConfirmacion.PrimaryButtonClick += (s, args) =>
                    {
                        double totalvalor,cantidadrecibida,cambio;

                        (totalvalor,cantidadrecibida,cambio) = dialog.ObtenerCambio();

                        if (cambio <= 0.0)
                        {
                            args.Cancel = true; // Cancela el cierre del dialog si el cambio es menor o igual a 0
                        }
                        else
                        {
                            var productos = new List<ItemVenta>();

                            foreach (var child in PanelCarrito.Children)
                            {
                                if (child is ListaCarrito item)
                                {
                                    string nombre = $"{item.Nombre} {item.Descripcion}";
                                    int cantidad = int.TryParse(item.Cantidad, out var c) ? c : 0;
                                    double precio = double.Parse(item.Precio.Replace("$", ""));
                                    double subtotal = cantidad * precio;

                                    productos.Add(new ItemVenta
                                    {
                                        Nombre = nombre,
                                        Cantidad = cantidad,
                                        PrecioUnitario = precio,
                                    });
                                }
                            }

                            string formapago = efectivo ? "Efectivo" : tarjeta ? "Tarjeta" : "No Especificado";
                            //double total = productos.Sum(p => p.Subtotal);

                            var generador = new ManejadorGenerarTicket();
                            generador.GenerarTicketPDF(productos, totalvalor, cantidadrecibida,formapago,cambio);

                            //Resetea el estado de pago
                            efectivo = true;
                            tarjeta = true;
                            btnPagoTarjeta.Background = new SolidColorBrush(Colors.LightGreen);
                            btnPagoEfectivo.Background = new SolidColorBrush(Colors.LightGreen);

                            // Resetea el carrito y total después del pago
                            PanelCarrito.Children.Clear();
                            total = 0.0;
                            txtTotal.Text = "TOTAL: \n$0.00";
                        }
                        
                    };

                    var result = await dialogConfirmacion.ShowAsync();
                    
                }
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog
                {
                    Title = $"Error al Pagar",
                    Content = ex.Message,
                    CloseButtonText = "Cancelar",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }
            
        }

        private async void BotonProductos_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (sender is FrameworkElement element && element.DataContext is Productos filaSeleccionada)
                {
                    Editar = false;
                    subtotalAnterior = 0; // Resetea el subtotal anterior
                    List<Productos> productos = MI.ObtenerProductosCompra(filaSeleccionada.Id);
                    foreach (var produc in productos)
                    {
                        txtTipoHelado.Text = produc.Producto;
                        RellenarSabores(produc.IDTIPSabor);
                        fkidtiposabor = produc.IDTIPSabor;
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
