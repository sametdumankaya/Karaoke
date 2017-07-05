namespace Karaoke
{
	public class Song : System.IComparable<Song>
	{
		public static readonly string[] languages = 
		{
			"turkce",
			"ingilizce",
			"almanca",
			"fransizca",
			"cocuk",
			"eglence"
		};

		public static readonly string[] definitions =
		{
			"Türkçe",
			"İngilizce",
			"Almanca",
			"Fransızca",
			"Çocuk Şarkıları",
			"Eğlence Şarkıları"
		};

		public string FilePath { get; set; }
		public string Language { get; set; }

		public int CompareTo(Song other)
		{
			return FilePath.CompareTo(other.FilePath);
		}

		public override string ToString()
		{
			return System.IO.Path.GetFileNameWithoutExtension(FilePath);
		}
	}
}
