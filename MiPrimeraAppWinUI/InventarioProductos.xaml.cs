using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Media.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CommunityToolkit.WinUI.UI.Controls;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Microsoft.UI.Input;
using ManejadoresPaleteria;
using EntidadPeleteria;


namespace MiPrimeraAppWinUI
{

    public sealed partial class InventarioProductos : Page
    {
        ManejadorInventarioProducto MI;

        public ObservableCollection<Productos> InventProductos { get; set; }
        public ObservableCollection<Sabores> SaboresList { get; set; } = new ObservableCollection<Sabores>();

        public InventarioProductos()
        {
            this.InitializeComponent();
            MI = new ManejadorInventarioProducto();

            txtPrecio.Text = "$0.00"; // Inicializar el TextBox de precio con un valor predeterminado

            txtEncabezadoAddEditProduc.Text = "AGREGAR/EDITAR\nPRODUCTO";
            RellenarSabores();

            Actualizar("%");
        }


        private async void btnSeleccionarArchivo_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();

            // Inicializa el picker con la ventana actual
            var window = (App.Current as App)?.MainWindow;
            var hwnd = WindowNative.GetWindowHandle(window);
            InitializeWithWindow.Initialize(picker, hwnd);

            // Configura filtros (opcional)
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpg");

            // Abre el selector
            var archivo = await picker.PickSingleFileAsync();
            if (archivo != null)
            {
                btnSeleccionarArchivo.Content = "Archivo seleccionado: " + archivo.Path;
                // Aquí puedes hacer más cosas con el archivo, como leerlo
            }
            else
            {
                btnSeleccionarArchivo.Content = "No se seleccionó ningún archivo.";
            }
        }

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current is App app && app.MainWindow is MainWindow mainWindow)
            {
                mainWindow.NavegarAPagina("inicio");
            }
        }

        private void Botones_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;
        }

        private void Botones_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtCantidad.Text = "";
            txtPrecio.Text = "$0.00";
            dtpCaducidad.Date = DateTimeOffset.Now;
            btnSeleccionarArchivo.Content = "Seleccionar archivo";
            cmbSabores.SelectedIndex = -1;
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtCantidad.Text) && !string.IsNullOrEmpty(txtPrecio.Text) && dtpCaducidad != null && (btnSeleccionarArchivo.Content != "No se seleccionó ningún archivo."
                || btnSeleccionarArchivo.Content != "Seleccionar archivo"))
            {
                if (!string.IsNullOrEmpty(GuardarIcono(btnSeleccionarArchivo.Content.ToString())))
                {
                    Productos productos = new Productos
                    {
                        RutaIcono = GuardarIcono(btnSeleccionarArchivo.Content.ToString()),
                        Producto = txtNombre.Text,
                        Cantidad = Convert.ToInt32(txtCantidad.Text),
                        Caducidad = dtpCaducidad.Date.ToString("d 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES")),
                        Precio = Convert.ToDouble(txtPrecio.Text.Replace("$", "").Trim()),
                        IDTIPSabor = Convert.ToInt32(cmbSabores.SelectedValue)
                    };

                    MI.InsertarProductos(productos);

                    LimpiarCampos();

                    Actualizar("%");
                }
                else 
                {
                    var dialog = new ContentDialog
                    {
                        Title = $"Error al Guardar el Icono",
                        Content = $"A Ocurrido un Error al Almacenar el Icono, Revisa el Nombre y el Archivo",
                        CloseButtonText = "Cancelar",
                        DefaultButton = ContentDialogButton.Primary,
                        XamlRoot = this.XamlRoot
                    };

                    await dialog.ShowAsync();
                }
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = $"Error al Guardar",
                    Content = $"A ocurrido un error al almacenar la informacion, Revisa que los campos esten correctamente llenos",
                    CloseButtonText = "Cancelar",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }

        string GuardarIcono(string texto)
        {
            try
            {
                string rutaCarpeta = @"C:\Paleteria Real Rojas";

                // Eliminar el prefijo y espacios
                string ruta = texto.Replace("Archivo seleccionado:", "").Trim();

                string nombreArchivo = Path.GetFileName(ruta);

                if (!Directory.Exists(rutaCarpeta))
                {
                    Directory.CreateDirectory(rutaCarpeta);
                }

                string destino = Path.Combine(rutaCarpeta, nombreArchivo);

                // Solo copiar si el archivo no existe ya
                if (!File.Exists(destino))
                {
                    File.Copy(ruta, destino);
                }

                return destino;
            }
            catch (Exception)
            {
                return "";
            }
        }




        void RellenarSabores()
        {
            //Consulta Sabores 
            cmbBuscarTipo.ItemsSource = cmbSabores.ItemsSource = MI.ObtenerTipoSabores();

            //Tipos de Productos Formulario (Mostrar)
            cmbSabores.DisplayMemberPath = "Sabor";
            cmbSabores.SelectedValuePath = "Id";
            
            //Tipos de Sabores Lista (Mostrar)
            cmbBuscarTipo.DisplayMemberPath = "Sabor";
            cmbBuscarTipo.SelectedValuePath = "Id";
        }

        private void cmbBuscarTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBuscarTipo.SelectedValue != null)
            {
                DGTSabores.ItemsSource = MI.ObtenerSabores(Convert.ToInt32(cmbBuscarTipo.SelectedValue));
            }
            else
            {
                DGTSabores.ItemsSource = null;
            }

        }

        private async void btnAgregar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var contenido = new AgregarSaborDialog();

            var dialog = new ContentDialog
            {
                Title = "Agregar nuevo sabor",
                Content = contenido,
                PrimaryButtonText = "Agregar",
                CloseButtonText = "Cancelar",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot // Importante para WinUI 3
            };

            dialog.PrimaryButtonClick += (s, args) =>
            {
                bool exito = contenido.ProcesarAgregar();

                if (!exito)
                {
                    args.Cancel = true; // No cerrar el dialog si falla la validación
                }
                else
                {
                    RellenarSabores();
                }
            };

            contenido.ParentDialog = dialog;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                RellenarSabores();
            }
        }

        private async void btnEliminar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string sabor = "";

            // Obtener el objeto asociado al botón presionado
            if (sender is FrameworkElement element && element.DataContext is Sabores filaSeleccionada)
            {
                sabor = filaSeleccionada.Sabor;

                var dialog = new ContentDialog
                {
                    Title = $"Eliminar Sabor {sabor}",
                    Content = $"¿Estás seguro de que deseas eliminar este sabor ({sabor})?",
                    PrimaryButtonText = "Eliminar",
                    CloseButtonText = "Cancelar",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot
                };

                dialog.PrimaryButtonClick += (s, args) =>
                {
                    MI.EliminarSabores(filaSeleccionada.ID);
                    DGTSabores.ItemsSource = MI.ObtenerSabores(Convert.ToInt32(cmbBuscarTipo.SelectedValue));
                };

                await dialog.ShowAsync();
            }
        }

        private async void btnEditar_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if (sender is FrameworkElement element && element.DataContext is Sabores filaSeleccionada)
            {
                var contenido = new AgregarSaborDialog(Convert.ToInt32(cmbBuscarTipo.SelectedValue),filaSeleccionada.Sabor,filaSeleccionada.ID);


                var dialog = new ContentDialog
                {
                    Title = "Agregar nuevo sabor",
                    Content = contenido,
                    PrimaryButtonText = "Agregar",
                    CloseButtonText = "Cancelar",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot // Importante para WinUI 3
                };

                dialog.PrimaryButtonClick += (s, args) =>
                {
                    bool exito = contenido.ProcesarAgregar();

                    if (!exito)
                    {
                        args.Cancel = true; // No cerrar el dialog si falla la validación
                    }
                    else
                    {
                        DGTSabores.ItemsSource = MI.ObtenerSabores(Convert.ToInt32(cmbBuscarTipo.SelectedValue));
                    }
                };

                contenido.ParentDialog = dialog;

                var result = await dialog.ShowAsync();

            }

                
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

        void Actualizar(string Filtro)
        {
            DGInventProducts.ItemsSource = MI.ObtenerProductos(Filtro);
        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBuscar.Text))
            {
                Actualizar($"%{txtBuscar.Text}%");
            }
            else
            {
                Actualizar("%");
            }
            
        }
    }
}
