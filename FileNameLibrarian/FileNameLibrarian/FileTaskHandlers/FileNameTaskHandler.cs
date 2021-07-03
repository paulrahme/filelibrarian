namespace FileNameLibrarian
{
	public abstract class FileTaskHandler
	{
		public abstract string Command { get; }
		public abstract string Description{ get; }

		public abstract int ProcessTask(string baseDir, string pattern);
	}
}
