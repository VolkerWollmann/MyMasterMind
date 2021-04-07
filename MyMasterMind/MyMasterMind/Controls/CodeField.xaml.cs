using MyMasterMind.Commands;
using MyMasterMind.Interfaces;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MyMasterMind.Controls
{
	/// <summary>
	/// Interaction logic for CodeField.xaml
	/// </summary>
	public partial class CodeField : INotifyPropertyChanged, ISetCheckCheckCommandEventHandler
	{
		private MyMasterMindCodeColors Color;

		EventHandler CheckCheckCommandEventHandler;

		private Brush _ColorBrush = DisplayColors.GetCodeBrush(MyMasterMindCodeColors.None);
		public Brush ColorBrush
		{
			private set
			{
				_ColorBrush = value;
				NotifyPropertyChanged("ColorBrush");
			}

			get => _ColorBrush;
        }

		private readonly SelectColorCommand _SelectColorCommand;

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }


		public ICommand SelectColorCommand => _SelectColorCommand;

        #region Constructor
        public CodeField()
		{
			InitializeComponent();
			this.DataContext = this;

			SetColor(MyMasterMindCodeColors.None);
            _SelectColorCommand = new SelectColorCommand(this);
			DisableMenu();
		}
		#endregion

		public void SetColor( MyMasterMindCodeColors color )
		{
			Color = color;
			ColorBrush = DisplayColors.GetCodeBrush(Color);

            CheckCheckCommandEventHandler?.Invoke(this, null);
        }

		public MyMasterMindCodeColors GetColor()
		{
			return Color;
		}

		public void EnableMenu()
		{
            if (CodeFieldStackPanel.ContextMenu != null)
            {
                CodeFieldStackPanel.ContextMenu.IsEnabled = true;
                CodeFieldStackPanel.ContextMenu.Visibility = Visibility.Visible;
            }
        }

		public void DisableMenu()
		{
            if (CodeFieldStackPanel.ContextMenu != null)
            {
                CodeFieldStackPanel.ContextMenu.IsEnabled = false;
                CodeFieldStackPanel.ContextMenu.Visibility = Visibility.Hidden;
            }
        }

		#region ISetCheckCheckCommandEventHandler
		public void SetCheckCheckCommandEventHandler(EventHandler checkCheckCommandEventHandler)
        {
			CheckCheckCommandEventHandler = checkCheckCommandEventHandler;

		}
		#endregion
	}
}
