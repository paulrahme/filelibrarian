using System;
using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
	public class CommandHandler_List : CommandHandler
	{
		enum Sizes { None, Bytes, Kilo, Mega, Giga };

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
			Sizes size = Sizes.None;
			if (sizeArgIdx >= 0)
			{
				string sizeTypeArg = (args.Count > sizeArgIdx + 1) ? args[sizeArgIdx + 1] : null;
				size = sizeTypeArg switch
				{
					"gb" => Sizes.Giga,
					"mb" => Sizes.Mega,
					"kb" => Sizes.Kilo,
					_ => Sizes.Bytes
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
				if (size != Sizes.None)
					output += $"{GetFormattedSize(thisFile, size),11} ";

				if (split)
					output += $"{thisFile.Name.PadRight(fileNameColWidth)} {thisFile.Directory.FullName}\n";
				else
					output += $"{thisFile.FullName}\n";
			}

			return output;
		}

		static string GetFormattedSize(FileInfo file, Sizes size)
		{
			float fileSize = file.Length;
			fileSize = size switch
			{
				Sizes.Kilo => fileSize /= 1024,
				Sizes.Mega => fileSize /= 1024 * 1024,
				Sizes.Giga => fileSize /= 1024 * 1024 * 1024,
				_ => fileSize
			};

			return string.Format("{0:N0}", fileSize);
		}
	}
}
