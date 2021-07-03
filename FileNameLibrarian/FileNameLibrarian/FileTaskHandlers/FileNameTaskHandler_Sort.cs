namespace FileNameLibrarian
{
	public class FileTaskHandler_Sort : FileTaskHandler
	{
		public override string Command => "sort";
		public override string Description => "Sorts all the filenames into a combined alphanumeric list.";

		public override int ProcessTask(string baseDir, string pattern)
		{
			return 0;
		}
	}
}
