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


        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var scaleTransform = ImageScale;
            if (scaleTransform != null)
            {
                var animationX = new DoubleAnimation
                {
                    To = 1.5, // Aumenta el zoom al 150%
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };
                var animationY = new DoubleAnimation
                {
                    To = 1.5, // Aumenta el zoom al 150%
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };

                var sb = new Storyboard();
                sb.Children.Add(animationX);
                sb.Children.Add(animationY);

                Storyboard.SetTarget(animationX, scaleTransform);
                Storyboard.SetTargetProperty(animationX, "ScaleX");
                Storyboard.SetTarget(animationY, scaleTransform);
                Storyboard.SetTargetProperty(animationY, "ScaleY");

                sb.Begin();
            }
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var scaleTransform = ImageScale;
            if (scaleTransform != null)
            {
                var animationX = new DoubleAnimation
                {
                    To = 1.2, // Vuelve al tamaño original
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };
                var animationY = new DoubleAnimation
                {
                    To = 1.2, // Vuelve al tamaño original
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };

                var sb = new Storyboard();
                sb.Children.Add(animationX);
                sb.Children.Add(animationY);

                Storyboard.SetTarget(animationX, scaleTransform);
                Storyboard.SetTargetProperty(animationX, "ScaleX");
                Storyboard.SetTarget(animationY, scaleTransform);
                Storyboard.SetTargetProperty(animationY, "ScaleY");

                sb.Begin();
            }
        }




    }
}
