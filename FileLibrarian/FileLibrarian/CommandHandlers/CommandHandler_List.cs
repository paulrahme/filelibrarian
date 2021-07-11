using System;
using System.Collections.Generic;
using System.IO;
using FileLibrarian.Utils;

namespace FileLibrarian
{
	public class CommandHandler_List : CommandHandler
	{
		public override List<string> Commands => new() { "list" };
		public override string Description => "Prints out the list of all files";
		public override string Usage => "Additional options:\n" +
										"split              - display filenames and directories in separate columns.\n" +
										"size               - display file sizes (in bytes)\n" +
										"size [kb|mb|gb]    - display file sizes (in kilo/mega/gigabytes)";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(List<string> args, ref List<FileInfo> allFiles, out string output)
		{
			output = GetListOutput(args, allFiles);
			return true;
		}

		/// <summary> Generates the listing of all files </summary>
		/// <returns> Text formatted listing </returns>
		public static string GetListOutput(List<string> args, List<FileInfo> allFiles)
		{
			bool split = (args.Contains("split"));

			int sizeArgIdx = args.IndexOf("size");
			Files.SizeTypes sizeType = Files.SizeTypes.None;
			if (sizeArgIdx >= 0)
			{
				string sizeTypeArg = (args.Count > sizeArgIdx + 1) ? args[sizeArgIdx + 1] : null;
				sizeType = sizeTypeArg switch
				{
					"gb" => Files.SizeTypes.Giga,
					"mb" => Files.SizeTypes.Mega,
					"kb" => Files.SizeTypes.Kilo,
					_ => Files.SizeTypes.Bytes
				};
			}

			int fileNameColWidth = 0;
			if (split)
			{
				for (int i = 0; i < allFiles.Count; ++i)
					fileNameColWidth = Math.Max(fileNameColWidth, allFiles[i].Name.Length);
			}

			string output = string.Empty;
			for (int i = 0; i < allFiles.Count; ++i)
			{
				var thisFile = allFiles[i];
				if (sizeType != Files.SizeTypes.None)
				{
					string formattedFileSize = string.Format("{0:N0}", Files.GetSizeByType(thisFile, sizeType));
					output += $"{formattedFileSize,11} ";
				}

				if (split)
					output += $"{thisFile.Name.PadRight(fileNameColWidth)} {thisFile.Directory.FullName}\n";
				else
					output += $"{thisFile.FullName}\n";
			}

			return output;
		}
	}
}
