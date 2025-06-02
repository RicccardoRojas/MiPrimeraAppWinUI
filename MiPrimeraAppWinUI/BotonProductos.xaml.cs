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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{
    public sealed partial class BotonProductos : UserControl
    {
        public BotonProductos()
        {
            this.InitializeComponent();
        }

        public string Producto
        {
            get => (string)GetValue(ProductoProperty);
            set => SetValue(ProductoProperty, value);
        }

        public static readonly DependencyProperty ProductoProperty =
            DependencyProperty.Register(nameof(Producto), typeof(string), typeof(BotonProductos), new PropertyMetadata("HELADO"));

        public string RutaIcono
        {
            get => (string)GetValue(RutaIconoProperty);
            set => SetValue(RutaIconoProperty, value);
        }

        public static readonly DependencyProperty RutaIconoProperty =
            DependencyProperty.Register(nameof(RutaIcono), typeof(string), typeof(BotonProductos), new PropertyMetadata("Assets/helado-menu.png"));

        public string TagID
        {
            get => (string)GetValue(TagIDProperty);
            set => SetValue(TagIDProperty, value);
        }

        public static readonly DependencyProperty TagIDProperty =
            DependencyProperty.Register(nameof(TagID), typeof(string), typeof(BotonProductos), new PropertyMetadata("0"));

    }
}
