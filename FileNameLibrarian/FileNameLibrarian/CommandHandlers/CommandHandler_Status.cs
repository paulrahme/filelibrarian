using System;
using System.Collections.Generic;
using System.IO;

namespace FileNameLibrarian
{
	public class CommandHandler_Status : CommandHandler
	{
		public override string Command => "status";
		public override string Description => "Lists status of files.";

		public override void Execute(string[] args, ref List<DirectoryInfo> allFiles)
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
