using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBaseApp.Converters
{
  // מחליף את הלוגיקה במקום ויומודל
  internal class BoolToIconConverter : IValueConverter
  {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value != null)
      {
        bool isTrue = (bool)value;
        return isTrue ? Helper.FontHelper.OPEN_EYE_ICON : Helper.FontHelper.CLOSED_EYE_ICON;
      }
      return Helper.FontHelper.CLOSED_EYE_ICON;
    }


    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
