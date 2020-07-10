﻿using MyMasterMind.Interfaces;
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
	/// Interaction logic for MasterMindBoard.xaml
	/// </summary>
	public partial class MasterMindBoard : UserControl, IMasterMindBoardView
	{
		private GuessCell code;
		private GuessCell[] guessCells = new GuessCell[MyMasterMindConstants.ROWS];
		public MasterMindBoard()
		{
			InitializeComponent();

			code = new GuessCell();
			BoardGrid.Children.Add(code);
			Grid.SetColumn(code, 0);
			Grid.SetRow(code, 0);

			for (int i = 0; i < MyMasterMindConstants.ROWS; i++)
			{
				guessCells[i] = new GuessCell();
				BoardGrid.Children.Add(guessCells[i]);
				Grid.SetColumn(guessCells[i], 0);
				Grid.SetRow(guessCells[i], 2 + (9-i));
			}

			code.HideEvaluation();
		}

		public void SetColor(int row, int column, MyMasterMindCodeColors color)
		{
			guessCells[row].SetColor(column, color);
		}

		public void SetEvaluation(int row, int black, int white)
		{
			guessCells[row].SetEvaluation(black, white);
		}
	}
}
