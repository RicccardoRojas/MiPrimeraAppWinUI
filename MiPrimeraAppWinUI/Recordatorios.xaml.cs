using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Input;
using System.Collections.ObjectModel;

namespace MiPrimeraAppWinUI
{

    public sealed partial class Recordatorios : Page
    {
        public ObservableCollection<Recordatorio> RecordatorioList { get; set; } = new ObservableCollection<Recordatorio>();
        public Recordatorios()
        {
            this.InitializeComponent();

            txtEncabeAddRecordatorios.Text = "AGREGAR\nRECORDATORIOS";

            RecordatorioList = GenerarRecordatorio(100);
            DGRecordatorios.ItemsSource = RecordatorioList;
        }

        public class Recordatorio
        {
            public int ID { get; set; }
            public string Estado { get; set; }
            public string Descripcion { get; set; }
            public string Observaciones { get; set; }
        }

        private ObservableCollection<Recordatorio> GenerarRecordatorio(int cantidad)
        {
            var lista = new ObservableCollection<Recordatorio>();
            var random = new Random();

            for (int i = 1; i <= cantidad; i++)
            {
                lista.Add(new Recordatorio
                {
                    ID = i + 1,
                    Estado = "Estado" + (i + 1),
                    Descripcion = "Descripcion " + (i + 1),
                    Observaciones = "Observaciones " + (i + 1)
                }
                );
            }
            return lista;
        }

        private void Botones_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void Botones_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;
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
