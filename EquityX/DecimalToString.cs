using System.Globalization;

namespace EquityX
{
    public class DecimalToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valid = Decimal.TryParse((string)value, out decimal val);

            if(valid && val < 0)
            {
                return "Red";
            }
            else
            {
                return "Green";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
