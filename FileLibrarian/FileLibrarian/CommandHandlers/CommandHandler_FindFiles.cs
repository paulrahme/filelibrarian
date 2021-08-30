using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
    public class CommandHandler_FindFiles : CommandHandler
    {
        public override List<string> Commands => new() { "findfiles" };
        public override string Description => "Finds files form a specified base directory.";
        public override string Usage => "Usage:\n" +
                                        "findfiles [append] baseDir filePattern - Scans directory 'baseDir' for all files matching 'filePattern'\n" +
                                        " - Note: By default, this overwrites the file list. If 'append' is specified, it will add to the existing list.";

        /// <summary> Executes the command (see base class comment for more details) </summary>
        public override bool Execute(List<string> args, ref List<FileEntry> allFiles, out string output)
        {
            bool append = args.Contains("append");
            if (append)
                args.Remove("append"); // Remove it makes parsing other arguments easier

            if (args.Count < 2)
            {
                output = Usage;
                return false;
            }

            string baseDir = args[0];
            string filePattern = args[1];

            if (!Directory.Exists(baseDir))
            {
                output = $"Could not find base directory '{baseDir}'.";
                return false;
            }

            if (!append)
                allFiles.Clear();

            string[] files = Directory.GetFiles(baseDir, filePattern, SearchOption.AllDirectories);
            int count = files.Length;
            for (int i = 0; i < count; ++i)
                allFiles.Add(new FileEntry(files[i]));

            output = $"Found {files.Length} files matching pattern '{filePattern}' under base directory '{baseDir}'.";
            if (append)
                output += $"\nAppended to existing list, total now = {allFiles.Count}.";
            return true;
        }
    }
}
