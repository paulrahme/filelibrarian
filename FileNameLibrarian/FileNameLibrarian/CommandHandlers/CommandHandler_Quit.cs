using System;
using System.Collections.Generic;
using System.IO;

namespace FileNameLibrarian
{
	public class CommandHandler_Quit : CommandHandler
	{
		public override string Command => "quit";
		public override string Description => "Quits back to the command prompt.";

		public override void Execute(string[] args, ref List<DirectoryInfo> allFiles)
		{
			Environment.Exit(0);
		}
	}

	public class CommandHandler_Exit : CommandHandler_Quit
	{
		public override string Command => "exit";
	}
}
