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
		Check, // User wants check of his current guess
		ComputerStep, // single step: each click advances the computer by one step
	}

	public class MyMasterMindBoarViewConstants
    {
		public static int GoodGuessDisplayTime = 400;
		public static int BadGuessDisplayTime = 20;
		public static int GenerationDisplayTime = 2000;
	}
}
