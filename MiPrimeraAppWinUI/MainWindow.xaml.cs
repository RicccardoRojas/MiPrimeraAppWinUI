using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;

namespace MiPrimeraAppWinUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(InicioPage)); // P�gina inicial
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem item)
            {
                switch (item.Tag)
                {
                    case "inicio":
                        ContentFrame.Navigate(typeof(InicioPage));
                        break;
                    case "formulario":
                        ContentFrame.Navigate(typeof(FormularioPage));
                        break;
                }
            }
        }

        private void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Obtener el NavigationViewItem que fue tocado
            var item = sender as NavigationViewItem;
            var tag = item.Tag.ToString();

            if (tag == "whatsapp")
            {
                // Aqu� va la l�gica cuando el usuario hace clic en WhatsApp
                ContentDialog dialog = new ContentDialog
                {
                    Title = "�Has hecho clic en WhatsApp!",
                    Content = "Aqu� puedes agregar m�s l�gica para WhatsApp.",
                    CloseButtonText = "Cerrar"
                };
                _ = dialog.ShowAsync();
            }
        }
    }
}
