using System;
using System.Collections.Generic;
using System.IO;

namespace FileNameLibrarian
{
	public class CommandHandler_Quit : CommandHandler
	{
		public override string Command => "quit";
		public override string Description => "Quits back to the command prompt.";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(string[] args, ref List<DirectoryInfo> allFiles, out string output)
		{
			Environment.Exit(0);
			output = string.Empty;
			return true;
		}
	}

	public class CommandHandler_Exit : CommandHandler_Quit
	{
		public override string Command => "exit";
	}
}
