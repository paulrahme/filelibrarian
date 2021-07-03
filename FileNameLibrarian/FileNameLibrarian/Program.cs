using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace FileNameLibrarian
{
	class Program
	{
		static List<FileTaskHandler> _handlers = new List<FileTaskHandler>();

		/// <summary> App's main entry + exit point </summary>
		/// <param name="args"> Command-line arguments </param>
		static void Main(string[] args)
		{
			AddHandlers();

			// Print title + version
			var version = ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
								Assembly.GetExecutingAssembly(),
								typeof(AssemblyFileVersionAttribute), false)).Version;
			Console.WriteLine($"File Name Librarian v{version}\n");

			// Check for valid arguments
			if (args.Length < 3)
			{
				string appName = AppDomain.CurrentDomain.FriendlyName;
				if (PrintHelpIfNecessary(args[0], appName))
				{
					Environment.Exit(0);
				}
				else
				{
					Console.WriteLine($"Usage: {appName} <operation> <base-dir> <file-name-pattern>\neg. {appName} sort . *.txt\n\nType '{appName} help' for more detailed help.");
					Environment.Exit(1);
				}
			}

			// Parse arguments
			string command = args[0];
			string baseDir = Path.GetFullPath(args[1]);
			string pattern = args[2];
			Console.WriteLine($"--- Command = {command}, Base dir = '{baseDir}', file name pattern = '{pattern}' ---");

			var handler = _handlers.Find(x => x.Command == command);
			if (handler == null)
			{
				Console.WriteLine($"Invalid command '{command}'.");
				Environment.Exit(2);
			}
			handler.ProcessTask(baseDir, pattern);
		}

		/// <summary> Adds handlers for various file tasks </summary>
		static void AddHandlers()
		{
			_handlers.Add(new FileTaskHandler_Sort());
		}

		/// <summary> Checks the argument and prints the help page if required </summary>
		/// <param name="appName"> App name to use in help page </param>
		/// <param name="arg"> Command-line argument </param>
		/// <returns> True if argument was recognised as a valid help request </returns>
		static bool PrintHelpIfNecessary(string arg, string appName)
		{
			if (arg.ToLower().Contains("help") || (arg == "-h") || (arg == "/?"))
			{
				string helpStr = "Detailed Usage:\n" +
					$"{appName} <operation> <base-dir> <file-name-pattern>\n" +
					$"eg. {appName} sort . *.txt\n\n" +
					"Commands:\n";

				foreach (var handler in _handlers)
				{
					helpStr += $"{handler.Command}: {handler.Description}";
				}
				Console.WriteLine(helpStr);

				return true;
			}

			return false;
		}
	}
}
