using System;
using System.Collections.Generic;

namespace FileLibrarian
{
	public class CommandHandler_Filter : CommandHandler
	{
		public override List<string> Commands => new() { "filter" };
		public override string Description => "Filters by specified criteria.";
		public override string Usage => "Usage:\n" +
										"filter filename <substring> - Filters the list down by a text match inside the filename.\n" +
										"filter content <substring> - Filters the list down by a text match inside the file's content.";

		enum FilterTypes { filename, content };

		/// <summary> Executes the command (see base class comment for more details) </summary>
		public override bool Execute(List<string> args, ref List<FileEntry> allFiles, out string output)
		{
			if (args.Count < 2)
			{
				output = Usage;
				return false;
			}

			if (Enum.TryParse(args[0], out FilterTypes filterType))
			{
				Console.WriteLine($"Filtering {allFiles.Count} files by {filterType}...");
				output = $"Filtered total files in list from {allFiles.Count} to ";
				Filter(filterType, args[1], ref allFiles);
				output += $"{allFiles.Count}.";
				return true;
			}
			else
			{
				output = $"Unhandled/invalid Filter Type'{args[0]}'.\nSee help for supported types.";
				return false;
			}
		}

		void Filter(FilterTypes filterType, string stringToMatch, ref List<FileEntry> allFiles)
		{
			switch (filterType)
			{

				case FilterTypes.filename: allFiles.RemoveAll(f => !f.DoesFilenameContain(stringToMatch)); break;
				case FilterTypes.content: allFiles.RemoveAll(f => !f.DoesFileContentContain(stringToMatch)); break;

				default:
					throw new NotImplementedException($"Unhandled filter type '{filterType}'.");
			}
		}
	}
}
