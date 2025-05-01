//https://www.exercisescsharp.com/binary-files/read-dimensions-of-bmp-image/
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



    public static void ReadBitmapSize (in InputManager.FileData fD, out int width, out int height) {
    //Get FileInfo and create FileStream 
        FileInfo fI = new FileInfo(fD.fullPath);
        FileStream fS = fI.OpenRead();
    //Get the bitmap header, which is 54 bytes long
        Byte[] byteArr_header = new byte[54];
    //Read the file header
        fS.Read(byteArr_header, 0, 54);
    //Validate Bitmap format
    //    if (data[0] != 'B' || data[1] != 'M') return;
    //I don't understand this next bit but I've no reason to assume it's wrong.
        width = byteArr_header[18]
            + byteArr_header[19] * 256
            + byteArr_header[20] * 256 * 256
            + byteArr_header[21] * 256 * 256 * 256;
        height = byteArr_header[22]
            + byteArr_header[23] * 256
            + byteArr_header[24] * 256 * 256
            + byteArr_header[25] * 256 * 256 * 256;
        fS.Close();
        return;
    }
}