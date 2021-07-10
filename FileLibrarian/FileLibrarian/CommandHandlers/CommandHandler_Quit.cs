using System;
using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
	public class CommandHandler_Quit : CommandHandler
	{
		public override List<string> Commands => new() { "quit", "exit" };
		public override string Description => "Quits back to the command prompt.";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(string[] args, ref List<DirectoryInfo> allFiles, out string output)
		{
			Environment.Exit(0);
			output = string.Empty;
			return true;
		}
	}
}
