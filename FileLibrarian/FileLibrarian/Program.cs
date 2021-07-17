using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace FileLibrarian
{
	class Program
	{
		/// <summary> All handlers for commands - add any new ones here </summary>
		static readonly List<CommandHandler> _handlers = new()
		{
			new CommandHandler_Export(),
			new CommandHandler_List(),
			new CommandHandler_Quit(),
			new CommandHandler_Sort(),
			new CommandHandler_Status(),
		};

		#region Member variables

		static string _baseDir = ".";
		static string _filePattern = "*.*";
		static List<FileInfo> _allFiles = new();

		#endregion

		/// <summary> App's main entry + exit point </summary>
		/// <param name="args"> Command-line arguments </param>
		static void Main(string[] args)
		{
			// Print title + version
			var version = ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
								Assembly.GetExecutingAssembly(),
								typeof(AssemblyFileVersionAttribute), false)).Version;
			Console.WriteLine($"File Librarian v{version}\n");

			GetBaseDirAndFilePattern(args);
			FindAllFiles();
			HandleCommand(new[] { "status" });
			Console.WriteLine("\nEnter command, or type \"help\" to see a list of commands.");

			while (true)
			{
				// Read next command
				Console.Write("? ");
				string[] commands = Console.ReadLine().Split(' ');
				if (commands.Length == 0)
					break;

				// Find handler for command
				HandleCommand(commands);
			}
		}

		/// <summary> Gets the base directory + file pattern to search for </summary>
		/// <param name="args"> Command-line arguments, if any </param>
		static void GetBaseDirAndFilePattern(string[] args)
		{
			// Base dir passed in as argument?
			bool dirExists = false;
			if (args?.Length > 0)
			{
				if (Directory.Exists(args[0]))
				{
					dirExists = true;
					_baseDir = args[0];
					Console.WriteLine($"Using base directory '{_baseDir}' from command-line.");
				}
				else
					Console.WriteLine($"Invalid base directory '{args[0]}' from command-line. \nUsage: {AppDomain.CurrentDomain.FriendlyName} baseDir filePattern\n");
			}

			// Prompt for base dir
			while (!dirExists)
			{
				Console.Write($"Base Directory? [{_baseDir}] ");
				string input = Console.ReadLine();
				if (!string.IsNullOrEmpty(input))
					_baseDir = input;

				dirExists = Directory.Exists(_baseDir);
				if (!dirExists)
					Console.WriteLine($"Directory '{_baseDir}' does not exist.");
			}

			// File pattern passed in as argument?
			if (args?.Length > 1)
			{
				_filePattern = args[1];
				Console.WriteLine($"Using file pattern '{_filePattern}' from command-line.");
			}
			else
			{
				// Prompt for file pattern
				Console.Write($"File pattern? [{_filePattern}] ");
				_filePattern = Console.ReadLine();
			}
		}

		/// <summary> Scans the directory tree finding all files matching the wildcard pattern </summary>
		static void FindAllFiles()
		{
			_allFiles.Clear();

			string[] files = Directory.GetFiles(_baseDir, _filePattern, SearchOption.AllDirectories);
			int count = files.Length;
			for (int i = 0; i < count; ++i)
				_allFiles.Add(new FileInfo(files[i]));
		}

		/// <summary> Handles the command by calling the appropriate handler </summary>
		/// <param name="commands"> Array of strings containing the main command + any additional arguments </param>
		static void HandleCommand(string[] commands)
		{
			string command = commands[0];

			var args = new List<string>();
			for (int i = 1; i < commands.Length; ++i)
				args.Add(commands[i]);

			var handler = _handlers.Find(x => x.Commands.Contains(command));

			// Special case for help
			if (command == "help")
			{
				if (args.Count == 0)
				{
					foreach (var helpHandler in _handlers)
					{
						string allCommands = helpHandler.Commands[0];
						for (int i = 1; i < helpHandler.Commands.Count; ++i)
							allCommands += $", { helpHandler.Commands[i]}";

						string helpStr = $"{allCommands,-12} - {helpHandler.Description}";
						if (helpHandler.Usage != null)
							helpStr += $" (type 'help {helpHandler.Commands[0]}' for more options)";
						
						Console.WriteLine(helpStr);
					}
				}
				else
				{
					var helpHandler = _handlers.Find(x => x.Commands.Contains(args[0]));
					if (helpHandler != null)
						Console.WriteLine(helpHandler.Usage ?? helpHandler.Description);
					else
						Console.WriteLine($"No help found for \"{args[0]}\".");
				}
			}
			else
			{
				// Execute command if valid
				if (handler != null)
				{
					if (handler.Execute(args, ref _allFiles, out string output))
						Console.WriteLine(output);
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"Error: {output}");
						Console.ResetColor();
					}
				}
				else
					Console.WriteLine($"Invalid command '{command}'. Type \"help\" to see available commands.");
			}
		}
	}
}
