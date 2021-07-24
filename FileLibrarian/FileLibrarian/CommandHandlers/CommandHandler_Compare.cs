using System;
using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
	public class CommandHandler_Compare : CommandHandler
	{
		public override List<string> Commands => new() { "compare", "comp" };
		public override string Description => "Compares file contents.";
		public override string Usage => //"Usage:\n" +
										//" - compare [filename] : Compares all files in the list against specified file (use full path, must be in the list)\n" +
										//" - compare * : Compares all files in the list against each other\n" +
										"Additional options:\n" +
										" - blanklines : Compare blank lines (default = ignore/skip empty lines)";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(List<string> args, ref List<FileEntry> allFiles, out string output)
		{
			bool ignoreEmptyLines = !args.Contains("blanklines");
			try
			{
				for (int i = 0; i < allFiles.Count; ++i)
				{
					var fileI = allFiles[i];
					for (int j = i; j < allFiles.Count; ++j)
					{
						var fileJ = allFiles[j];
							Console.WriteLine($"'{fileI.FileInfo.Name}' vs '{fileJ.FileInfo.Name}': {fileI.CompareWith(fileJ, ignoreEmptyLines)}");
					}
				}

				output = $"Compare all files complete.";
				return true;
			}
			catch (Exception e)
			{
				output = $"Error during compare: '{e.Message}'";
				return false;
			}
		}
	}
}
