using System;
using System.Collections.Generic;

namespace FileLibrarian
{
    public class CommandHandler_Undo : CommandHandler
    {
        public override List<string> Commands => new() { "undo" };
        public override string Description => "Reverts the last command run, restoring the file list back to what it was before.";

        /// <summary> Executes the command (see base class comment for more details) </summary>
        public override CommandResults Execute(List<string> args, ref List<FileEntry> allFiles, List<CommandData> commandHistory, out string output)
        {
            if (commandHistory.Count == 0)
            {
                output = "No steps found to undo";
                return CommandResults.Failure;
            }
            else
            {
                CommandData lastCommand = commandHistory[commandHistory.Count - 1];
                output = $"Discarding step '{lastCommand.Command}', restoring file list back from '{allFiles.Count}' to '{lastCommand.AllFiles.Count}'.";
                allFiles = lastCommand.AllFiles;
                commandHistory.RemoveAt(commandHistory.Count - 1);

                return CommandResults.Success;
            }
        }
    }
}
