using ExtensionMethods;
using System.Drawing;

public static class InputManager {

//////////////////////////////////////////////////
//
//  FILEDATA STRUCT AND FILETYPE ENUM
//
//////////////////////////////////////////////////
    public struct FileData {
        public string directory;
        public string fileName;
        public FileType fileType;
        public string fileContents;
        public FileData (string directory, string fileName, FileType fileType, string fileContents) {
            this.directory = directory;
            this.fileName = fileName;
            this.fileType = fileType;
            this.fileContents = fileContents;
        }
        public string fullName {get{return fileName+"."+fileExtension;}}
        public string fullPath {get{return directory+"/"+fileName+"."+fileExtension;}}
        public string fileExtension {get{return fileType.ToString();}}
    }
    private static List<FileData> ListFileData (string input) {
        string[] inputs = input.Split('"', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return ListFileData_Recursive(inputs);
    }
    private static List<FileData> ListFileData_Recursive (string[] inputs) {
        List<FileData> fileData = new List<FileData>();
        for (int i = 0; i < inputs.Length; i++) {
            string s = inputs[i];
            if (Directory.Exists(inputs[i])) {
                List<string> l = new List<string>();
                l.AddRange(Directory.GetDirectories(inputs[i]));
                l.AddRange(Directory.GetFiles(inputs[i]));
                fileData.AddRange(ListFileData_Recursive(l.ToArray()));
            }
            else if (File.Exists(inputs[i])) {
                string fileName;
                string fileExtension;
                FileReader.GetNameAndExtension(inputs[i], out fileName, out fileExtension);
                FileType? fileType;
                if (FileTypeIsValid(fileExtension, out fileType)) {
                    string directory = inputs[i].Remove( inputs[i].LastIndexOfAny(new char[] {'\\', '/'}) );
                    string fileContents = FileReader.File_ReadContents(inputs[i]);
                    fileData.Add(new FileData(directory, fileName, fileType!.Value, fileContents));
                }
            }
        }
        return fileData;
    }
    public enum FileType {
        obj, mtl, bmp //map
    }
    public static bool FileTypeIsValid (string fileExtension) {
        if (fileExtension[0] == '.') fileExtension = fileExtension.Remove(0,1);
        FileType _out;
        return Enum.TryParse<FileType>(fileExtension, out _out);
    }
    public static bool FileTypeIsValid (string fileExtension, out FileType? FileType) {
        if (fileExtension[0] == '.') fileExtension = fileExtension.Remove(0,1);
        FileType _out;
        bool b = Enum.TryParse<FileType>(fileExtension, out _out);
        if (!b) FileType = null;
        else FileType = _out;
        return b;
    }
    public static bool FileTypeIsModel (FileType fileType) {
        return fileType == FileType.obj;
    }
    public static bool FileTypeIsMaterialLibrary (FileType fileType) {
        return fileType == FileType.mtl;
    }
    public static bool FileTypeIsBitmap (FileType fileType) {
        return fileType == FileType.bmp;
    }



//////////////////////////////////////////////////
//
//  PROCESS INPUT
//
//////////////////////////////////////////////////
    public static bool Import_AsSingle (string input, GameSettings gameSettings, GeneralSettings generalSettings, out Model model, out MaterialLibrary materialLibrary, out string outputDirectory, out int filesConverted) {
        List<FileData> fileData = ListFileData(input);
        List<FileData> modelFiles = fileData.Where(x => FileTypeIsModel(x.fileType)).ToList();
        materialLibrary = new MaterialLibrary(fileData);
        model = Model.Amalgamate(fileData.ToArray(), materialLibrary);
        if (model.isEmpty) {
            outputDirectory = "";
            filesConverted = 0;
            ConsoleManager.ResetConsole(ConsoleColor.Red, "Bad input (no valid files)");
            return false;
        }
        //model.TrimUnused();
        if (fileData.Count == 1) model.name = fileData[0].fileName;
    //These settings work
        if (gameSettings.scaleFactor != 1) model.Scale(gameSettings.scaleFactor);
        if (generalSettings.rounding != 0) model.RoundVertices(generalSettings.rounding);
        if (gameSettings.reverseVertexOrder) model.ReverseVertexOrder();
        if (gameSettings.swapYZCoordinates) model.SwapYZCoordinates();
    //These settings are untested
        if (gameSettings.marathonCeilingFix) MarathonFix.Quick(model);
        if (gameSettings.subdivideFaces) model.SubdivideFaces();
    //OUTPUT
        outputDirectory = fileData.Find(x => x.directory != "").directory;
        filesConverted = fileData.Count;
        return true;
    }


    public static bool Import_AsMultiple (string input, GameSettings gameSettings, GeneralSettings generalSettings, out Model[] models, out MaterialLibrary[] materialLibraries, out string[] outputDirectories) {
        List<FileData> fileData = ListFileData(input);
        List<FileData> modelFiles = fileData.Where(fD => FileTypeIsModel(fD.fileType)).ToList();
        List<Model> models_ = new List<Model>(modelFiles.Count);
        List<MaterialLibrary> materialLibraries_ = new List<MaterialLibrary>(modelFiles.Count);
        for (int i = 0; i < modelFiles.Count; i++) {
            materialLibraries_.Add(new MaterialLibrary(fileData.Where(fD => fD.directory == modelFiles[i].directory).ToList()));
            models_.Add(new Model(modelFiles[i], materialLibraries_[i]));
        }
        for (int i = models_.Count-1; i > -1; i--) {
            if (models_[i].isEmpty) {
                models_.RemoveAt(i);
                materialLibraries_.RemoveAt(i);
                modelFiles.RemoveAt(i);
            }
            else {
                //models_[i].TrimUnused();
            //These settings work
                if (gameSettings.scaleFactor != 1) models_[i].Scale(gameSettings.scaleFactor);
                if (generalSettings.rounding != 0) models_[i].RoundVertices(generalSettings.rounding);
                if (gameSettings.reverseVertexOrder) models_[i].ReverseVertexOrder();
                if (gameSettings.swapYZCoordinates) models_[i].SwapYZCoordinates();
            //These settings are untested
                if (gameSettings.marathonCeilingFix) MarathonFix.Quick(models_[i]);
                if (gameSettings.subdivideFaces) models_[i].SubdivideFaces();
            }
        }
    //OUTPUT
        models = models_.ToArray();
        materialLibraries = materialLibraries_.ToArray();
        outputDirectories = modelFiles.Select(x => x.directory).ToArray();
        if (models_.Count == 0) return false;
        return true;
    }

}