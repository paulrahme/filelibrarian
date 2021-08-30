using System;
using System.Collections.Generic;

namespace FileLibrarian
{
    public abstract class CommandHandler
    {
        [Flags]
        public enum CommandResults
        {
            Failure = 0,
            Success = 1,
            SaveUndoStep = 2,
            // NextResult = 4, then 8, etc
        }

        public abstract List<string> Commands { get; }
        public abstract string Description { get; }
        public virtual string Usage => null;

        /// <summary> Executes the command with the optional specified arguments </summary>
        /// <param name="args"> Optional additional arguments for this command </param>
        /// <param name="allFiles"> List of all the files for processing </param>
        /// <param name="commandHistory"> All the previous commands that have been called before this one </param>
        /// <param name="output"> Output string to get printed to the console after execution </param>
        /// <returns> Combination of flags, including 'Success' if the command executed without errors </returns>
        public abstract CommandResults Execute(List<string> args, ref List<FileEntry> allFiles, List<CommandData> commandHistory, out string output);
    }

    /// <summary> Data structure for 1 item in the command history </summary>
    public struct CommandData
    {
        public string Command;
        public List<string> Args;
        public List<FileEntry> AllFiles;

        /// <summary> Constructor </summary>
        public CommandData(string command, List<string> args, List<FileEntry> allFiles)
        {
            Command = command;
            Args = args;
            AllFiles = allFiles;
        }
    }

}
