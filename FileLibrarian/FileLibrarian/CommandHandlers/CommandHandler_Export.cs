using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileLibrarian
{
	public class CommandHandler_Export : CommandHandler
	{
		public override List<string> Commands => new() { "export" };
		public override string Description => "Exports the list in a specified file format";
		public override string Usage => "Usage: 'export filename.xxx'\n" +
										"filename's extension can be one of the following formats:\n" +
										".txt	- text file, columns lined up using spaces (can include additional options from 'list', eg. size, split, etc)\n" +
										".csv	- comma-separated-value file, for viewing as / importing into spreadsheets";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(List<string> args, ref List<FileEntry> allFiles, out string output)
		{
			if (args.Count == 0)
			{
				output = Usage;
				return false;
			}

			var exportFileInfo = new FileInfo(args[0]);
			switch (exportFileInfo.Extension)
			{
				case ".txt":
					File.WriteAllText(exportFileInfo.FullName, CommandHandler_List.GetListOutput(args, allFiles));
					output = $"Wrote file list in text format to '{exportFileInfo.FullName}'.";
					return true;

				case ".csv":
					ExportCSV(exportFileInfo.FullName, allFiles, out output);
					return true;

				default:
					output = $"Unhandled/invalid file extension '{exportFileInfo.Extension}'.\nSee help for supported output formats.";
					return false;
			}
		}

		/// <summary> Exports details of all files in separate columns in CSV format </summary>
		/// <param name="filename"> Output .csv file to write </param>
		/// <param name="allFiles"> List of all files to process </param>
		void ExportCSV(string filename, List<FileEntry> allFiles, out string output)
		{
			Console.Write("Generating CSV...");
			StringBuilder contents = new StringBuilder("Filename,Size (bytes),Directory,Full Path\n");

			int count = allFiles.Count; // cached for performance
			for (int i = 0; i < count; ++i)
			{
				var thisFile = allFiles[i];
				contents.Append("\""); contents.Append(thisFile.FileInfo.Name); contents.Append("\""); contents.Append(",");
				contents.Append(thisFile.GetSizeByType(FileEntry.SizeTypes.Bytes)); contents.Append(",");
				contents.Append("\""); contents.Append(thisFile.FileInfo.Directory); contents.Append("\""); contents.Append(",");
				contents.Append("\""); contents.Append(thisFile.FileInfo.FullName); contents.Append("\""); contents.Append("\n");

				if (i << 1 == count)
					Console.Write("50%...");
			}
			Console.WriteLine("done. Writing file...");

			File.WriteAllText(filename, contents.ToString());

			output = $"Wrote {contents.Length} bytes to '{filename}'";
		}
	}
}
