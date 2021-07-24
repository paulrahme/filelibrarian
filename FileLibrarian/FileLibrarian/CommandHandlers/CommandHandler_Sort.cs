using System;
using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
	public class CommandHandler_Sort : CommandHandler
	{
		public override List<string> Commands => new() { "sort" };
		public override string Description => "Sorts all the filenames into a combined sorted list";
		public override string Usage => "sort, sort full - sorts the list by full path (directory+filename).\n" +
										"sort filename   - sorts the list by filenames (ignoring parent directory name).\n" +
										"sort dirname    - sorts the list by directory name (ignoring filename).\n" +
										"sort date       - sorts the list by modified date on each file.\n" +
										"sort size       - sorts the list by file size.";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(List<string> args, ref List<FileEntry> allFiles, out string output)
		{
			string sorttype = (args.Count == 0) ? "full" : args[0];

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

				case "size":
					allFiles.Sort(new Comparer_FileSize());
					output = $"Sorted {allFiles.Count} files by size.";
					return true;

				default:
					output = $"Unhandled sort type \"{sorttype}\".\n{Usage}";
					return false;
			}
		}

		private class Comparer_FullPath : IComparer<FileEntry>
		{
			public int Compare(FileEntry x, FileEntry y) => string.Compare(x.FileInfo.FullName, y.FileInfo.FullName);
		}

		private class Comparer_FileName : IComparer<FileEntry>
		{
			public int Compare(FileEntry x, FileEntry y) => string.Compare(x.FileInfo.Name, y.FileInfo.Name);
		}

		private class Comparer_DirName : IComparer<FileEntry>
		{
			public int Compare(FileEntry x, FileEntry y) => string.Compare(x.FileInfo.Directory.FullName, y.FileInfo.Directory.FullName);
		}

		private class Comparer_ModifiedTime : IComparer<FileEntry>
		{
			public int Compare(FileEntry x, FileEntry y) => DateTime.Compare(x.FileInfo.LastWriteTime, y.FileInfo.LastWriteTime);
		}

		private class Comparer_FileSize : IComparer<FileEntry>
		{
			public int Compare(FileEntry x, FileEntry y) => x.FileInfo.Length.CompareTo(y.FileInfo.Length);
		}
	}
}
