using System;
using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
	public class CommandHandler_List : CommandHandler
	{
		public override List<string> Commands => new() { "list" };
		public override string Description => "Prints out a list of files, or split columns of files and directories.";
		public override string Usage => "list       - lists all files to the console.\n" +
										"list split - shows filenames and directories in separate columns.";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(string[] args, ref List<DirectoryInfo> allFiles, out string output)
		{
			bool split = (args.Length > 0) && (args[0] == "split");
			output = string.Empty;

			int fileNameColWidth = 0;
			if (split)
			{
				for (int i = 0; i < allFiles.Count; ++i)
					fileNameColWidth = Math.Max(fileNameColWidth, allFiles[i].Name.Length);
			}

			for (int i = 0; i < allFiles.Count; ++i)
			{ 
				if (split)
					output += $"\n{allFiles[i].Name.PadRight(fileNameColWidth)} {allFiles[i].Parent.FullName}";
				else
					output += $"\n{allFiles[i].FullName}";
			}

			return true;
		}
	}
}
