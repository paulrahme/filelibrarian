using System;
using System.Collections.Generic;
using System.IO;

namespace FileNameLibrarian
{
	public class CommandHandler_Sort : CommandHandler
	{
		public override string Command => "sort";
		public override string Description => "Sorts all the filenames into a combined alphanumeric list.";

		public override void Execute(string[] args, ref List<DirectoryInfo> allFiles)
		{
			allFiles.Sort(new SortComparer());
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
