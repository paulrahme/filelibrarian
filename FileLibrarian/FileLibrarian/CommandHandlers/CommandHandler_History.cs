using System.Collections.Generic;
using System.Text;

namespace FileLibrarian
{
    public class CommandHandler_History : CommandHandler
    {
        public override List<string> Commands => new() { "history", "hist" };
        public override string Description => "Lists history of commands called so far.";

        StringBuilder sbuild = new StringBuilder();
        StringBuilder sbuildCmd = new StringBuilder();

        /// <summary> Executes the command (see base class comment for more details) </summary>
        public override CommandResults Execute(List<string> args, ref List<FileEntry> allFiles, List<CommandData> commandHistory, out string output)
        {
            sbuild.Clear();
            sbuild.Append("Command history:\n");
            for (int commandIdx = 0; commandIdx < commandHistory.Count; ++commandIdx)
            {
                sbuildCmd.Clear();
                var commandData = commandHistory[commandIdx];
                bool lastOne = commandIdx == commandHistory.Count - 1;
                sbuildCmd.Append(commandData.Command);
                if (commandData.Args?.Count > 0)
                {
                    sbuildCmd.Append("(");
                    for (int i = 0; i < commandData.Args.Count; ++i)
                    {
                        sbuildCmd.Append(commandData.Args[i]);
                        if (i < commandData.Args.Count - 1)
                            sbuildCmd.Append(", ");
                    }
                    sbuildCmd.Append(")");
                }

                sbuild.Append($"{sbuildCmd,-64}  [");
                sbuild.Append(commandData.AllFiles.Count);
                sbuild.Append(" -> ");
                sbuild.Append(lastOne ? allFiles.Count : commandHistory[commandIdx + 1].AllFiles.Count);
                sbuild.Append(" files]");
                if (!lastOne)
                    sbuild.Append("\n");
            }

            output = sbuild.ToString();
            return CommandResults.Success;
        }
    }
}
