using System;
using System.Collections.Generic;

namespace FileLibrarian
{
    public class CommandHandler_Quit : CommandHandler
    {
        public override List<string> Commands => new() { "quit", "exit" };
        public override string Description => "Quits back to the command prompt.";

        /// <summary> Executes the command (see base class comment for more details) </summary>
        public override CommandResults Execute(List<string> args, ref List<FileEntry> allFiles, List<CommandData> commandHistory, out string output)
        {
            Environment.Exit(0);
            output = string.Empty;
            return CommandResults.Success;
        }
    }
}
