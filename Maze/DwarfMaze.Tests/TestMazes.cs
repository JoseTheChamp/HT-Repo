namespace DwarfMaze.Tests
{
	static class TestMazes
	{
		public static readonly string[] Corridor =
		{
			"#S#####",
			"# #   #",
			"# # # #",
			"#   # #",
			"#####F#",
		};

		public static readonly string[] TwoWays =
		{
			"#S#####",
			"#     #",
			"# ### #",
			"#     #",
			"#F#####",
		};

		public static readonly string[] Ring =
		{
			"#######",
			"#     #",
			"# ### #",
			"# # # #",
			"# ### #",
			"#  S  F",
			"#######",
		};
	}
}
