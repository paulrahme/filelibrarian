using System;
using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
	public class CommandHandler_Sort : CommandHandler
	{
		public override List<string> Commands => new() { "sort" };
		public override string Description => "Sorts all the filenames into a combined sorted list (type\"help sort\" for more options)";
		public override string Usage => "sort, sort full - sorts the list by full path (directory+filename).\n" +
										"sort filename   - sorts the list by filenames (ignoring parent directory name).\n" +
										"sort dirname    - sorts the list by directory name (ignoring filename).\n" +
										"sort date       - sorts the list by modified date on each file.";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(string[] args, ref List<DirectoryInfo> allFiles, out string output)
		{
			string sorttype = ((args == null) || (args.Length == 0)) ? "full" : args[0];

			switch (sorttype)
			{
				case "full":
					allFiles.Sort(new Comparer_FullPath());
					output = $"Sorted {allFiles.Count} files by full path.";
					return true;

				case "filename":
					allFiles.Sort(new Comparer_FileName());
					output = $"Sorted {allFiles.Count} files by filename.";
					return true;

				case "dirname":
					allFiles.Sort(new Comparer_DirName());
					output = $"Sorted {allFiles.Count} files by directory name.";
					return true;

				case "date":
					allFiles.Sort(new Comparer_ModifiedTime());
					output = $"Sorted {allFiles.Count} files by modified date.";
					return true;

				default:
					output = $"Unhandled sort type \"{sorttype}\".\n{Usage}";
					return false;
			}
		}

		private class Comparer_FullPath : IComparer<DirectoryInfo>
		{
			public int Compare(DirectoryInfo x, DirectoryInfo y) => string.Compare(x.FullName, y.FullName);
		}

		private class Comparer_FileName : IComparer<DirectoryInfo>
		{
			public int Compare(DirectoryInfo x, DirectoryInfo y) => string.Compare(x.Name, y.Name);
		}

		private class Comparer_DirName : IComparer<DirectoryInfo>
		{
			public int Compare(DirectoryInfo x, DirectoryInfo y) => string.Compare(x.Parent.FullName, y.Parent.FullName);
		}

		private class Comparer_ModifiedTime : IComparer<DirectoryInfo>
		{
			public int Compare(DirectoryInfo x, DirectoryInfo y) => DateTime.Compare(x.LastWriteTime, y.LastWriteTime);
		}
	}
}
