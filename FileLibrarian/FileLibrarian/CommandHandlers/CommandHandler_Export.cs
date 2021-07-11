using System;
using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
	public class CommandHandler_Export : CommandHandler
	{
		public override List<string> Commands => new() { "export" };
		public override string Description => "Exports the list in a specified file format";
		public override string Usage => "Usage: 'export filename.xxx'\n" +
										"filename's extension can be one of the following formats:\n" +
										".txt	- text file, columns lined up using spaces (can include additional options from 'list', eg. size, split, etc)\n" +
										".csv	- comma-separated-value file, for importing into spreadsheets";

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(List<string> args, ref List<FileInfo> allFiles, out string output)
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
					ExportCSV(exportFileInfo.FullName, allFiles);
					output = $"Wrote file list in csv format to '{exportFileInfo.FullName}'.";
					return true;

				default:
					output = $"Unhandled/invalid file extension '{exportFileInfo.Extension}'.\nSee help for supported output formats.";
					return false;
			}
		}

		void ExportCSV(string filename, List<FileInfo> allFiles)
		{
			File.WriteAllText(filename, "WIP...");
		}
	}
}
