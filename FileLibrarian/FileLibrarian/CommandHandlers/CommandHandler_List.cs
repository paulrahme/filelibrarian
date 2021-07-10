using System;
using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
	public class CommandHandler_List : CommandHandler
	{
		enum Sizes { None, Bytes, Kilo, Mega, Giga };

		public override List<string> Commands => new() { "list" };
		public override string Description => "Prints out a list of files, or split columns of files and directories.";
		public override string Usage => "list                - lists all files to the console.\n" +
										" Additional options:\n" +
										" split              - display filenames and directories in separate columns.\n" +
										" size               - display file sizes (in bytes)\n" +
										" size [kb|mb|gb]    - display file sizes (in kilo/mega/gigabytes)";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(List<string> args, ref List<FileInfo> allFiles, out string output)
		{
			bool split = (args.Contains("split"));

			Sizes size;
			if (args.Contains("size"))
			{
				if (args.Contains("gb"))
					size = Sizes.Giga;
				else if (args.Contains("mb"))
					size = Sizes.Mega;
				else if (args.Contains("kb"))
					size = Sizes.Kilo;
				else
					size = Sizes.Bytes;
			}
			else
				size = Sizes.None;

			int fileNameColWidth = 0;
			if (split)
			{
				for (int i = 0; i < allFiles.Count; ++i)
					fileNameColWidth = Math.Max(fileNameColWidth, allFiles[i].Name.Length);
			}

			output = string.Empty;
			for (int i = 0; i < allFiles.Count; ++i)
			{
				var thisFile = allFiles[i];
				output += $"\n{GetFormattedSize(thisFile, size),-14}";

				if (split)
					output += $"{thisFile.Name.PadRight(fileNameColWidth)} {thisFile.Directory.FullName}";
				else
					output += $"{thisFile.FullName}";
			}

			return true;
		}

		string GetFormattedSize(FileInfo file, Sizes size)
		{
			float fileSize = file.Length;
			switch (size)
			{
				case Sizes.Bytes: return file.Length.ToString();
				case Sizes.Kilo: return (fileSize / 1024).ToString("0.000");
				case Sizes.Mega: return (fileSize / 1024 / 1024).ToString("0.000");
				case Sizes.Giga: return (fileSize / 1024 / 1024 / 1024).ToString("0.000");

				default:
					return string.Empty;
			}
		}
	}
}
