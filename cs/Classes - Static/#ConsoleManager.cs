using System.Collections.Generic;
using System.Reflection;
using static ExtensionMethods.CollectionExtensions;

public static class ConsoleManager {

    private static GeneralSettings generalSettings = new GeneralSettings();
    private static GameSettings gameSettings = new GameSettings();

    private static class InputOption {
        public static char scaleFactor {get{return 's';}}
        public static char rounding {get{return 'r';}}
        public static char subdivideFaces {get{return 'f';}}
        public static char invertNormals {get{return 'n';}}
        public static char reverseVertexOrder {get{return 'v';}}
        public static char swapYZCoordinates {get{return 'c';}}
        public static char marathonCeilingFix {get{return 'x';}}
        public static char outputFormat {get{return 'o';}}
        public static char optimiseFormat {get{return 'm';}}
        public static char brushThickness {get{return 't';}}
        public static char singleOutput {get{return 'i';}}
        public static char excludeTexture {get{return 'e';}}
        public static char preset {get{return 'p';}}
    }

    private static class PresetOption {
        public static char theWorldIsNotEnough {get{return '7';}}
        public static char goldenEye007 {get{return 'g';}}
        public static char marathon {get{return 'm';}}
        public static char spyro {get{return 's';}}
        public static char starWarsJediKnight {get{return 'j';}}
    }



//////////////////////////////////////////////////
//
//  MAIN FUNCTION
//
//////////////////////////////////////////////////
    public static void Run () {
        DisplayOptions();
    }
    public static void ResetConsole (ConsoleColor messageColor = ConsoleColor.White, string message = "") {
        Console.ForegroundColor = ConsoleColor.White;
        DisplayOptions(messageColor, message);
    }
    private static void DisplayOptions (ConsoleColor messageColor = ConsoleColor.White, string message = "") {
        Console.Clear();

    //
    //  WELCOME TEXT
    //
        Console.WriteLine("Welcome to the Obj2Map converter!");
        Console.WriteLine();
        WriteColoredLine(ConsoleColor.Yellow, "* To convert a file, type the full path of the file wrapped in double quotes (\"), and press return.");
        WriteColoredLine(ConsoleColor.Yellow, "\t* You can enter multiple paths to process multiple files, so long as they are each wrapped in double quotes.");
        WriteColoredLine(ConsoleColor.Yellow, "\t* You must include the file extension for each file.");
        WriteColoredLine(ConsoleColor.Yellow, "\t* Alternatively, you can enter a folder path to process all model files contained therein (including subdirectories).");
        WriteColoredLine(ConsoleColor.Yellow, "* To change the output options, type the relevant argument, followed by a space, followed by a value, and press return.");
        Console.WriteLine();
        WriteColoredLine(ConsoleColor.Cyan, "Parameter:\t\t\tArgument:\tRange of values:\tCurrent value:");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Scale factor:\t\t\t "                +InputOption.scaleFactor        +"\t\t[-???...???]\t\t"             +gameSettings.scaleFactor);
        Console.WriteLine("Round to nearest [x] units:\t "      +InputOption.rounding           +"\t\t[-0.0000001...???]\t"         +generalSettings.rounding);
        Console.WriteLine("Reverse vertex order:\t\t "          +InputOption.reverseVertexOrder +"\t\t[true/false]\t\t"             +gameSettings.reverseVertexOrder);
        Console.WriteLine("Invert face normals:\t\t "           +InputOption.invertNormals      +"\t\t[true/false]\t\t"             +gameSettings.invertNormals);
        Console.WriteLine("Subdivide faces into triangles:\t "  +InputOption.subdivideFaces     +"\t\t[true/false]\t\t"             +gameSettings.subdivideFaces);
        Console.WriteLine("Swap YZ coordinates:\t\t "           +InputOption.swapYZCoordinates  +"\t\t[true/false]\t\t"             +gameSettings.swapYZCoordinates);
        Console.WriteLine("Marathon ceiling fix:\t\t "          +InputOption.marathonCeilingFix +"\t\t[true/false]\t\t"             +gameSettings.marathonCeilingFix);
        Console.WriteLine("Output format:\t\t\t "               +InputOption.outputFormat       +"\t\t[.map]\t\t\t"                 +'.'+generalSettings.outputFormat);
        if (generalSettings.outputFormat == GeneralSettings.OutputFormat.map) {
            Console.WriteLine("\tOptimise format:\t "               +InputOption.optimiseFormat     +"\t\t[WorldCraft/Radiant]\t"       +generalSettings.optimiseMap);
            Console.WriteLine("\tBrush thickness:\t "               +InputOption.brushThickness     +"\t\t[1...???]\t\t"                +generalSettings.brushThickness);
        }
        Console.WriteLine("Single output file:\t\t "            +InputOption.singleOutput       +"\t\t[true/false]\t\t"             +generalSettings.singleOutput);
        Console.WriteLine("Exclude textures:\t\t "              +InputOption.excludeTexture     +"\t\t[\"a.bmp\" \"b.bmp\" etc.]\t" +gameSettings.excludeTextures.Stringify());

    //DISPLAY SETTING PRESET OPTIONS
        {
            string SettingsMatchPreset(GameSettings preset) {
                bool b = ConsoleManager.gameSettings.MatchAll(preset);
                return b ? "<<<" : "";
            };
            WriteColoredLine(ConsoleColor.Cyan, "Preset options:");
            Console.WriteLine("Preset:\t\t\t\t"+InputOption.preset+"\t\t"   +PresetOption.theWorldIsNotEnough   +"\t007 The World Is Not Enough (N64)\t\t\t"            +SettingsMatchPreset(SettingsPresets.theWorldIsNotEnough));
            Console.WriteLine("\t\t\t\t\t\t"                                +PresetOption.goldenEye007          +"\tGoldenEye 007 / Perfect Dark\t\t\t"                 +SettingsMatchPreset(SettingsPresets.goldenEye007));
            Console.WriteLine("\t\t\t\t\t\t"                                +PresetOption.marathon              +"\tMarathon 1, 2, & ∞\t\t\t\t\t"                       +SettingsMatchPreset(SettingsPresets.marathon));
            Console.WriteLine("\t\t\t\t\t\t"                                +PresetOption.spyro                 +"\tSpyro 1, 2, & 3\t\t\t\t\t"                          +SettingsMatchPreset(SettingsPresets.spyro));
            Console.WriteLine("\t\t\t\t\t\t"                                +PresetOption.starWarsJediKnight    +"\tStar Wars Jedi Knight & Mysteries of the Sith\t"    +SettingsMatchPreset(SettingsPresets.starWarsJediKnight));
        }
    //DISPLAY OPTIONAL MESSAGE
        if (message != "") {
            Console.WriteLine();
            WriteColoredLine(messageColor, message);
        }
        Console.WriteLine();

    //
    //  PARSE INPUTS
    //
        string? input = Console.ReadLine();
        if (input != null) {
            input = input.Trim().ToLower();

        //OPTIONS
            if (input.Length > 2 && input[1] == ' ') {
                char _inputC = input[0];
            //No, you can't get a dictionary value by key and use it for a case-check in a switch statement.
                if (_inputC == InputOption.scaleFactor)             TryChangeOption_Float(input, ref gameSettings.scaleFactor);
                else if (_inputC == InputOption.rounding)           TryChangeOption_Float(input, ref generalSettings.rounding);
                else if (_inputC == InputOption.reverseVertexOrder) TryChangeOption_Bool(input, ref gameSettings.reverseVertexOrder);
                else if (_inputC == InputOption.invertNormals)      TryChangeOption_Bool(input, ref gameSettings.invertNormals);
                else if (_inputC == InputOption.subdivideFaces)     TryChangeOption_Bool(input, ref gameSettings.subdivideFaces);
                else if (_inputC == InputOption.swapYZCoordinates)  TryChangeOption_Bool(input, ref gameSettings.swapYZCoordinates);
                else if (_inputC == InputOption.marathonCeilingFix) TryChangeOption_Bool(input, ref gameSettings.marathonCeilingFix);
                else if (_inputC == InputOption.outputFormat)       TryChangeOption_OutputFormat(input);
                else if (_inputC == InputOption.singleOutput)       TryChangeOption_Bool(input, ref generalSettings.singleOutput);
                else if (_inputC == InputOption.excludeTexture)     TryChangeOption_ExcludeTextures(input);
                else if (_inputC == InputOption.preset)             TryChangeOption_Preset(input);
                else if (generalSettings.outputFormat == GeneralSettings.OutputFormat.map) {
                    if (_inputC == InputOption.brushThickness)          TryChangeOption_Int(input, ref generalSettings.brushThickness);
                    if (_inputC == InputOption.optimiseFormat)          TryChangeOption_Enum(input, ref generalSettings.optimiseMap);
                }
                else {
                    ResetConsole(ConsoleColor.Red, "Bad input (argument not recognised).");
                }
            }

        //KEYWORDS
            else if (input.Length == 4) {
                if (input == "quit" || input == "exit") {
                    return;
                }
                if (input == "more" || input == "info") {
                    return;
                }
            }

        //FILE PATH(S)
            else {
                string FormatOutputDirectory (string _outputDirectory) {
                    if (_outputDirectory.EndsWith('\\') || _outputDirectory.EndsWith('/')) return _outputDirectory;
                    return _outputDirectory+"/";
                }
                WriteColoredLine(ConsoleColor.Yellow, "Working...");
                if (generalSettings.singleOutput) {
                    Model model;
                    string outputDirectory;
                    int filesConverted;
                    if (InputManager.Import_AsSingle(input, gameSettings, generalSettings, out model, out outputDirectory, out filesConverted)) {
                        string output = OutputCompiler.Compile_MAP(model, gameSettings.invertNormals, generalSettings.brushThickness, generalSettings.optimiseMap);
                        string path = FormatOutputDirectory(outputDirectory)+model.name+".map";
                        FileWriter.WriteFile(path, output);
                        ResetConsole(ConsoleColor.Green, "Successfully converted "+filesConverted+" files to a single output.");
                        return;
                    }
                    else {
                        ResetConsole(ConsoleColor.Red, "Failed to convert "+filesConverted+" files to a single output.");
                        return;
                    }
                }
                else {
                    Model[] models;
                    string[] outputDirectories;
                    if (InputManager.Import_AsMultiple(input, gameSettings, generalSettings, out models, out outputDirectories)) {
                        for (int i = 0; i < models.Length; i++) {
                            string output = OutputCompiler.Compile_MAP(models[i], gameSettings.invertNormals, generalSettings.brushThickness, generalSettings.optimiseMap);
                            string path = FormatOutputDirectory(outputDirectories[i])+models[i].name+".map";
                            FileWriter.WriteFile(path, output);
                        }
                        ResetConsole(ConsoleColor.Green, "Successfully converted "+models.Length+" files.");
                        return;
                    }
                    else {
                        ResetConsole(ConsoleColor.Red, "Failed to convert "+models.Length+" files.");
                        return;
                    }
                }
            //End parse file path(s)
            }
        //End parse inputs
        }
    //End function
    }



//////////////////////////////////////////////////
//
//  UTILITIES
//
//////////////////////////////////////////////////
    public static void WriteColoredLine (ConsoleColor c, string s) {
            Console.ForegroundColor = c;
            Console.WriteLine(s);
            Console.ResetColor();
    }



//////////////////////////////////////////////////
//
//  CHANGE USER SETTINNGS
//
//////////////////////////////////////////////////
/*
    private static bool TryParse<T>(string value, out T result) where T : struct {
        var tryParseMethod = typeof(T).GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, new [] { typeof(string), typeof(T).MakeByRefType() });
        var parameters = new object[] { value, null };
        bool b = (bool)tryParseMethod.Invoke(null, parameters);
        result = (T)parameters[1];
        return b;
    }
*/
    private static void TryChangeOption_Float (string input, ref float field, float min = float.MinValue, float max = float.MaxValue) {
        float value;
        bool b = float.TryParse(input.Substring(2), out value);
        if (b) {
            field = Math.Clamp(value, min, max);
            ResetConsole();
        }
        else ResetConsole(ConsoleColor.Red, "Bad input (value).");
    }
    private static void TryChangeOption_Bool (string input, ref bool field) {
        bool value;
        bool b = bool.TryParse(input.Substring(2), out value);
        if (b) {
            field = value;
            ResetConsole();
        }
        else ResetConsole(ConsoleColor.Red, "Bad input (value).");
    }
    private static void TryChangeOption_Int (string input, ref int field, int min = int.MinValue, int max = int.MaxValue) {
        int value;
        bool b = int.TryParse(input.Substring(2), out value);
        if (b) {
            field = Math.Clamp(value, min, max);
            ResetConsole();
        }
        else ResetConsole(ConsoleColor.Red, "Bad input (value).");
    }
    private static void TryChangeOption_OutputFormat (string input) {
        if (input[0] == '.') input = input.Remove(0,1);
        GeneralSettings.OutputFormat value;
        bool b = Enum.TryParse(input, out value);
        if (b) {
            generalSettings.outputFormat = value;
            ResetConsole();
        }
        else ResetConsole(ConsoleColor.Red, "Bad input (value).");
    }
    private static void TryChangeOption_Enum<T> (string input, ref T field) where T : struct, Enum {
        if (input[0] == '.') input = input.Remove(0,1);
        T value;
        bool b = Enum.TryParse(input, out value);
        if (b) {
            field = value;
            ResetConsole();
        }
        else ResetConsole(ConsoleColor.Red, "Bad input (value).");
    }
    private static void TryChangeOption_ExcludeTextures (string input) {
        string[] inputs = input.Split('"', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        inputs = inputs.Distinct().ToArray();
        for (int i = 1; i < inputs.Length; i++) {
            bool removed = false;
            for (int j = gameSettings.excludeTextures.Count-1; j > -1; j--) {
                if (gameSettings.excludeTextures[j] == inputs[i]) {
                    gameSettings.excludeTextures.RemoveAt(j);
                    removed = true;
                }
            }
            if (!removed) gameSettings.excludeTextures.Add(inputs[i]);
        }
        ResetConsole();
    }


    private static void TryChangeOption_Preset (string input) {
        if (input.Length > 3) {
            ResetConsole(ConsoleColor.Red, "Bad input (value).");
            return;
        }
        void SetAndReturn (GameSettings preset) {
            ConsoleManager.gameSettings = preset;
            ResetConsole();
            return;
        }
        char value = input[2];
        if (value == PresetOption.theWorldIsNotEnough)      SetAndReturn(SettingsPresets.theWorldIsNotEnough);
        else if (value == PresetOption.goldenEye007)        SetAndReturn(SettingsPresets.goldenEye007);
        else if (value == PresetOption.marathon)            SetAndReturn(SettingsPresets.marathon);
        else if (value == PresetOption.spyro)               SetAndReturn(SettingsPresets.spyro);
        else if (value == PresetOption.starWarsJediKnight)  SetAndReturn(SettingsPresets.starWarsJediKnight);
        else ResetConsole(ConsoleColor.Red, "Bad input (value).");
    }



}