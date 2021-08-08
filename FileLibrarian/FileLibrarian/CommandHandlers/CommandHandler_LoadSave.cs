using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace FileLibrarian
{
    public class LoadSaveCommon
    {
        public static string GetSaveFolder()
        {
            string asmName = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute), false)).Product;

            string saveFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            saveFolder = Path.Combine(saveFolder, asmName);

            return saveFolder;
        }
    }

    public class CommandHandler_Load : CommandHandler
    {
        public override List<string> Commands => new() { "load" };
        public override string Description => "Loads a saved list of File Entries.";
        public override string Usage => "Usage:\n" +
                                        "load          - Presents a list of saved file entries to load from.\n" +
                                        "load filename - Loada a list of saved file entries.";

        /// <summary> Executes the command (see base class comment for more details) </summary>
        public override bool Execute(List<string> args, ref List<FileEntry> allFiles, out string output)
        {
            output = "Not yet implemented!";
            return false;
        }
    }

    public class CommandHandler_Save : CommandHandler
    {
        public override List<string> Commands => new() { "save" };
        public override string Description => "Saves a list of File Entries to disc.";
        public override string Usage => "Usage:\n" +
                                        "save          - Quicksaves the current list of file entries over the last save name.\n" +
                                        "save filename - Saves the current list of file entries under the name given.";

        string _lastSaveName = null;

        /// <summary> Executes the command (see base class comment for more details) </summary>
        public override bool Execute(List<string> args, ref List<FileEntry> allFiles, out string output)
        {
            string saveFolder = LoadSaveCommon.GetSaveFolder();
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            string filePath = null;
            if (args.Count > 0)
                filePath = Path.Combine(saveFolder, args[0] + ".json");
            else
            {
                while (filePath == null)
                {
                    while (string.IsNullOrEmpty(_lastSaveName))
                    {
                        Console.Write("Save name: ");
                        _lastSaveName = Console.ReadLine();
                    }

                    filePath = Path.Combine(saveFolder, _lastSaveName + ".json");
                    if (File.Exists(filePath))
                    {
                        Console.Write($"File '{filePath}' exists. Overwrite? (Y/N)");

                        var keyChar = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                        if ((keyChar != 'y') && (keyChar != 'Y'))
                            filePath = _lastSaveName = null;
                    }
                }
            }

            try
            {
                File.WriteAllText(filePath, "blah"); // TODO: replace with allFiles -> json
                output = $"Saved {allFiles.Count} file entries to '{filePath}'";
                return true;
            }
            catch (Exception e)
            {
                output = $"Error writing to '{filePath}':\n{e.Message}";
                return false;
            }
        }
    }
}
