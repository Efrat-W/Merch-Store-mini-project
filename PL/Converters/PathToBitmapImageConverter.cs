using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PL.Converters;

internal class PathToBitmapImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            string imageName=(string)value;
            string currentDir = Environment.CurrentDirectory[..^4];
            string imageFullPath = currentDir + "\\PL\\" + imageName;
            BitmapImage image=new BitmapImage(new Uri(imageFullPath));
            return image;
        }
        catch
        {
            string imageName = @"\Images\background.jpg";
            string currentDir = Environment.CurrentDirectory[..^4];
            string imageFullPath = currentDir + "\\PL\\"+ imageName;
            BitmapImage image = new BitmapImage(new Uri(imageFullPath));
            return image;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
