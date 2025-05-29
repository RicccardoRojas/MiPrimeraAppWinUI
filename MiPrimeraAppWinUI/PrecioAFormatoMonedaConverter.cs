using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace MiPrimeraAppWinUI
{
    public class PrecioAFormatoMonedaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double precio)
            {
                return precio.ToString("C", CultureInfo.GetCultureInfo("es-MX"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
