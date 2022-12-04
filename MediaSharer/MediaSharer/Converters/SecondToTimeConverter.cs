using System;
using Microsoft.UI.Xaml.Data;

namespace MediaSharer.Converters
{
    public class SecondToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => TimeSpan.FromSeconds((double)value).ToString("hh\\:mm\\:ss");

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
