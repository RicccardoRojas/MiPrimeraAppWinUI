using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MiPrimeraAppWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FormularioPage : Page
    {
        public FormularioPage()
        {
            this.InitializeComponent();
        }

        private void btnCantidad_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = null;
        }

        private void btnCantidad_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void btnMas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCantidad.Text))
            {
                txtCantidad.Text = "1";
            }
            else
            {
                txtCantidad.Text = (int.Parse(txtCantidad.Text) + 1).ToString();
            }
            

        }

        private void btnMenos_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCantidad.Text) && int.Parse(txtCantidad.Text) >= 1 )
            {
                txtCantidad.Text = (int.Parse(txtCantidad.Text) - 1).ToString();
            }
        }

        bool _suppressTextChanged = false;

        private void txtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_suppressTextChanged) return;

            var box = (TextBox)sender;
            string original = box.Text;

            string filtered = new string(original.Where(char.IsDigit).ToArray());

            if (filtered != original)
            {
                int selStart = box.SelectionStart;
                int diff = original.Length - filtered.Length;
                int newPos = Math.Max(0, selStart - diff);

                _suppressTextChanged = true;
                box.Text = filtered;
                box.SelectionStart = Math.Min(filtered.Length, newPos);
                _suppressTextChanged = false;
            }
        }


    }
}
