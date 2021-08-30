using System;
using System.Collections.Generic;

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
        public override CommandResults Execute(List<string> args, ref List<FileEntry> allFiles, List<CommandData> commandHistory, out string output)
        {
            output = GetListOutput(args, allFiles);
            return CommandResults.Success;
        }

        /// <summary> Generates the listing of all files </summary>
        /// <returns> Text formatted listing </returns>
        public static string GetListOutput(List<string> args, List<FileEntry> allFiles)
        {
            bool split = (args.Contains("split"));

            int sizeArgIdx = args.IndexOf("size");
            FileEntry.SizeTypes sizeType = FileEntry.SizeTypes.None;
            if (sizeArgIdx >= 0)
            {
                string sizeTypeArg = (args.Count > sizeArgIdx + 1) ? args[sizeArgIdx + 1] : null;
                sizeType = sizeTypeArg switch
                {
                    "gb" => FileEntry.SizeTypes.Giga,
                    "mb" => FileEntry.SizeTypes.Mega,
                    "kb" => FileEntry.SizeTypes.Kilo,
                    _ => FileEntry.SizeTypes.Bytes
                };
            }

            int fileNameColWidth = 0;
            if (split)
            {
                for (int i = 0; i < allFiles.Count; ++i)
                    fileNameColWidth = Math.Max(fileNameColWidth, allFiles[i].FileInfo.Name.Length);
            }

            string output = string.Empty;
            for (int i = 0; i < allFiles.Count; ++i)
            {
                var thisFile = allFiles[i];
                if (sizeType != FileEntry.SizeTypes.None)
                {
                    string formattedFileSize = string.Format("{0:N0}", thisFile.GetSizeByType(sizeType));
                    output += $"{formattedFileSize,11} ";
                }

                if (split)
                    output += $"{thisFile.FileInfo.Name.PadRight(fileNameColWidth)} {thisFile.FileInfo.Directory.FullName}\n";
                else
                    output += $"{thisFile.FileInfo.FullName}\n";
            }

            return output;
        }
    }
}
