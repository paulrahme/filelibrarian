using System.Collections.Generic;

namespace FileLibrarian
{
    public class CommandHandler_History : CommandHandler
    {
        public override List<string> Commands => new() { "history", "hist" };
        public override string Description => "Lists history of commands called so far.";

        /// <summary> Executes the command (see base class comment for more details) </summary>
        public override bool Execute(List<string> args, ref List<FileEntry> allFiles, out string output)
        {
            output = $"...";
            return true;
        }
    }
}
