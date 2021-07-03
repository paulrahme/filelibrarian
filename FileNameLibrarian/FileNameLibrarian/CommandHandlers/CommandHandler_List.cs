using System;
using System.Collections.Generic;
using System.IO;

namespace FileNameLibrarian
{
	public class CommandHandler_List : CommandHandler
	{
		public override string Command => "list";
		public override string Description => "Prints out a list of files, or split columns of files and directories.";
		public override string Usage => "\"list\" lists all files to the console.\n\"" +
										"\"list split\" shows filenames and directories in separate columns.";

		public override void Execute(string[] args, ref List<DirectoryInfo> allFiles)
		{
			bool split = (args.Length > 0) && (args[0] == "split");
			string listStr = string.Empty;

			int fileNameColWidth = 0;
			if (split)
			{
				for (int i = 0; i < allFiles.Count; ++i)
					fileNameColWidth = Math.Max(fileNameColWidth, allFiles[i].Name.Length);
			}

			for (int i = 0; i < allFiles.Count; ++i)
			{ 
				if (split)
					listStr += $"\n{allFiles[i].Name.PadRight(fileNameColWidth)} {allFiles[i].Parent.FullName}";
				else
					listStr += $"\n{allFiles[i].FullName}";
			}

			Console.WriteLine(listStr);
		}
	}
}
