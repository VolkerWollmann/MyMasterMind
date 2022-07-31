namespace MyMasterMind.Interfaces
{
	public enum CellMark
	{
		None = 0,
		ForInput,
		CompareFalse,
		CompareTrue,
	}

	public enum MyMasterMindCommands
	{
		Clear = 0,
		ComputerFast,
		ComputerSlow,
		Cancel,
		User,
		Check,
	}

	public class MyMasterMindBoarViewConstants
    {
		public static int GoodGuessDisplayTime = 400;
		public static int BadGuessDisplayTime = 20;
	}
}
