namespace Karaoke
{
	public class Song : System.IComparable<Song>
	{
		public static readonly string[] languages = 
		{
			"turkce",
			"ingilizce",
			"cocuk",
			"almanca",
			"fransizca",
			"japonca",
			"korece",
			"rusca",
			"turkceArsiv"
		};

		public static readonly string[] definitions =
		{
			"Türkçe (Turkish)",
			"İngilizce (English)",
			"Çocuk Şarkıları (Child Songs)",
			"Almanca (German)",
			"Fransızca (French)",
			"Japonca (Japanese)",
			"Korece (Korean)",
			"Rusça (Russian)",
			"Türkçe Arşiv"
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
