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
using EntidadPeleteria;
using ManejadoresPaleteria;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{
    public sealed partial class AgregarSaborDialog : UserControl
    {
        public ContentDialog ParentDialog { get; set; }
        ManejadorInventarioProducto MI;

        public AgregarSaborDialog()
        {
            this.InitializeComponent();
            MI = new ManejadorInventarioProducto();

            LlenarSabores();
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
            if (btnMas.Tag?.ToString() == "Ingresar")
            {
                EstadoInsercion(true);
            }
            else if (btnMas.Tag?.ToString() == "Cancelar")
            {
                EstadoInsercion(false);

            }
        }

        void LlenarSabores()
        {
            cmbTipoSabores.ItemsSource = MI.ObtenerTipoSabores();
            cmbTipoSabores.DisplayMemberPath = "Sabor";
            cmbTipoSabores.SelectedValuePath = "Id";
        }

        private async void btnVerificar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTipoSabor.Text))
            {
                TiposSabores nuevoSabor = new TiposSabores
                {
                    Sabor = txtTipoSabor.Text.Trim()
                };

                MI.InsertarTipoSabores(nuevoSabor);

                LlenarSabores();
                EstadoInsercion(false);
            }
            else
            {
                txtTipoSabor.PlaceholderForeground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
                txtTipoSabor.PlaceholderText = "Ingresa un Valor Valido";
            }
        }

        void EstadoInsercion(bool estado)
        {
            if (estado)
            {
                if (ParentDialog != null)
                {
                    ParentDialog.IsPrimaryButtonEnabled = false;
                }

                cmbTipoSabores.Visibility = Visibility.Collapsed;
                txtTipoSabor.Visibility = Visibility.Visible;
                btnVerificar.Visibility = Visibility.Visible;
                IMGbtnMas.Source = new Microsoft.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/eliminar.png"));
                txtSaborEncabezado.Opacity = 0.5;
                txtSabor.IsEnabled = false;
                btnMas.Tag = "Cancelar";
            }
            else
            {
                if (ParentDialog != null)
                {
                    ParentDialog.IsPrimaryButtonEnabled = true;
                }

                cmbTipoSabores.Visibility = Visibility.Visible;
                txtTipoSabor.Visibility = Visibility.Collapsed;
                btnVerificar.Visibility = Visibility.Collapsed;
                IMGbtnMas.Source = new Microsoft.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/signo-de-mas.png"));
                txtSaborEncabezado.Opacity = 1;
                txtSabor.IsEnabled = true;
                btnMas.Tag = "Ingresar";

                txtTipoSabor.PlaceholderForeground = null;
                txtTipoSabor.PlaceholderText = "Ingresa Nuevo Tipo Sabor";
            }    
        }
    }
}
