using MyMasterMind.Commands;
using MyMasterMind.Interfaces;
using MyMasterMind.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	public partial class CodeField : UserControl, INotifyPropertyChanged, ISetCheckCheckCommandEventHandler
	{
		private MyMasterMindCodeColors Color;

		EventHandler CheckCheckCommandEventHandler;

		Brush colorBrush = DisplayColors.GetCodeBrush(MyMasterMindCodeColors.None);
		public Brush ColorBrush
		{
			private set
			{
				colorBrush = value;
				NotifyPropertyChanged("ColorBrush");
			}

			get
			{
				return colorBrush;
			}
		}

		private SelectColorCommand selectColorCommand;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}


		public ICommand SelectColorCommand
		{ 
			get { return selectColorCommand; }
		}

        #region Constructor
        public CodeField()
		{
			InitializeComponent();
			this.DataContext = this;

			SetColor(MyMasterMindCodeColors.None);
			selectColorCommand = new SelectColorCommand(this);
			DisableMenu();
		}
		#endregion

		public void SetColor( MyMasterMindCodeColors color )
		{
			Color = color;
			ColorBrush = DisplayColors.GetCodeBrush(Color);

			if (CheckCheckCommandEventHandler != null)
				CheckCheckCommandEventHandler(this, null);
		}

		public MyMasterMindCodeColors GetColor()
		{
			return Color;
		}

		public void EnableMenu()
		{
			CodeFieldStackPanel.ContextMenu.IsEnabled = true;
			CodeFieldStackPanel.ContextMenu.Visibility  = Visibility.Visible;
		}

		public void DisableMenu()
		{
			CodeFieldStackPanel.ContextMenu.IsEnabled = false;
			CodeFieldStackPanel.ContextMenu.Visibility = Visibility.Hidden;
		}

		#region ISetCheckCheckCommandEventHandler
		public void SetCheckCheckCommandEventHandler(EventHandler checkCheckCommandEventHandler)
        {
			CheckCheckCommandEventHandler = checkCheckCommandEventHandler;

		}
		#endregion
	}
}
