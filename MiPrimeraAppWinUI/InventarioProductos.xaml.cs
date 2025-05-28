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

            txtEncabezadoAddEditProduc.Text = "AGREGAR/EDITAR\nPRODUCTO";
            RellenarSabores();
            

            InventProductos = GenerarProductosConIcono(100);
            DGInventProducts.ItemsSource = InventProductos;
        }

        public class Productos
        {
            public int Id { get; set; }
            public BitmapImage Icono { get; set; }
            public string Producto { get; set; }
            public int Cantidad { get; set; }
            public string Caducidad { get; set; }
            public int IDTIPSabor { get; set; }
            public string Sabor { get; set; }
            public string Precio { get; set; }
        }

        private ObservableCollection<TiposSabores> GetTiposSabores(int cantidad)
        {
            var lista = new ObservableCollection<TiposSabores>();
            var sabores = new[] { "Chocolate", "Vainilla", "Fresa", "Menta", "Caf�", "Lim�n", "Coco", "Frutos Rojos", "Caramelo", "Pistacho" };
            var random = new Random();

            for (int i = 1; i <= cantidad; i++)
            {
                lista.Add(new TiposSabores { Id = i + 1, Sabor = sabores[random.Next(sabores.Length)] + $" #{i}" });
            }
            return lista;
        }

        private ObservableCollection<Productos> GenerarProductosConIcono(int cantidad)
        {
            var lista = new ObservableCollection<Productos>();
            var nombres = new[] { "Manzana", "Pl�tano", "Naranja", "Pera", "Sand�a", "Mel�n", "Durazno", "Fresa", "Kiwi", "Mango" };
            var tipos = new[] { "Paleta", "Nieve", "Helado", "Malteada", "Agua de sabor", "Raspado", "Yogurt congelado", "Cono", "Troleb�s", "Barquillo" };

            var random = new Random();

            for (int i = 1; i <= cantidad; i++)
            {
                string rutaImagen = "ms-appx:///Assets/helado.png"; // Aseg�rate de que est� en tu proyecto y marcado como 'Contenido'

                var bitmap = new BitmapImage(new Uri(rutaImagen));

                var Caducidad = DateTime.Now.AddDays(random.Next(30, 365));
                string fechaFormateada = Caducidad.ToString("d 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));


                lista.Add(new Productos
                { 
                    Id = i,
                    Icono = bitmap,
                    Producto = nombres[random.Next(nombres.Length)] + $" #{i}",
                    Cantidad = random.Next(1, 100),
                    Caducidad = fechaFormateada,
                    Sabor = tipos[random.Next(tipos.Length)],
                    Precio = $"${Math.Round(random.NextDouble() * 50 + 5, 2):N2}"
                });
            }

            return lista;
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
                // Aqu� puedes hacer m�s cosas con el archivo, como leerlo
            }
            else
            {
                btnSeleccionarArchivo.Content = "No se seleccion� ning�n archivo.";
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

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

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
                int a = Convert.ToInt32(cmbBuscarTipo.SelectedValue);
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

            contenido.ParentDialog = dialog;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {

                /*if (!string.IsNullOrWhiteSpace(sabor))
                {
                    //MI.InsertarSabor(sabor); // Aseg�rate de tener un m�todo InsertarSabor
                    cmbBuscarTipo.ItemsSource = MI.ObtenerTipoSabores();
                }*/
            }
            if (result == ContentDialogResult.None)
            {
                RellenarSabores();
            }
        }
    }
}
