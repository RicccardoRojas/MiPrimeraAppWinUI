using Microsoft.UI.Xaml.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Microsoft.UI.Input;
using CommunityToolkit.WinUI.UI.Controls;
using System.Globalization;
using System;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using ManejadoresPaleteria;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistroVenta : Page
    {
        //Prueba datagrid
        //Grafico de Numero de Ventas
        public ISeries[] SerieNumVentas { get; set; }
        public ObservableCollection<ICartesianAxis> XAxesNumVentas { get; set; }
        public ObservableCollection<ICartesianAxis> YAxesNumVentas { get; set; }

        //Grafico de Cantidad Ganada
        public ISeries[] SerieCantGanada { get; set; }
        public ObservableCollection<ICartesianAxis> XAxesCantGanada { get; set; }
        public ObservableCollection<ICartesianAxis> YAxesCantGanada { get; set; }

        ManejadorRegistroVenta MR;
        public RegistroVenta()
        {
            this.InitializeComponent();
            MR = new ManejadorRegistroVenta();

            txtTituloVentSemana.Text = "VENTAS SEMANALES\nNUM VENTAS";
            txtTituloCantGanada.Text = "VENTAS SEMANALES\nCANTIDAD GANADA";
            GenerarGraficoNumVentas();
            GenerarGraficoCantGanada();

            cmbTipoRegistro.SelectedIndex = 0; // Seleccionar el primer elemento por defecto
            MiDataGrid.ItemsSource = MR.ObtenerRegistroVenta(cmbTipoRegistro.SelectedItem.ToString());
        }

        public void GenerarGraficoNumVentas()
        {
            var valores = new double[] { 10, 15, 20, 5, 12, 8, 17 };
            var dias = new[] { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom" };

            // Serie de barras
            SerieNumVentas = new ISeries[]
            {
            new ColumnSeries<double>
            {
                Values = valores,
                Name = "Cantidad",
                DataLabelsSize = 14,
                DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                DataLabelsFormatter = point => point.Model.ToString()
            }
            };

            // Eje X con los días (usando ObservableCollection<ICartesianAxis>)
            XAxesNumVentas = new ObservableCollection<ICartesianAxis>
        {
            new Axis
            {
                Labels = dias,
                LabelsRotation = 0,
                TextSize = 16
            }
        };

            // Eje Y con las cantidades (usando ObservableCollection<ICartesianAxis>)
            YAxesNumVentas = new ObservableCollection<ICartesianAxis>
        {
            new Axis
            {
                Name = "Cantidad",
                TextSize = 16
            }
        };

            // Asignar DataContext
            this.DataContext = this;
        }



        public void GenerarGraficoCantGanada()
        {
            // Valores numéricos
            var valores = new double[] { 1500.00, 950.00, 500.00, 2100.00, 1200.00, 980.00, 467.00 };
            var dias = new[] { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom" };

            SerieCantGanada = new ISeries[]
            {
        new ColumnSeries<double>
        {
            Values = valores,
            Name = "Cantidad Ganada",
            DataLabelsSize = 14,
            DataLabelsPaint = new SolidColorPaint(SKColors.Black),
            DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,

            // Mostrar formato tipo moneda
            DataLabelsFormatter = point => $"${point.Model:N2}"
        }
            };

            XAxesCantGanada = new ObservableCollection<ICartesianAxis>
    {
        new Axis
        {
            Labels = dias,
            LabelsRotation = 0,
            TextSize = 16
        }
    };

            YAxesCantGanada = new ObservableCollection<ICartesianAxis>
    {
        new Axis
        {
            Name = "Monto ($)",
            TextSize = 16
        }
    };

            this.DataContext = this;
        }

        private void btnPDF_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void btnPDF_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;
        }

        private void Botones_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;
        }

        private void Botones_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        public class Producto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public double Precio { get; set; }
        }

        private ObservableCollection<Producto> GenerarProductos(int cantidad)
        {
            var lista = new ObservableCollection<Producto>();
            var nombres = new[] { "Manzana", "Plátano", "Naranja", "Pera", "Sandía", "Melón", "Durazno", "Fresa", "Kiwi", "Mango" };
            var random = new Random();

            for (int i = 1; i <= cantidad; i++)
            {
                lista.Add(new Producto
                {
                    Id = i,
                    Nombre = nombres[random.Next(nombres.Length)] + $" #{i}",
                    Precio = Math.Round(random.NextDouble() * 50 + 5, 2) // Precio entre 5.00 y 55.00
                });
            }

            return lista;
        }

        private void btnFlecha_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current is App app && app.MainWindow is MainWindow mainWindow)
            {
                mainWindow.NavegarAPagina("inicio");
            }
        }

    }
    


}
