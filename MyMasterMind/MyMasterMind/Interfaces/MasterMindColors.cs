using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyMasterMind.Interfaces
{
	public enum MyMasterMindColors
	{
		Gray=0,
		White,
		Black,
		Red,
		Green,
		Blue,
		Yellow,
		Magenta,
		Cyan
	}

	public class DisplayColors
	{
		private static DisplayColors singleton = null;
		private Brush[] Brushes;

		public static Brush GetBrush(MyMasterMindColors color)
		{
			if (singleton == null)
				singleton = new DisplayColors();

			return singleton.Brushes[(int)color];
	    }

		private DisplayColors()
		{
			Brushes = new Brush[9];
			Brushes[(int)MyMasterMindColors.Gray]		= new SolidColorBrush(Colors.Gray);
			Brushes[(int)MyMasterMindColors.White]		= new SolidColorBrush(Colors.White);
			Brushes[(int)MyMasterMindColors.Black]		= new SolidColorBrush(Colors.Black);
			Brushes[(int)MyMasterMindColors.Red]		= new SolidColorBrush(Colors.Red);
			Brushes[(int)MyMasterMindColors.Green]		= new SolidColorBrush(Colors.Green);
			Brushes[(int)MyMasterMindColors.Blue]		= new SolidColorBrush(Colors.Blue);
			Brushes[(int)MyMasterMindColors.Yellow]		= new SolidColorBrush(Colors.Yellow);
			Brushes[(int)MyMasterMindColors.Magenta]	= new SolidColorBrush(Colors.Magenta);
			Brushes[(int)MyMasterMindColors.Cyan]		= new SolidColorBrush(Colors.Cyan);
		}
	}

}
