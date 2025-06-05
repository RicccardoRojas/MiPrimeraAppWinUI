using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;

namespace MiPrimeraAppWinUI
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(InicioPage)); // Página inicial

            var appWindow = this.AppWindow;
            var overlapped = (OverlappedPresenter)appWindow.Presenter;
            overlapped.Maximize();
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
                    case "registroventa":
                        ContentFrame.Navigate(typeof(RegistroVenta));
                        break;
                    case "inventarioproducto":
                        ContentFrame.Navigate(typeof(InventarioProductos));
                        break;
                    case "inventario":
                        ContentFrame.Navigate(typeof(Inventario));
                        break;
                    case "gastos":
                        ContentFrame.Navigate(typeof(Gastos));
                        break;
                    case "recordatorios":
                        ContentFrame.Navigate(typeof(Recordatorios));
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
                // Aquí va la lógica cuando el usuario hace clic en WhatsApp
                ContentDialog dialog = new ContentDialog
                {
                    Title = "¡Has hecho clic en WhatsApp!",
                    Content = "Aquí puedes agregar más lógica para WhatsApp.",
                    CloseButtonText = "Cerrar"
                };
                _ = dialog.ShowAsync();
            }
        }

        public void NavegarAPagina(string tag)
        {
            Type pageType = tag switch
            {
                "inicio" => typeof(InicioPage),
                "formulario" => typeof(FormularioPage),
                "registroventa" => typeof(RegistroVenta),
                "inventarioproducto" => typeof(InventarioProductos),
                "inventario" => typeof(Inventario),
                "gastos" => typeof(Gastos),
                "recordatorios" => typeof(Recordatorios),
                _ => null
            };

            if (pageType != null)
            {
                ContentFrame.Navigate(pageType);

                foreach (var item in NavView.MenuItems)
                {
                    if (item is NavigationViewItem navItem && (string)navItem.Tag == tag)
                    {
                        NavView.SelectedItem = navItem;
                        break;
                    }
                }
            }
        }

    }
}
