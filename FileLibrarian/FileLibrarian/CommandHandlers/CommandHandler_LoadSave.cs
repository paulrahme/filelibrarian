using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using LitJson;

namespace FileLibrarian
{
    public class LoadSaveCommon
    {
        /// <summary> Returns the directory for saving/loading </summary>
        public static string GetSaveFolder()
        {
            string productName = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute), false)).Product;

            string saveFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            saveFolder = Path.Combine(saveFolder, productName);

            return saveFolder;
        }

        /// <summary> Converts List of FileEntries into an array of (serializable) SaveData </summary>
        public static FileEntry.SaveData[] AllFilesToSaveDataArray(List<FileEntry> allFiles)
        {
            FileEntry.SaveData[] saveDataArray = new FileEntry.SaveData[allFiles.Count];
            for (int i = 0; i < saveDataArray.Length; ++i)
                saveDataArray[i] = allFiles[i].CreateSaveData();

            return saveDataArray;
        }

        /// <summary> Converts an array of (serializable) SaveData into a List of FileEntries </summary>
        public static List<FileEntry> SaveDataArrayToAllFiles(FileEntry.SaveData[] saveDataArray)
        {
            List<FileEntry> allFiles = new List<FileEntry>();
            for (int i = 0; i < saveDataArray.Length; ++i)
                allFiles.Add(FileEntry.CreateFromSaveData(saveDataArray[i]));

            return allFiles;
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

            var saveDataArray = LoadSaveCommon.AllFilesToSaveDataArray(allFiles);
            string jsonContent = JsonMapper.ToJson(saveDataArray);

            try
            {
                File.WriteAllText(filePath, jsonContent);
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
