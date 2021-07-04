using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace FileNameLibrarian
{
	class Program
	{
		static string _baseDir, _filePattern;
		static List<DirectoryInfo> _allFiles = new();
		static List<CommandHandler> _handlers = new();

		/// <summary> App's main entry + exit point </summary>
		/// <param name="args"> Command-line arguments </param>
		static void Main(string[] args)
		{
			// Print title + version
			var version = ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
								Assembly.GetExecutingAssembly(),
								typeof(AssemblyFileVersionAttribute), false)).Version;
			Console.WriteLine($"File Name Librarian v{version}\n");

			Console.Write("Base Directory? [.] ");
			_baseDir = Console.ReadLine();
			if (string.IsNullOrEmpty(_baseDir))
				_baseDir = Directory.GetCurrentDirectory();

			Console.Write("File pattern? [*.*] ");
			_filePattern = Console.ReadLine();

			AddHandlers();
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

		/// <summary> Adds handlers for various file tasks </summary>
		static void AddHandlers()
		{
			_handlers.Add(new CommandHandler_Exit());
			_handlers.Add(new CommandHandler_List());
			_handlers.Add(new CommandHandler_Quit());
			_handlers.Add(new CommandHandler_Sort());
			_handlers.Add(new CommandHandler_Status());
		}

		/// <summary> Scans the directory tree finding all files matching the wildcard pattern </summary>
		static void FindAllFiles()
		{
			_allFiles.Clear();

			foreach (string file in Directory.GetFiles(_baseDir, _filePattern, SearchOption.AllDirectories))
			{
				_allFiles.Add(new DirectoryInfo(file));
			}
		}

		/// <summary> Handles the command by calling the appropriate handler </summary>
		/// <param name="commands"> Array of strings containing the main command + any additional arguments </param>
		static void HandleCommand(string[] commands)
		{
			string command = commands[0];
			string[] args = null;
			if (commands.Length > 0)
			{
				args = new string[commands.Length - 1];
				for (int i = 0; i < args.Length; ++i)
					args[i] = commands[i + 1];
			}
			var handler = _handlers.Find(x => x.Command == command);

			// Special case for help
			if (command == "help")
			{
				if (args is {Length:0 })
				{
					foreach (var helpHandler in _handlers)
						Console.WriteLine($"{helpHandler.Command,-8} - {helpHandler.Description}");
				}
				else
				{
					var helpHandler = _handlers.Find(x => x.Command == args[0]);
					Console.WriteLine(helpHandler.Usage ?? helpHandler.Description);
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
