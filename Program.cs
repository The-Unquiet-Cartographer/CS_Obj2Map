using ExtensionMethods;
using static ExtensionMethods.FloatExtensions;

bool runTestCode = false;



if (runTestCode) {

    Material testMaterial = new Material("testMat", "texture", "bmp", 64, 64);
    Model testModel_wedge = new Model(
        new Vector3[]{new Vector3(0, 0, 0), new Vector3(-32, 0, 64), new Vector3(128, 0, 32)},
        new Vector2[]{new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0)},
        new Face[]{new Face(new Face.IPair(0, 0), new Face.IPair(1, 1), new Face.IPair(2, 2))}
    );
    string debugText;
    Console.WriteLine(Extrapolation.ExtrapolateMapTextureInfo(testModel_wedge, 0, testMaterial, false, out debugText));
}



else {
    ConsoleManager.Run();
}