using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Input;
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{
    public sealed partial class ListaCarrito : UserControl
    {
        public event Action<double> FilaEliminada;
        public event Action<string, double, string, int> FilaEditada;

        public ListaCarrito()
        {
            this.InitializeComponent();
        }

        public string Nombre
        {
            get => (string)GetValue(NombreProperty);
            set => SetValue(NombreProperty, value);
        }

        public static readonly DependencyProperty NombreProperty =
            DependencyProperty.Register(nameof(Nombre), typeof(string), typeof(ListaCarrito), new PropertyMetadata(""));

        public string Descripcion
        {
            get => (string)GetValue(DescripcionProperty);
            set => SetValue(DescripcionProperty, value);
        }

        public static readonly DependencyProperty DescripcionProperty =
            DependencyProperty.Register(nameof(Descripcion), typeof(string), typeof(ListaCarrito), new PropertyMetadata(""));

        public string Precio
        {
            get => (string)GetValue(PrecioProperty);
            set => SetValue(PrecioProperty, value);
        }

        public static readonly DependencyProperty PrecioProperty =
            DependencyProperty.Register(nameof(Precio), typeof(string), typeof(ListaCarrito), new PropertyMetadata(""));

        public string Cantidad
        {
            get => (string)GetValue(CantidadProperty);
            set => SetValue(CantidadProperty, value);
        }

        public static readonly DependencyProperty CantidadProperty =
            DependencyProperty.Register(nameof(Cantidad), typeof(string), typeof(ListaCarrito), new PropertyMetadata(""));

        public string Total
        {
            get => (string)GetValue(TotalProperty);
            set => SetValue(TotalProperty, value);
        }

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register(nameof(Total), typeof(string), typeof(ListaCarrito), new PropertyMetadata(""));

        public string Imagen
        {
            get => (string)GetValue(ImagenProperty);
            set => SetValue(ImagenProperty, value);
        }

        public static readonly DependencyProperty ImagenProperty =
            DependencyProperty.Register(nameof(Imagen), typeof(string), typeof(ListaCarrito), new PropertyMetadata(""));

        private void Botones_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void Botones_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;
        }

        public void EliminarFila()
        {
            var parent = this.Parent as Panel;
            if (parent != null)
            {
                parent.Children.Remove(this); // Elimina este control del contenedor
            }

            if (double.TryParse(txtSubTotal.Text.Replace("$", "").Trim(), out double valor))
            {
                FilaEliminada?.Invoke(valor); // Dispara el evento y pasa el valor restado
            }

        }

        public void EditarFila()
        {
            var parent = this.Parent as Panel;
            if (parent != null)
            {
                // Cambia el fondo para indicar visualmente que se está editando
                borderColor.Background = new SolidColorBrush(Colors.LightGreen);

                // Extrae los valores
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                string precioStr =  Precio.Replace("$", "").Trim();
                int cantidad = int.Parse(txtCantidad.Text);

                if (double.TryParse(precioStr, out double precio))
                {
                    FilaEditada?.Invoke(nombre, precio, descripcion, cantidad);
                }
            }
        }


        private void btnEliminar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EliminarFila();
        }

        private void btnEditar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EditarFila();
        }
    }
}
