using System.IO;

public static class FileReader {

/// <summary>
/// Path must be the full path and end with "/filename.extension".
/// </summary>
    public static string File_ReadContents (string filePath) {
        StreamReader reader = new StreamReader(filePath);
        string contents = reader.ReadToEnd();
        reader.Close();
        return contents;
    }
/// <summary>
/// Path must be the full path and end with "/filename.extension".
/// </summary>
    public static string File_ReadContents (string filePath, out string fileName, out string fileType) {
        GetNameAndExtension(filePath, out fileName, out fileType);
        string fileContents = File_ReadContents(filePath);
        return fileContents;
    }
    public static void GetNameAndExtension (string filePath, out string fileName, out string fileType) {
        fileName = filePath.Substring(filePath.LastIndexOfAny(new char[] {'\\', '/'}) +1);
        fileType = fileName.Substring(fileName.LastIndexOf('.'));
        fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
        return;
    }

}