using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;                    // PointerRoutedEventArgs
using Microsoft.UI.Xaml.Media;                    // ScaleTransform
using Microsoft.UI.Xaml.Media.Animation;          // DoubleAnimation, Storyboard
using System;                                     // TimeSpan


namespace MiPrimeraAppWinUI
{
    public sealed partial class InicioPage : Page
    {
        public InicioPage()
        {
            this.InitializeComponent();
        }

        private void ButtonClosed_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);

        }

        private void ButtonClosed_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;

        }

        private void Cerrar_App(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
