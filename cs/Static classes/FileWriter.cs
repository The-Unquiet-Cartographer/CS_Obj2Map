using System.Text;

public static class FileWriter {

/// <summary>
/// Path must be the full path and end with "/filename.extension".
/// </summary>
	public static void WriteFile (string path, string data) {
		using (FileStream newFile = File.Create (path)) {
			byte[] encodedData = new UTF8Encoding(true).GetBytes(data);
			newFile.Write (encodedData, 0, encodedData.Length);
		};
	}
}