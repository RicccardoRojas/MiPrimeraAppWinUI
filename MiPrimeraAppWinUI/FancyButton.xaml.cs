using System;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.UI.Core;

namespace MiPrimeraAppWinUI
{
    public sealed partial class FancyButton : UserControl
    {

        public FancyButton()
        {
            this.InitializeComponent();
            this.PointerEntered += FancyButton_PointerEntered;
            this.PointerExited += FancyButton_PointerExited;
        }

        // Propiedad BackgroundImage
        public string BackgroundImage
        {
            get { return (string)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register(nameof(BackgroundImage), typeof(string), typeof(FancyButton), new PropertyMetadata(string.Empty));

        // Propiedad IconImage
        public string IconImage
        {
            get { return (string)GetValue(IconImageProperty); }
            set { SetValue(IconImageProperty, value); }
        }

        public static readonly DependencyProperty IconImageProperty =
            DependencyProperty.Register(nameof(IconImage), typeof(string), typeof(FancyButton), new PropertyMetadata(string.Empty));

        // Propiedad Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(FancyButton), new PropertyMetadata(string.Empty));



        private void FancyButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);

            if (ImageScale != null)
            {
                var animationX = new DoubleAnimation
                {
                    To = 1.5,
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };
                var animationY = new DoubleAnimation
                {
                    To = 1.5,
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };

                var sb = new Storyboard();
                sb.Children.Add(animationX);
                sb.Children.Add(animationY);

                Storyboard.SetTarget(animationX, ImageScale);
                Storyboard.SetTargetProperty(animationX, "ScaleX");
                Storyboard.SetTarget(animationY, ImageScale);
                Storyboard.SetTargetProperty(animationY, "ScaleY");

                sb.Begin();
            }
        }

        private void FancyButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;

            if (ImageScale != null)
            {
                var animationX = new DoubleAnimation
                {
                    To = 1.2,
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };
                var animationY = new DoubleAnimation
                {
                    To = 1.2,
                    Duration = new Duration(TimeSpan.FromSeconds(0.2))
                };

                var sb = new Storyboard();
                sb.Children.Add(animationX);
                sb.Children.Add(animationY);

                Storyboard.SetTarget(animationX, ImageScale);
                Storyboard.SetTargetProperty(animationX, "ScaleX");
                Storyboard.SetTarget(animationY, ImageScale);
                Storyboard.SetTargetProperty(animationY, "ScaleY");

                sb.Begin();
            }
        }


    }
}
