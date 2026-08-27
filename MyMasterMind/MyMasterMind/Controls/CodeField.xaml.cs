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
	public partial class CodeField : INotifyPropertyChanged, ISetEnableCheckCommandEventHandler
	{
		private MyMasterMindCodeColors Color;

		EventHandler EnableCheckCommandEventHandler;

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

            EnableCheckCommandEventHandler?.Invoke(this, null);
        }

		public MyMasterMindCodeColors GetColor()
		{
			return Color;
		}

		public void MarkContribution(MyMasterMindEvaluationColors contribution)
		{
			CodeFieldStackPanel.Background = new SolidColorBrush(Colors.LightGreen);
			CodeFieldRectangle.Stroke = DisplayColors.GetEvaluationBrush(contribution);
			CodeFieldRectangle.StrokeThickness = 4;
		}

		public void MarkOrigin(GeneticGeneOrigin origin)
		{
			switch (origin)
			{
				case GeneticGeneOrigin.FirstParent:
					CodeFieldRectangle.Stroke = new SolidColorBrush(Colors.DodgerBlue);
					break;

				case GeneticGeneOrigin.SecondParent:
					CodeFieldRectangle.Stroke = new SolidColorBrush(Colors.DarkOrange);
					break;

				case GeneticGeneOrigin.Mutation:
					CodeFieldRectangle.Stroke = new SolidColorBrush(Colors.Red);
					CodeFieldRectangle.StrokeDashArray = new DoubleCollection { 2, 1 };
					break;

				default:
					UnmarkContribution();
					return;
			}

			CodeFieldRectangle.StrokeThickness = 4;
		}

		public void UnmarkContribution()
		{
			CodeFieldStackPanel.Background = null;
			CodeFieldRectangle.Stroke = new SolidColorBrush(Colors.DarkMagenta);
			CodeFieldRectangle.StrokeThickness = 2;
			CodeFieldRectangle.StrokeDashArray = null;
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
		public void SetEnableCheckCommandEventHandler(EventHandler enableCheckCommandEventHandler)
        {
			EnableCheckCommandEventHandler = enableCheckCommandEventHandler;

		}
		#endregion
	}
}
