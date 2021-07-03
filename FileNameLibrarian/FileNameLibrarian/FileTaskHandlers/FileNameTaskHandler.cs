using System.Collections.Generic;
using System.IO;

namespace FileNameLibrarian
{
	public abstract class FileTaskHandler
	{
		public abstract string Command { get; }
		public abstract string Description { get; }

		public abstract int ProcessTask(string baseDir, string pattern);

		protected List<DirectoryInfo> GetAllFiles(string baseDir, string pattern)
		{
			var allFiles = new List<DirectoryInfo>();

			foreach (string file in Directory.GetFiles(baseDir, pattern, SearchOption.AllDirectories))
			{
				allFiles.Add(new DirectoryInfo(file));
			}

			return allFiles;
		}
	}
}
