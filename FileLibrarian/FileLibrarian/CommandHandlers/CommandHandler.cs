using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
	public abstract class CommandHandler
	{
		public abstract string Command { get; }
		public abstract string Description { get; }
		public virtual string Usage => null;

		/// <summary> Executes the command with the optional specified arguments </summary>
		/// <param name="args"> Optional additional arguments for this command </param>
		/// <param name="allFiles"> List of all the files for reading/modifying </param>
		/// <param name="output"> Output string to get printed to the console after execution </param>
		/// <returns> True if the command executed successfully, false if there was an error </returns>
		public abstract bool Execute(string[] args, ref List<DirectoryInfo> allFiles, out string output);
	}
}
