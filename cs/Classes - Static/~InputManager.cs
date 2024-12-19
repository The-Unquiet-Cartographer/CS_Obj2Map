using ExtensionMethods;

public static class InputManager {

    private static List<Model.FileData> ListFileData (string input, out List<string> directories) {
        string[] inputs = input.Split('"', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        directories = new List<string>();
        return ListFileData_Recursive(inputs, ref directories);
    }
    private static List<Model.FileData> ListFileData_Recursive (string[] inputs, ref List<string> directories) {
        List<Model.FileData> fileData = new List<Model.FileData>();
        for (int i = 0; i < inputs.Length; i++) {
            string s = inputs[i];
            if (Directory.Exists(inputs[i])) {
                List<string> l = new List<string>();
                l.AddRange(Directory.GetFiles(inputs[i]));
                l.AddRange(Directory.GetDirectories(inputs[i]));
                fileData.AddRange(ListFileData_Recursive(l.ToArray(), ref directories));
            }
            else if (File.Exists(inputs[i])) {
                string fileName;
                string fileType;
                FileReader.GetNameAndExtension(inputs[i], out fileName, out fileType);
                if (Model.InputFormatIsValid(fileType)) {
                    string fileContents = FileReader.File_ReadContents(inputs[i]);
                    fileData.Add(new Model.FileData(fileName, fileType, fileContents));
                    directories.Add(inputs[i].Remove( inputs[i].LastIndexOfAny(new char[] {'\\', '/'}) ) );
                }
            }
        }
        return fileData;
    }


    public static bool Import_AsSingle (string input, GameSettings gameSettings, GeneralSettings generalSettings, out Model model, out string outputDirectory, out int filesConverted) {
        List<string> directories;
        List<Model.FileData> fileData = ListFileData(input, out directories);
        filesConverted = fileData.Count;
        model = Model.Amalgamate(fileData.ToArray());
        if (model.isEmpty) {
            outputDirectory = "";
            ConsoleManager.ResetConsole(ConsoleColor.Red, "Bad input (no valid files)");
            return false;
        }
        model.TrimUnused();
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
        outputDirectory = directories[0];
        return true;
    }


    public static bool Import_AsMultiple (string input, GameSettings gameSettings, GeneralSettings generalSettings, out Model[] models, out string[] outputDirectories) {
        List<string> directories;
        List<Model.FileData> fileData = ListFileData(input, out directories);
        List<Model> models_ = new List<Model>(fileData.Count);
        for (int i = 0; i < models_.Count; i++) {
            models_.Add(new Model(fileData[i]));
        }
        for (int i = models_.Count-1; i > -1; i--) {
            if (models_[i].isEmpty) {
                models_.RemoveAt(i);
                directories.RemoveAt(i);
            }
            else {
                models_[i].TrimUnused();
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
        outputDirectories = directories.ToArray();
        if (models_.Count == 0) return false;
        return true;
    }
}