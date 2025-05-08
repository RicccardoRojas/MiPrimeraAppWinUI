using Microsoft.UI.Xaml.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Microsoft.UI.Input;
using System.Globalization;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistroVenta : Page
    {
        //Grafico de Numero de Ventas
        public ISeries[] SerieNumVentas { get; set; }
        public ObservableCollection<ICartesianAxis> XAxesNumVentas { get; set; }
        public ObservableCollection<ICartesianAxis> YAxesNumVentas { get; set; }

        //Grafico de Cantidad Ganada
        public ISeries[] SerieCantGanada { get; set; }
        public ObservableCollection<ICartesianAxis> XAxesCantGanada { get; set; }
        public ObservableCollection<ICartesianAxis> YAxesCantGanada { get; set; }

        public RegistroVenta()
        {
            this.InitializeComponent();
            txtTituloVentSemana.Text = "VENTAS SEMANALES\nNUM VENTAS";
            txtTituloCantGanada.Text = "VENTAS SEMANALES\nCANTIDAD GANADA";
            GenerarGraficoNumVentas();
            GenerarGraficoCantGanada();
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

       
    }


}
