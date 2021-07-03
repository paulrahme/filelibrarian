using System;
using System.Collections.Generic;
using System.IO;

namespace FileNameLibrarian
{
	public class FileTaskHandler_Sort : FileTaskHandler
	{
		public override string Command => "sort";
		public override string Description => "Sorts all the filenames into a combined alphanumeric list.";

		public override int ProcessTask(string baseDir, string pattern)
		{
			List<DirectoryInfo> allFiles = GetAllFiles(baseDir, pattern);

			string debugStr = "BEFORE SORT:";
			for (int i = 0; i < allFiles.Count; ++i)
				debugStr += $"\n{allFiles[i].FullName}";

			allFiles.Sort(new SortComparer());

			debugStr += "\n\nAFTER SORT:";
			for (int i = 0; i < allFiles.Count; ++i)
				debugStr += $"\n{allFiles[i].FullName}";

			Console.WriteLine(debugStr);

			return 0;
		}

		private class SortComparer : IComparer<DirectoryInfo>
		{
			public int Compare(DirectoryInfo x, DirectoryInfo y)
			{
				return string.Compare(x.Name, y.Name);
			}
		}
	}
}
