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


namespace MiPrimeraAppWinUI
{

    public sealed partial class InventarioProductos : Page
    {

        public ObservableCollection<Productos> InventProductos { get; set; }

        public InventarioProductos()
        {
            this.InitializeComponent();
            txtEncabezadoAddEditProduc.Text = "AGREGAR/EDITAR\nPRODUCTO";

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
            public string Precio { get; set; }
        }

        private ObservableCollection<Productos> GenerarProductosConIcono(int cantidad)
        {
            var lista = new ObservableCollection<Productos>();
            var nombres = new[] { "Manzana", "Plátano", "Naranja", "Pera", "Sandía", "Melón", "Durazno", "Fresa", "Kiwi", "Mango" };
            var random = new Random();

            for (int i = 1; i <= cantidad; i++)
            {
                string rutaImagen = "ms-appx:///Assets/helado.png"; // Asegúrate de que esté en tu proyecto y marcado como 'Contenido'

                var bitmap = new BitmapImage(new Uri(rutaImagen));

                var Caducidad = DateTime.Now.AddDays(random.Next(30, 365));
                string fechaFormateada = Caducidad.ToString("dddd d 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));


                lista.Add(new Productos
                { 
                    Id = i,
                    Icono = bitmap,
                    Producto = nombres[random.Next(nombres.Length)] + $" #{i}",
                    Cantidad = random.Next(1, 100),
                    Caducidad = fechaFormateada,
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
                // Aquí puedes hacer más cosas con el archivo, como leerlo
            }
            else
            {
                btnSeleccionarArchivo.Content = "No se seleccionó ningún archivo.";
            }
        }
    }
}
