using System.Collections.Generic;
using System.IO;

namespace FileNameLibrarian
{
	public abstract class CommandHandler
	{
		public abstract string Command { get; }
		public abstract string Description { get; }
		public virtual string Usage => null;

		public abstract void Execute(string[] args, ref List<DirectoryInfo> allFiles);
	}
}
