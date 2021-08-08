using System.Collections.Generic;
using System.IO;

namespace FileLibrarian
{
    public abstract class CommandHandler
    {
        public abstract List<string> Commands { get; }
        public abstract string Description { get; }
        public virtual string Usage => null;

        /// <summary> Executes the command with the optional specified arguments </summary>
        /// <param name="args"> Optional additional arguments for this command </param>
        /// <param name="allFiles"> List of all the files for processing </param>
        /// <param name="output"> Output string to get printed to the console after execution </param>
        /// <returns> True if the command executed successfully, false if there was an error </returns>
        public abstract bool Execute(List<string> args, ref List<FileEntry> allFiles, out string output);
    }
}
    