using System.IO;

namespace FileLibrarian.Utils
{
	class Files
	{
		public enum SizeTypes { None, Bytes, Kilo, Mega, Giga };

		public static float GetSizeByType(FileInfo file, SizeTypes sizeType)
		{
			float fileSize = file.Length;

			return sizeType switch
			{
				SizeTypes.Kilo => fileSize /= 1024,
				SizeTypes.Mega => fileSize /= 1024 * 1024,
				SizeTypes.Giga => fileSize /= 1024 * 1024 * 1024,
				_ => fileSize
			};
		}
	}
}
