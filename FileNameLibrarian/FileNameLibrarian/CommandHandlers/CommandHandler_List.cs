using System;
using System.Collections.Generic;
using System.IO;

namespace FileNameLibrarian
{
	public class CommandHandler_List : CommandHandler
	{
		public override string Command => "list";
		public override string Description => "Lists status of files, or prints out files.";
		public override string Usage => "\"list\" shows a status summary of the current files.\n\"list full\" lists all files to the console.";

		public override void Execute(string[] args, ref List<DirectoryInfo> allFiles)
		{
			if ((args.Length > 0) && (args[0] == "full"))
				ListFull(allFiles);
			else
				ListSummary(allFiles);
		}

		void ListFull(List<DirectoryInfo> allFiles)
		{
			string debugStr = string.Empty;

			for (int i = 0; i < allFiles.Count; ++i)
				debugStr += $"\n{allFiles[i].FullName}";

			Console.WriteLine(debugStr);
		}

		void ListSummary(List<DirectoryInfo> allFiles)
		{
			var uniqueDirs = new List<string>();
			foreach(var file in allFiles)
			{
				string dir = file.Parent.FullName;
				if (!uniqueDirs.Contains(dir))
					uniqueDirs.Add(dir);
			}

			Console.WriteLine($"Current list contains '{allFiles.Count}' files in '{uniqueDirs.Count}' directories.");
		}
	}
}
