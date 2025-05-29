using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
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

        bool editar = false;
        int idedit;

        public AgregarSaborDialog()
        {
            this.InitializeComponent();
            MI = new ManejadorInventarioProducto();

            LlenarSabores();
        }

        public AgregarSaborDialog(int tiposabor, string sabor, int id)
        {
            this.InitializeComponent();
            MI = new ManejadorInventarioProducto();
            LlenarSabores();

            cmbTipoSabores.SelectedValue = tiposabor;
            cmbTipoSabores.IsEnabled = false;
            btnMas.Visibility = Visibility.Collapsed;
            txtSabor.Text = sabor;

            editar = true;
            idedit = id;
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

        public bool ProcesarAgregar()
        {
            if (cmbTipoSabores.SelectedValue != null && !string.IsNullOrEmpty(txtSabor.Text))
            {
                int id = editar ? idedit : -1;

                Sabores sabores = new Sabores
                {
                    FKIDTipoSabor = Convert.ToInt32(cmbTipoSabores.SelectedValue),
                    Sabor = txtSabor.Text.Trim(),
                    ID = id

                };

                if (sabores != null)
                {
                    MI.InserEditSabores(sabores); // Asegúrate que este método exista
                    return true;
                }
            }
            else
            {
                // Resaltar el campo de texto si no se ha ingresado un sabor
                txtSabor.PlaceholderForeground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red);
                txtSabor.PlaceholderText = "Ingresa un Texto Valido o Selecciona Tipo Sabor";
                return false; // No se pudo procesar la inserción
            }

            return false; // Algo falló
        }

    }
}
