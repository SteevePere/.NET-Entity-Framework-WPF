using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;


namespace Client.src.helpers
{

	public class ProgressToColorConverter : IValueConverter
	{

		public object Convert(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			var progress = (double)value;

			if (progress >= 2.5d)
			{
				return "#8ad14f";
			}

			if (progress < 2.5d)
			{
				return "#fc5678";
			}

			return Brushes.Gray;
		}


		public object ConvertBack(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}

}
