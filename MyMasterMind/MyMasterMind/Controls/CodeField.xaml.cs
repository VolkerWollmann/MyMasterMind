using MyMasterMind.Commands;
using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMasterMind.Controls
{
	/// <summary>
	/// Interaction logic for CodeField.xaml
	/// </summary>
	public partial class CodeField : UserControl
	{
		MyMasterMindCodeColors Color;

		private SelectColorCommand selectColorCommand;
		public ICommand SelectColorCommand
		{ 
			get { return selectColorCommand; }
		}

		public CodeField()
		{
			InitializeComponent();
			this.DataContext = this;
			Color = MyMasterMindCodeColors.None;
			selectColorCommand = new SelectColorCommand(this);
		}

		public void SetColor( MyMasterMindCodeColors color )
		{
			Color = color;
			CodeFieldRectangle.Fill = DisplayColors.GetCodeBrush(Color);
		}

		public MyMasterMindCodeColors GetColor()
		{
			return Color;
		}
	}
}
