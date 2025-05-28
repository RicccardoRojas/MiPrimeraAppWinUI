using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Inventario : Page
    {
        public ObservableCollection<InventarioItems> InventarioItemsList { get; set; } = new ObservableCollection<InventarioItems>();
        public Inventario()
        {
            this.InitializeComponent();

            txtEncabeAddInvent.Text = "AGREGAR/EDITAR\nINVENTARIO";
            tsCaducidad.IsOn = true;

            InventarioItemsList = GenerarInventario(100);
            DGInventario.ItemsSource = InventarioItemsList;
        }

        public class InventarioItems
        {
            public int ID { get; set; }
            public string Producto { get; set; }
            public string Tipo { get; set; }
            public string Caducidad { get; set; }
            public int Cantidad { get; set; }
        }

        public ObservableCollection<InventarioItems> GenerarInventario(int cantidad)
        {
            var inventario = new ObservableCollection<InventarioItems>();
            var random = new Random();

            for (int i = 0; i < cantidad; i++)
            {
                var Caducidad = DateTime.Now.AddDays(random.Next(30, 365));
                string fechaFormateada = Caducidad.ToString("d 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                inventario.Add(new InventarioItems
                {
                    ID = i + 1,
                    Producto = "Producto " + (i + 1),
                    Tipo = "Tipo " + (i + 1),
                    Caducidad = fechaFormateada,
                    Cantidad = random.Next(1, 100)
                });
            }
            return inventario;
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            if (toggle != null)
            {
                if (toggle.IsOn)
                {
                    dtpCaducidad.IsEnabled = true;
                    txtCaducidad.Opacity = 1;
                }
                else
                {
                    dtpCaducidad.IsEnabled = false;
                    txtCaducidad.Opacity = 0.5;
                }
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

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current is App app && app.MainWindow is MainWindow mainWindow)
            {
                mainWindow.NavegarAPagina("inicio");
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
